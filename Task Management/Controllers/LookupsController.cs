using Microsoft.AspNetCore.Mvc;
using Task_Management.DbContexts;

namespace Task_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly TaskManagementContext _context;

        public LookupsController(TaskManagementContext context)
        {
            _context = context;
        }

        [HttpGet("GetByMajorCode/{majorCode}")]
        public IActionResult GetByMajorCode(int majorCode)
        {
            try
            {
                var data = _context.Lookups
                    .Where(x => x.MajorCode == majorCode)
                    .OrderBy(x => x.MinorCode)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}