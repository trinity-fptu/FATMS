using Application.Interfaces;
using Application.IValidators;
using Application.ViewModels.TrainingMaterialViewModels;
using AutoMapper;
using Domain.Models;
using Domain.Models.Users;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Application.Services
{
    public class TrainingMaterialService : ITrainingMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;

        private readonly int NotFound = -1;
        private readonly string BaseUrl = "training-materials";

        // Configure Firebase
        private readonly string apiKey = "AIzaSyB-6fkuevpObM-5lFuks8idOfceeHGSCso";
        private readonly string authDomain = "fatms-679c1.firebaseapp.com";
        private readonly string bucket = "fatms-679c1.appspot.com";
        private readonly string authEmail = "datlt.mdc@gmail.com";
        private readonly string authPassword = "Fireb@se";


        public TrainingMaterialService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsService claimsService,
            ICurrentTime currentTime,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _configuration = configuration;
        }
        public async Task AddTrainingMaterialAsync(int lectureId, IFormFile fileModel)
        {
            var createUserId = _claimsService.GetCurrentUserId;
            if (createUserId == NotFound) createUserId = 1;
            //throw new Exception("Not authenticated");

            var fileUrl = await UploadAsyncToFirebase(fileModel, $"{BaseUrl}/{lectureId}");
            if (fileUrl.IsNullOrEmpty()) throw new Exception("Cannot upload file to Firebase");

            var trainingMaterial = new TrainingMaterial()
            {
                Name = fileModel.FileName,
                CreatedBy = createUserId,
                CreatedOn = _currentTime.GetCurrentTime(),
                LectureId = lectureId,
                Url = fileUrl,
                isDeleted = false,
            };
            await _unitOfWork.TrainingMaterialRepo.AddAsync(trainingMaterial);
            var addSuccess = await _unitOfWork.SaveChangesAsync() > 0;          
        }

        public async Task<string> UploadAsyncToFirebase(IFormFile fileModel, string baseUrl)
        {
            // Firebase uploading stuffs
            var authConfig = new FirebaseAuthConfig
            {
                ApiKey = apiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                }
            };
            var client = new FirebaseAuthClient(authConfig);

            var userCredential = await client
                .SignInWithEmailAndPasswordAsync(authEmail, authPassword);
            var token = await userCredential.User.GetIdTokenAsync();

            var cancellation = new CancellationTokenSource();
            var firebaseTask = new FirebaseStorage(
                bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token),
                    ThrowOnCancel = true
                })
                .Child($"{baseUrl}/{fileModel.FileName}")
                .PutAsync(fileModel.OpenReadStream(), cancellation.Token);
            firebaseTask.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            return await firebaseTask;
        }
    }
}
