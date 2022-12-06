﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<Student>>(students));
        }


        [HttpGet]
        [Route("[controller]/{studentId:guid}")]
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
    }
}
