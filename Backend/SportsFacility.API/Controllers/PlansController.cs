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
    public class PlansController : BaseApiController
    {
        private readonly IPlanService _planService;
        private readonly IMapper _mapper;

        public PlansController(IPlanService planService, IMapper mapper)
        {
            _planService = planService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipPlanDto>>> GetPlans()
        {
            var plans = await _planService.GetPlansAsync();
            return Ok(_mapper.Map<IEnumerable<MembershipPlanDto>>(plans));
        }

        [HttpPost]
        public async Task<ActionResult<MembershipPlanDto>> CreatePlan(MembershipPlanDto dto)
        {
            var plan = _mapper.Map<SubscriptionPlan>(dto);
            var created = await _planService.CreatePlanAsync(plan);
            return CreatedAtAction(nameof(GetPlans), new { id = created.Id }, _mapper.Map<MembershipPlanDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(Guid id, MembershipPlanDto dto)
        {
            if (id.ToString() != dto.Id) return BadRequest();

            var plan = _mapper.Map<SubscriptionPlan>(dto);
            var result = await _planService.UpdatePlanAsync(id, plan);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            var result = await _planService.DeletePlanAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
