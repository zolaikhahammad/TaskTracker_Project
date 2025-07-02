using AutoMapper;
using TaskTracker.Application;
using TaskTracker.Domain.Entities;

namespace TaskTrackerAPI.MappingProfile
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskItem, TaskDto>()
                .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.TaskPriority.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category));

            CreateMap<TaskCreateDto, TaskItem>();
            CreateMap<TaskUpdateDto, TaskItem>();
        }
    }

}
