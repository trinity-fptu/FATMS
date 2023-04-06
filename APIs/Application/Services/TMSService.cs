using Application.Interfaces;
using Application.ViewModels.MailViewModels;
using Application.ViewModels.TimeManagementSystemViewModels;
using Domain.Enums.AttendanceEnums;
using Domain.Enums.TMSEnums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class TMSService : ITMSService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IClaimsService _claimsService;

        public TMSService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IEmailService emailService, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _emailService = emailService;
            _claimsService = claimsService;
        }

        //action

        public async Task<bool> CreateAbsentRequestAsync(string reason)
        {
            var tms = new TMS()
            {
                Time = DateTime.Now,
                Reason = reason,
                TraineeId = _claimsService.GetCurrentUserId,
                ApproveStatus = TMSApproveStatus.Pending
            };

            await _unitOfWork.TimeMngSystemRepo.AddAsync(tms);

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result ? true : false;

        }

        public async Task<bool> ApproveAbsentRequestAsync(int id, string status)
        {
            var checkIdTMS = await _unitOfWork.TimeMngSystemRepo.GetByIdAsync(id);

            if (Enum.TryParse(status, true, out TMSApproveStatus newStatus))
            {
                checkIdTMS.ApproveStatus = newStatus;
                checkIdTMS.CheckedBy = _claimsService.GetCurrentUserId;
            }
            else
            {
                throw new Exception("Input must be Approved or Declined!");
            }

            if (newStatus == TMSApproveStatus.Pending)
            {
                return false;
            }

            _unitOfWork.TimeMngSystemRepo.Update(checkIdTMS);

            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<List<TMSListViewModel>> GetAllAbsentRequestsAsync()
        {
            var list = await _unitOfWork.TimeMngSystemRepo.GetAllAsync(x => x.Include(x => x.Trainee).Include(x => x.Admin));

            var listViewModel = new List<TMSListViewModel>();

            foreach (var tms in list)
            {
                var item = new TMSListViewModel
                {
                    Id = tms.Id,
                    Time = tms.Time,
                    Reason = tms.Reason,
                    TraineeName = tms.Trainee.FullName,
                    TraineeEmail = tms.Trainee.Email,
                    TraineePhone = tms.Trainee.Phone,
                    ApproveStatus = tms.ApproveStatus.ToString()
                };

                if (tms.Admin != null)
                {
                    item.CheckAdminName = tms.Admin.FullName;
                    item.CheckAdminEmail = tms.Admin.Email;
                }

                listViewModel.Add(item);
            }

            return listViewModel;
        }

        public async Task<List<TMSPendingListViewModel>> GetAllPendingAbsentRequestsAsync()
        {
            var list = await _unitOfWork.TimeMngSystemRepo.GetAllAsync(x => x.Include(x => x.Trainee));

            var pendingList = list.FindAll(x => x.ApproveStatus == TMSApproveStatus.Pending);

            var listViewModel = new List<TMSPendingListViewModel>();

            foreach (var tms in pendingList)
            {
                var item = new TMSPendingListViewModel
                {
                    Id = tms.Id,
                    Time = tms.Time,
                    Reason = tms.Reason,
                    TraineeName = tms.Trainee.FullName,
                    TraineeEmail = tms.Trainee.Email,
                    TraineePhone = tms.Trainee.Phone
                };

                listViewModel.Add(item);
            }

            return listViewModel;
        }

        public async Task<List<TMSListViewModel>> SearchAbsentRequest(string searchBy, string searchElement)
        {
            var list = await _unitOfWork.TimeMngSystemRepo.GetAllAsync(x => x.Include(x => x.Trainee));

            var listSearch = new List<TMS>();
            if (string.Equals(searchBy, "Name", StringComparison.OrdinalIgnoreCase))
            {
                listSearch = list.FindAll(x => x.Trainee.FullName.ToLower().Contains(searchElement.ToLower()));
            }
            else if (string.Equals(searchBy, "Email", StringComparison.OrdinalIgnoreCase))
            {
                listSearch = list.FindAll(x => x.Trainee.Email.ToLower().Contains(searchElement.ToLower()));
            }
            else if (string.Equals(searchBy, "Phone", StringComparison.OrdinalIgnoreCase))
            {
                listSearch = list.FindAll(x => x.Trainee.Phone.Contains(searchElement));
            }

            var listViewModel = new List<TMSListViewModel>();

            foreach (var tms in listSearch)
            {
                var item = new TMSListViewModel
                {
                    Id = tms.Id,
                    Time = tms.Time,
                    Reason = tms.Reason,
                    TraineeName = tms.Trainee.FullName,
                    TraineeEmail = tms.Trainee.Email,
                    TraineePhone = tms.Trainee.Phone,
                    ApproveStatus = tms.ApproveStatus.ToString()
                };

                listViewModel.Add(item);
            }

            return listViewModel;
        }

        public async Task<TMS> GetAbsentRequestByIdAsync(int id) => await _unitOfWork.TimeMngSystemRepo.GetByIdAsync(id);

        public async Task SendAttendanceMailAsync()
        {
            var absentees = await _unitOfWork.AttendancesRepo.GetAbsenteeListAsync();

            // Group absentees by trainer
            var absenteesByTrainer = absentees
                .GroupBy(a => a.ClassUser.Class.TrainingProgram.CreatedAdmin)
                .ToDictionary(g => g.Key, g => g.Select(a => a.ClassUser.User).ToList());

            // Send email to each trainer and their absent trainees
            foreach (var entry in absenteesByTrainer)
            {
                var trainer = entry.Key;
                var trainees = entry.Value;

                var to = new List<string>();
                var title = $"Attendance Report for {DateTime.Today:d}";
                var speech = $"Dear {trainer.FullName},<br><br>The following trainees were absent in your class today ({DateTime.Today:d}):<br>";
                var mainContent = "Hello";

                foreach (var trainee in trainees)
                {
                    speech += $"- {trainee.FullName} ({trainee.Email})<br>";
                    to.Add(trainee.Email);
                }

                to.Add(trainer.Email);
                //to.Add("khangndse161725@fpt.edu.vn");

                var sign = "Admin";
                var body = _emailService.GetMailBody(title, speech, mainContent, sign: sign);
                var subject = $"Attendance Report for {DateTime.Today:d}";
                var mailData = new MailDataModel(to, subject, body);

                // Send email
                await _emailService.SendAsync(mailData, new CancellationToken());
            }
        }
    }
}