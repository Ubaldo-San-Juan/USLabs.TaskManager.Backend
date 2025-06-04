using AutoMapper;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Shared.DTOs.Auth;
using USLabs.TaskManager.Shared.DTOs.Categories;
using USLabs.TaskManager.Shared.DTOs.Tasks;

namespace USLabs.TaskManager.Business.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Will be handled in service


            // Category mappings
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.TaskItems != null ? src.TaskItems.Count : 0))
                .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src =>
                    src.TaskItems != null ? src.TaskItems.Count(t => t.Status == Shared.Enums.TaskStatusU.Completed) : 0))
                .ForMember(dest => dest.PendingTaskCount, opt => opt.MapFrom(src =>
                    src.TaskItems != null ? src.TaskItems.Count(t => t.Status == Shared.Enums.TaskStatusU.Pending) : 0));

            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Will be set in service

            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItems, opt => opt.Ignore());


            // Task mappings
            CreateMap<TaskItem, TaskDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<CreateTaskDTO, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Shared.Enums.TaskStatusU.Pending))
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateTaskDTO, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => 
                    src.Status == Shared.Enums.TaskStatusU.Completed ? DateTime.UtcNow : (DateTime?)null))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }        
    }
}