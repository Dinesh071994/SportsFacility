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
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffListDto>>> GetStaff()
        {
            var staff = await _staffService.GetStaffAsync();
            return Ok(_mapper.Map<IEnumerable<StaffListDto>>(staff));
        }

        [HttpPost]
        public async Task<ActionResult<StaffListDto>> CreateStaff(StaffListDto dto)
        {
            var staff = await _staffService.CreateStaffAsync(dto);
            if (staff == null) return BadRequest("Could not create staff member");

            return CreatedAtAction(nameof(GetStaff), new { id = staff.Id }, _mapper.Map<StaffListDto>(staff));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(Guid id, StaffListDto dto)
        {
            var result = await _staffService.UpdateStaffAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(Guid id)
        {
            var result = await _staffService.DeleteStaffAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
