using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public ClassesController(IClassService classService, IMapper mapper)
        {
            _classService = classService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassScheduleDto>>> GetClasses()
        {
            var classes = await _classService.GetClassesAsync();
            return Ok(_mapper.Map<IEnumerable<ClassScheduleDto>>(classes));
        }

        [HttpPost]
        public async Task<ActionResult<ClassScheduleDto>> CreateClass(ClassScheduleDto dto)
        {
            var created = await _classService.CreateClassAsync(dto);
            if (created == null) return BadRequest("Could not create class");

            return CreatedAtAction(nameof(GetClasses), new { id = created.Id }, _mapper.Map<ClassScheduleDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(Guid id, ClassScheduleDto dto)
        {
            var result = await _classService.UpdateClassAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var result = await _classService.DeleteClassAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/attendance")]
        public async Task<IActionResult> MarkAttendance(Guid id)
        {
            var result = await _classService.MarkAttendanceAsync(id);
            if (!result) return BadRequest("Could not mark attendance");

            return Ok();
        }
    }
}
