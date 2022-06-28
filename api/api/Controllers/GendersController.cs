using api.DTOs;
using api.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [ApiController]
    public class GendersController : Controller
    {
        private readonly IStudentRepository repo;
        private readonly IMapper mapper;

        public GendersController(IStudentRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGenders()
        {
            var genderList = await repo.GetGendersAsync();

            if (genderList == null || !genderList.Any())
            {
                return NotFound();
            }

            return Ok(mapper.Map<List<GenderDTO>>(genderList));
        }
    }
}
