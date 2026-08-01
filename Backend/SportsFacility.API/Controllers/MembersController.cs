using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly IMembershipService _membershipService;
        private readonly IMapper _mapper;

        public MembersController(IMembershipService membershipService, IMapper mapper)
        {
            _membershipService = membershipService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetMembers([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var (items, totalCount) = await _membershipService.GetPagedMembershipsAsync(pageNumber.Value, pageSize.Value);
                var mappedItems = _mapper.Map<IEnumerable<MemberListDto>>(items);
                return Ok(new PaginatedResultDto<MemberListDto>
                {
                    Items = mappedItems,
                    TotalCount = totalCount
                });
            }

            var memberships = await _membershipService.GetMembershipsAsync();
            return Ok(_mapper.Map<IEnumerable<MemberListDto>>(memberships));
        }

        [HttpPost]
        public async Task<ActionResult<MemberListDto>> CreateMember(MemberCreateDto dto)
        {
            var membership = await _membershipService.CreateMembershipAsync(dto);
            if (membership == null)
            {
                return NotFound("Plan not found");
            }

            return CreatedAtAction(nameof(GetMembers), new { id = membership.Id }, _mapper.Map<MemberListDto>(membership));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(Guid id, MemberCreateDto dto)
        {
            var result = await _membershipService.UpdateMembershipAsync(id, dto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _membershipService.DeleteMembershipAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
