using Application.Commons;
using AutoFixture;
using Domain.Tests;

namespace Applications.Tests.Commons
{
    public class PaginationTests : SetupTest
    {
        [Fact]
        public void Pagination_PaginationFirstPage_ShouldReturnExpectedObject()
        {
            // arrange
            var mockItems = _fixture.Build<string>().CreateMany(100).ToList();

            // act
            var pagination = new Pagination<string>
            {
                Items = mockItems,
                PageSize = 10,
                TotalItemCount = mockItems.Count,
                PageIndex = 0,
                
            };
            //pagination.Items = mockItems;

            // assert
            Assert.False(pagination.Previous);
            Assert.True(pagination.Next);
            Assert.NotNull(pagination.Items);
            Assert.NotEmpty(pagination.Items);
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(0, pagination.PageIndex);
            Assert.Equal(100, pagination.TotalItemCount);
            Assert.Equal(10, pagination.TotalPagesCount);
        }

        [Fact]
        public void Pagination_PaginationSecondPage_ShouldReturnExpectedObject()
        {
            // arrange
            var mockItems = _fixture.Build<string>().CreateMany(100).ToList();

            // act
            var pagination = new Pagination<string>
            {
                Items = mockItems,
                PageSize = 10,
                TotalItemCount = mockItems.Count,
                PageIndex = 1,
                
            };

            // assert
            Assert.True(pagination.Previous);
            Assert.True(pagination.Next);
            Assert.NotNull(pagination.Items);
            Assert.NotEmpty(pagination.Items);
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(1, pagination.PageIndex);
            Assert.Equal(100, pagination.TotalItemCount);
            Assert.Equal(10, pagination.TotalPagesCount);
        }

        [Fact]
        public void Pagination_PaginationLastPage_ShouldReturnExpectedObject()
        {
            // arrange
            var mockItems = _fixture.Build<string>().CreateMany(101).ToList();

            // act
            var pagination = new Pagination<string>
            {
                Items = mockItems,
                PageSize = 10,
                TotalItemCount = mockItems.Count,
                PageIndex = 10,
            };

            // assert
            Assert.True(pagination.Previous);
            Assert.False(pagination.Next);
            Assert.NotNull(pagination.Items);
            Assert.NotEmpty(pagination.Items);
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(10, pagination.PageIndex);
            Assert.Equal(101, pagination.TotalItemCount);
            Assert.Equal(11, pagination.TotalPagesCount);
        }

        [Fact]
        public void Pagination_PageIndexOutOfRang_ReturnLastPage()
        {
            // arrange
            var mockItems = _fixture.Build<string>().CreateMany(101).ToList();
            int pageIndex = 11;

            // act
            var pagination = new Pagination<string>
            {
                PageSize = 10,
                TotalItemCount = mockItems.Count,
                PageIndex = pageIndex,
                Items = mockItems
            };

            // assert
            Assert.True(pagination.Previous);
            Assert.False(pagination.Next);
            Assert.NotNull(pagination.Items);
            Assert.NotEmpty(pagination.Items);
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(10, pagination.PageIndex);
            Assert.Equal(101, pagination.TotalItemCount);
            Assert.Equal(11, pagination.TotalPagesCount);
        }
    }
}
