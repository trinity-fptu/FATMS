using Application.Repositories;
using AutoFixture;
using Domain.Models.Users;
using Domain.Tests;
using FluentAssertions;
using Infracstructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class GenericRepositoryTests : SetupTest
    {
        private readonly IGenericRepository<User> _genericRepository;
        private Fixture _customFixture;

        public GenericRepositoryTests()
        {
            _genericRepository = new GenericRepository<User>(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
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

        //#region ToPaginationAsync
        //[Fact]
        //public async Task ToPaginationAsync_ValidParametersAndData_ReturnFirstPage()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();
        //    int pageSize = 10, pageIndex = 0;

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        //    //Assert
        //    result.PageIndex.Should().Be(pageIndex);
        //    result.PageSize.Should().Be(pageSize);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeTrue();
        //    result.Previous.Should().BeFalse();
        //    result.Items.Count.Should().Be(pageSize);
        //}

        //[Fact]
        //public async Task ToPaginationAsync_ValidParametersAndData_ReturnSecondPage()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();
        //    int pageSize = 10, pageIndex = 1;

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        //    //Assert
        //    result.PageIndex.Should().Be(pageIndex);
        //    result.PageSize.Should().Be(pageSize);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeTrue();
        //    result.Previous.Should().BeTrue();
        //    result.Items.Count.Should().Be(pageSize);
        //}

        //[Fact]
        //public async Task ToPaginationAsync_ValidParametersAndData_ReturnLastPage()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();
        //    int pageSize = 10, pageIndex = 2;

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        //    //Assert
        //    result.PageIndex.Should().Be(pageIndex);
        //    result.PageSize.Should().Be(pageSize);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeFalse();
        //    result.Previous.Should().BeTrue();
        //    result.Items.Count.Should().Be(pageSize);
        //}

        //[Fact]
        //public async Task ToPaginationAsync_NoParametersPassed_ReturnPageWithDefaultParameters()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync();

        //    //Assert
        //    result.PageIndex.Should().Be(0);
        //    result.PageSize.Should().Be(10);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeTrue();
        //    result.Previous.Should().BeFalse();
        //    result.Items.Count.Should().Be(10);
        //}

        //[Fact]
        //public async Task ToPaginationAsync_OutOfRangeMinTotalPage_ReturnFirstPageWithWrongPageIndex()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();
        //    int pageSize = 10, pageIndex = -2;

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        //    //Assert
        //    result.PageIndex.Should().Be(-2);
        //    result.PageSize.Should().Be(pageSize);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeTrue();
        //    result.Previous.Should().BeFalse();
        //    result.Items.Count.Should().Be(10);
        //}

        //[Fact]
        //public async Task ToPaginationAsync_OutOfRangeMaxTotalPage_ReturnLastPage()
        //{
        //    //Assign
        //    var users = _customFixture.Build<User>().CreateMany(30).ToList();
        //    await _genericRepository.AddRangeAsync(users);
        //    await _dbContext.SaveChangesAsync();
        //    int pageSize = 10, pageIndex = 10;

        //    //Act
        //    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        //    //Assert
        //    result.PageIndex.Should().Be(2);
        //    result.PageSize.Should().Be(pageSize);
        //    result.TotalItemCount.Should().Be(users.Count);
        //    result.Next.Should().BeFalse();
        //    result.Previous.Should().BeTrue();
        //    result.Items.Count.Should().Be(pageSize);
        //}

        ////[Fact]
        ////public async Task ToPaginationAsync_OutOfRangeMinPageSize_ReturnPageWithMinPageSize()
        ////{
        ////    //Assign
        ////    var users = _customFixture.Build<User>().CreateMany(5).ToList();
        ////    await _genericRepository.AddRangeAsync(users);
        ////    await _dbContext.SaveChangesAsync();
        ////    int pageSize = 0, pageIndex = 3;

        ////    //Act
        ////    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        ////    //Assert
        ////    result.PageIndex.Should().Be(3);
        ////    result.PageSize.Should().Be(1);
        ////    result.TotalItemCount.Should().Be(users.Count);
        ////    result.Next.Should().BeTrue();
        ////    result.Previous.Should().BeTrue();
        ////    result.Items.Count.Should().Be(pageSize);
        ////}

        ////[Fact]
        ////public async Task ToPaginationAsync_OutOfRangeMaxPageSize_ReturnPageWithMaxPageSize()
        ////{
        ////    //Assign
        ////    var users = _customFixture.Build<User>().CreateMany(101).ToList();
        ////    await _genericRepository.AddRangeAsync(users);
        ////    await _dbContext.SaveChangesAsync();
        ////    int pageSize = 101, pageIndex = 0;

        ////    //Act
        ////    var result = await _genericRepository.ToPaginationAsync(pageIndex, pageSize);

        ////    //Assert
        ////    result.PageIndex.Should().Be(0);
        ////    result.PageSize.Should().Be(100);
        ////    result.TotalItemCount.Should().Be(users.Count);
        ////    result.Next.Should().BeTrue();
        ////    result.Previous.Should().BeFalse();
        ////    result.Items.Count.Should().Be(100);
        ////}
        //#endregion

        //#region AddAsync
        //[Fact]
        //public async Task GenericRepository_AddAsync_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<User>();

        //    //Act
        //    await _genericRepository.AddAsync(mock);
        //    var result = await _dbContext.SaveChangesAsync();

        //    //Assert
        //    Assert.Equal(1, result);
        //}
        //#endregion

        //#region AddRangeAsync
        //[Fact]
        //public async Task GenericRepository_AddRangeAsync_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.CreateMany<User>(10);

        //    //Act
        //    await _genericRepository.AddRangeAsync(mock.ToList());
        //    var result = await _dbContext.SaveChangesAsync();

        //    //Assert
        //    Assert.Equal(10, result);
        //}
        //#endregion

        //#region GetAllAsync
        //[Fact]
        //public async Task GenericRepository_GetAllAsync_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.CreateMany<User>(10);
        //    await _genericRepository.AddRangeAsync(mock.ToList());
        //    await _dbContext.SaveChangesAsync();

        //    //Act
        //    var result = await _genericRepository.GetAllAsync();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.NotEmpty(result);
        //    Assert.Equal(10, result.Count);
        //    Assert.True(mock.SequenceEqual(result));
        //}

        //[Fact]
        //public async Task GenericRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        //{
        //    //Act
        //    var result = await _genericRepository.GetAllAsync();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}
        //#endregion

        //#region GetByIdAsync
        //[Fact]
        //public async Task GenericRepository_GetByIdAsync_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<User>();
        //    await _genericRepository.AddAsync(mock);
        //    await _dbContext.SaveChangesAsync();

        //    //Act
        //    var result = await _genericRepository.GetByIdAsync(mock.Id);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Same(mock, result);
        //}

        //[Fact]
        //public async Task GenericRepository_GetByIdAsync_ShouldReturnNullWhenHaveNoData()
        //{
        //    //Act
        //    var result = await _genericRepository.GetByIdAsync(0);

        //    //Assert
        //    Assert.Null(result);
        //}
        //#endregion

        //#region Update
        //[Fact]
        //public async Task GenericRepository_Update_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<User>();
        //    await _genericRepository.AddAsync(mock);
        //    await _dbContext.SaveChangesAsync();

        //    //Act
        //    mock.FullName = "Test";
        //    _genericRepository.Update(mock);
        //    var result = await _dbContext.SaveChangesAsync();
        //    var objectResult = await _genericRepository.GetByIdAsync(mock.Id);

        //    //Assert
        //    Assert.Equal(1, result);
        //    Assert.Equal("Test", objectResult!.FullName);
        //}
        //#endregion

        //#region UpdateRange
        //[Fact]
        //public async Task GenericRepository_UpdateRange_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var mock = _customFixture.CreateMany<User>(10);
        //    await _genericRepository.AddRangeAsync(mock.ToList());
        //    await _dbContext.SaveChangesAsync();

        //    //Act
        //    _genericRepository.UpdateRange(mock.ToList());
        //    var result = await _dbContext.SaveChangesAsync();

        //    //Assert
        //    Assert.Equal(10, result);
        //}
        //#endregion
    }
}
