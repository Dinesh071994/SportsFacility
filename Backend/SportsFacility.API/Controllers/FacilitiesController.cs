using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using SportsFacility.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        private readonly IMapper _mapper;

        public FacilitiesController(IFacilityService facilityService, IMapper mapper)
        {
            _facilityService = facilityService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetFacilities()
        {
            var facilities = await _facilityService.GetFacilitiesAsync();
            return Ok(_mapper.Map<IEnumerable<ActivityDto>>(facilities));
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateFacility(ActivityDto dto)
        {
            var facility = _mapper.Map<Facility>(dto);
            var created = await _facilityService.CreateFacilityAsync(facility, dto.NumberOfCourts);
            return CreatedAtAction(nameof(GetFacilities), new { id = created.Id }, _mapper.Map<ActivityDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(Guid id, ActivityDto dto)
        {
            if (id.ToString() != dto.Id) return BadRequest();

            var facility = _mapper.Map<Facility>(dto);
            var result = await _facilityService.UpdateFacilityAsync(id, facility);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            var result = await _facilityService.DeleteFacilityAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
