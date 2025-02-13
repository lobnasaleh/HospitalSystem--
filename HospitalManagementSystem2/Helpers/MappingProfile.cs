using AutoMapper;
using HMS.Entites.Contracts;
using HMS.Entites.Enums;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;

namespace HMS.web.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
             CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<RegisterStaffRequest, RegisterStaffRequestVM>().ReverseMap();
            CreateMap<RegisterRequest, RegisterRequestVM>().ReverseMap();
            //  CreateMap<RegisterRequestVM, Patient>().ReverseMap();

            CreateMap<RegisterRequestVM, RegisterRequest>().ReverseMap();
            CreateMap<LoginRequestVM, LoginRequest>().ReverseMap();
            CreateMap<AuthResponseVM, AuthResponse>().ReverseMap();






        }
    }
}
