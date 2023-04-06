using Application.Interfaces;
using Quartz;

namespace Application.Cronjob
{
    public class SendAttendanceMailJob : IJob
    {
        private readonly ITMSService _tMSService;

        public SendAttendanceMailJob(ITMSService tMSService)
        {
            _tMSService = tMSService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _tMSService.SendAttendanceMailAsync();
        }
    }
}
