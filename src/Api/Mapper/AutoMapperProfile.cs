using Api.Models.Task;
using Api.Models.User;
using AutoMapper;
using Persistence.Entities.ProjectX;

namespace Api.Mapper;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<User, UserModel>()
			.ForMember(dest => dest.Role, opt => opt.MapFrom(s => s.Role.Value))
			.ReverseMap()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id == 0 ? (int?)null : s.Id));

		CreateMap<Task, TaskModel>()
			.ReverseMap()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id == 0 ? (int?)null : s.Id));
	}
}