using Application.Repositories;
using AutoFixture;
using Domain.Models.Users;
using Domain.Tests;
using Infracstructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class UserRepositoryTests : SetupTest
    {
        private readonly IUserRepository _userRepository;
        private Fixture _customFixture;
        
        public UserRepositoryTests()
        {
            _userRepository = new UserRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "Role",
                "TimeMngSystem",
                "ModifyTrainingProgram",
                "CreatedTrainingProgram",
                "CreatedSyllabus",
                "ModifiedSyllabus",
                "ApprovedClass",
                "CreatedClass",
                "FeedbackTrainee",
                "FeedbackTrainer",
                "GradeReports",
                "ClassUsers",
                "AuditPlans",
                "AuditDetailsAuditor",
                "AuditDetailsTrainee",
                "TimeMngSystemList",
                "TrainingMaterials"));
        }

        #region GetUserByUserEmailAndPasswordHash
        //[Fact]
        public async Task GetUserByUserEmailAndPasswordHash_ShouldReturnCorrectData_WhenUserEmailAndPasswordIsCorrect()
        {
            //Assign
            var mock = _customFixture.Create<User>();
            await _dbContext.AddAsync(mock);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _userRepository.GetUserByUserEmailAndPasswordHash(mock.Email, mock.Password);

            //Assert
            Assert.Same(mock, result);
        }

        //[Fact]
        public async Task GetUserByUserEmailAndPasswordHash_ShouldThrowException_WhenUserEmailAndPasswordIsIncorrect()
        {
            //Assign
            string email = "test@gmail.com";
            string password = "testpassword";

            //Assert
            var result = async () => await _userRepository.GetUserByUserEmailAndPasswordHash(email, password);
            var exception = await Record.ExceptionAsync(result);

            //Act
            await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal("Email or Password is incorrect.", exception.Message);
        }
        #endregion

        #region IsExistsUser
        // [Fact]
        public async Task IsExistsUser_ShouldReturnTrue_WhenUserEmailIsExists()
        {
            //Assign
            var mock = _customFixture.Create<User>();
            await _dbContext.AddAsync(mock);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _userRepository.IsExistsUser(mock.Email);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsExistsUser_ShouldReturnFalse_WhenUserEmailDoesntExists()
        {
            //Assign
            string email = "test@gmail.com";

            //Act
            var result = await _userRepository.IsExistsUser(email);

            //Assert
            Assert.False(result);
        }
        #endregion
    }
}
