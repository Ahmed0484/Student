using api.DataModels;
using api.DTOs;
using api.Profiles.AfterMaps;
using AutoMapper;

namespace api.Profiles
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<UpdateStudentDTO, Student>()
                .AfterMap<UpdateStudentAfterMap>();
            CreateMap<AddStudentDTO, Student>()
                .AfterMap<AddStudentAfterMap>();

        }
    }
}
