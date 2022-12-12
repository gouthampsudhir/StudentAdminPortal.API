using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<Student>>(students));
        }


        [HttpGet]
        [Route("[controller]/{studentId:guid}"), ActionName("GetOneStudentAsync")]
        public async Task<IActionResult> GetOneStudentAsync([FromRoute] Guid studentId)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);

            if(student==null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateOneStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {

            if(await studentRepository.StudentExistsCheck(studentId))
            {
                //Update Details
                var updatedStudent = await studentRepository.UpdateStudentAsync(
                    studentId,
                    mapper.Map<DataModels.Student>(request)
                    );

                

                if(updatedStudent!=null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }

            }

            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> DeleteOneStudent([FromRoute] Guid studentId)
        {
            if(await studentRepository.StudentExistsCheck(studentId))
            {
                var student = await studentRepository.DeleteStudentByIdAsync(studentId);

                return Ok(mapper.Map<Student>(student));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]

        public async Task<IActionResult> AddOneStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await studentRepository.AddStudentAsync(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetOneStudentAsync),
                new { studentId = student.Id},
                mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]

        public async Task<IActionResult> UploadProfileImageAsync([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
                ".jpg", ".jpeg", ".gif", ".png", ".heif"
            };

            if (profileImage != null && profileImage.Length> 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);

                if(validExtensions.Contains(extension))
                {
                    if (await studentRepository.StudentExistsCheck(studentId))
                    {
                        //upload-image
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                        var filePath = await imageRepository.UploadImageAsync(profileImage, fileName);

                        //update

                        if (await studentRepository.UpdateStudentProfileImage(studentId, filePath))
                        {
                            return Ok(filePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError);

                    }
                }

                return BadRequest("Not a valid image format.");
            }
            
            return NotFound();
        }
    }
}
