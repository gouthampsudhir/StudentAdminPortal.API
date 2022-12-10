using StudentAdminPortal.API.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();

        Task<Student> GetStudentByIdAsync(Guid studentId);

        Task<List<Gender>> GetGendersAsync();

        Task<bool> StudentExistsCheck(Guid studentId);

        Task<Student> UpdateStudentAsync(Guid studentId, Student request);

        Task<Student> DeleteStudentByIdAsync(Guid studentId);

        Task<Student> AddStudentAsync(Student request);

        Task<bool> UpdateStudentProfileImage(Guid studentId, string profileImageUrl);
    }
}
