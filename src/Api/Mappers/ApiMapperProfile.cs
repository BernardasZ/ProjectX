using Api.DTOs.Login;
using Api.DTOs.Task;
using Api.DTOs.User;
using Application.Models.Login;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Api.Mappers;

public class ApiMapperProfile : Profile
{
	public ApiMapperProfile()
	{
		CreateMap<UserModel, UserDto>()
			.ForMember(dest => dest.Role, opt => opt.MapFrom(s => s.Role))
			.ReverseMap()
			.ForMember(dest => dest.Role, opt => opt.MapFrom(s => s.Role));

		CreateMap<TaskModel, TaskDto>()
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(s => s.User))
			.ReverseMap()
			.ForMember(dest => dest.User, opt => opt.MapFrom(s => s.UserId));

		CreateMap<RoleModel, UserRole>()
			.ConvertUsing(s => s.Value);

		CreateMap<UserRole, RoleModel>()
			.ForMember(dest => dest.Value, opt => opt.MapFrom(s => s));

		CreateMap<UserModel, int>()
			.ConvertUsing(s => s.Id.Value);

		CreateMap<int, UserModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s));

		CreateMap<int, ModelBase>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s == 0 ? (int?)null : s));

		CreateMap<UserCreateDto, UserModel>();
		CreateMap<UserUpdateDto, UserModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.UserId));

		CreateMap<UserDeleteDto, UserModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.UserId));

		CreateMap<UserModel, UserResponseDto>();
		CreateMap<UserModel, UserLoginResponseModel>();

		CreateMap<UserLoginDto, UserLoginModel>();
		CreateMap<UserChangePasswordDto, UserChangePasswordModel>();
		CreateMap<UserResetPasswordDto, UserResetPasswordModel>();
		CreateMap<InitPasswordResetDto, InitPasswordResetModel>();
		CreateMap<UserLoginResponseModel, UserLoginResponseDto>();

		CreateMap<TaskCreateDto, TaskModel>()
			.ForMember(dest => dest.User, opt => opt.MapFrom(s => s.UserId));

		CreateMap<TaskUpdateDto, TaskModel>()
			.ForMember(dest => dest.User, opt => opt.MapFrom(s => s.UserId));

		CreateMap<TaskDeleteDto, TaskModel>()
			.ForMember(dest => dest.User, opt => opt.MapFrom(s => s.UserId));
	}
}