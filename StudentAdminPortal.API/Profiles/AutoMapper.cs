using AutoMapper;
using DataModels = StudentAdminPortal.API.DataModels;
using DomainModels = StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapper: Profile
    {
        public AutoMapper()
        {
            CreateMap<DomainModels.Student, DataModels.Student>().ReverseMap();

            CreateMap<DomainModels.Gender, DataModels.Gender>().ReverseMap();

            CreateMap<DomainModels.Address, DataModels.Address>().ReverseMap(); 
        }
    }
}
