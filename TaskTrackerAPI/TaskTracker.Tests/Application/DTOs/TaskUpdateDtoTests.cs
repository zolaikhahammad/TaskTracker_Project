using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Application;

namespace TaskTracker.Tests.Application
{
    public class TaskUpdateDtoTests
    {
        #region Blue Sky TestCases

        [Fact]
        public void Should_PassValidation_When_ValidTaskUpdateDto()
        {
            var dto = new TaskUpdateDto
            {
                Title = "Updated Task",
                Description = "Updated description",
                PriorityId = 2,
                DueDate = DateTime.UtcNow.AddDays(1),
                IsCompleted = false
            };

            var results = ValidateModel(dto);

            results.Should().BeEmpty();
        }

        #endregion

        #region Non-BlueSky Test Cases

        [Fact]
        public void Should_FailValidation_When_Title_IsMissing()
        {
            var dto = new TaskUpdateDto
            {
                Title = "",
                Description = "Description",
                PriorityId = 1,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var results = ValidateModel(dto);
            results.Should().Contain(r => r.MemberNames.Contains("Title"));
        }

        [Fact]
        public void Should_FailValidation_When_Description_IsMissing()
        {
            var dto = new TaskUpdateDto
            {
                Title = "Task",
                Description = "",
                PriorityId = 1,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var results = ValidateModel(dto);
            results.Should().Contain(r => r.MemberNames.Contains("Description"));
        }

        #endregion

        #region Helper Method

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
