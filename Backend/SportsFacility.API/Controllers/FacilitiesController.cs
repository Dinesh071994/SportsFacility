using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.Domain.Models;
using SportsFacility.DTO;

namespace SportsFacility.API.Controllers
{
    // Controllers/FacilitiesController.cs
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public class FacilitiesController : ControllerBase
    {
        private readonly IBaseService<Facility> _facilityService;
        private readonly IMapper _mapper;

        public FacilitiesController(IBaseService<Facility> facilityService, IMapper mapper)
        {
            _facilityService = facilityService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacilities()
        {
            var facilities = await _facilityService.GetAllAsync();
            var facilityDtos = _mapper.Map<List<FacilityDto>>(facilities);

            return Ok(new { data = facilityDtos, count = facilityDtos.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacilityById(int id)
        {
            var facility = await _facilityService.GetByIdAsync(id);
            if (facility == null)
                return NotFound(new { error = "Facility not found" });

            var facilityDto = _mapper.Map<FacilityDto>(facility);
            return Ok(facilityDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var facility = _mapper.Map<Facility>(facilityDto);
            var result = await _facilityService.CreateAsync(facility);

            return CreatedAtAction(
                nameof(GetFacilityById),
                new { id = result.Id },
                result
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, [FromBody] FacilityDto facilityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var facility = _mapper.Map<Facility>(facilityDto);

            var result = await _facilityService.UpdateAsync(id, facility);
            if (result == null)
                return NotFound(new { error = "Facility not found" });

            return Ok(new { message = "Facility updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var result = await _facilityService.DeleteAsync(id);
            if (result == false)
                return NotFound(new { error = "Facility not found" });

            return Ok(new { message = "Facility deleted successfully" });
        }
    }
}
