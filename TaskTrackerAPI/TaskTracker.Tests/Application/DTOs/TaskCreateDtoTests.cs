using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Application;

namespace TaskTracker.Tests.Application
{
    public class TaskCreateDtoTests
    {
        #region BlueSky TestCases

        [Fact]
        public void Should_Validate_When_Valid_TaskCreateDto()
        {
            var dto = new TaskCreateDto
            {
                Title = "Test Task",
                Description = "Valid description",
                TaskPriorityId = 1,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var results = ValidateModel(dto);

            results.Should().BeEmpty();
        }

        #endregion

        #region Non-BlueSky TestCases

        [Fact]
        public void Should_Invalidate_When_Title_Is_Missing()
        {
            var dto = new TaskCreateDto
            {
                Title = "",
                Description = "Description",
                TaskPriorityId = 1,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var results = ValidateModel(dto);

            results.Should().Contain(r => r.MemberNames.Contains("Title"));
        }

        [Fact]
        public void Should_Invalidate_When_Description_Is_Too_Long()
        {
            var dto = new TaskCreateDto
            {
                Title = "Test",
                Description = new string('x', 501),
                TaskPriorityId = 1,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var results = ValidateModel(dto);

            results.Should().Contain(r => r.MemberNames.Contains("Description"));
        }

        #endregion

        #region Helper

        private List<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        #endregion
    }
}
