using api.DataModels;
using api.DTOs;
using api.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository repo;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepo;

        public StudentsController(IStudentRepository repo,IMapper mapper,IImageRepository imageRepo)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.imageRepo = imageRepo;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await repo.GetStudentsAsync();
            
            return Ok(mapper.Map<List<StudentDTO>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudent")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid studentId)
        {
            var student = await repo.GetStudentAsync(studentId);
            if (student == null) return NotFound();
            return Ok(mapper.Map<StudentDTO>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, 
                                                            [FromBody] UpdateStudentDTO request)
        {
            if (await repo.Exists(studentId))
            {
                // Update Details
                var updatedStudent = await repo.UpdateStudent(studentId, 
                    mapper.Map<Student>(request));

                if (updatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await repo.Exists(studentId))
            {
                var student = await repo.DeleteStudent(studentId);
                return Ok(mapper.Map<StudentDTO>(student));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentDTO request)
        {
            var student = await repo.AddStudent(mapper.Map<Student>(request));
            return CreatedAtAction(nameof(GetStudent), new { studentId = student.Id },
                mapper.Map<StudentDTO>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await repo.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await imageRepo.Upload(profileImage, fileName);

                        if (await repo.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }
    }
}
