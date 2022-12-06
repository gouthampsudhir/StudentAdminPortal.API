using AutoMapper;
using StudentAdminPortal.API.Profiles.AfterMaps;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapper: Profile
    {
        public AutoMapper()
        {
            CreateMap<Student, DataModels.Student>()
                .ReverseMap();

            CreateMap<Gender, DataModels.Gender>()
                .ReverseMap();

            CreateMap<Address, DataModels.Address>()
                .ReverseMap();

            CreateMap<UpdateStudentRequest, DataModels.Student>()
                .AfterMap<UpdateStudentRequestAfterMap>();
        }
    }
}
