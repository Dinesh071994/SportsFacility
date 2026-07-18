using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.Domain.Models;
using SportsFacility.DTO;

namespace SportsFacility.API.Controllers
{


    // Controllers/MembersController.cs
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    public class MembersController : ControllerBase
    {
        private readonly IBaseService<Member> _memberService;
        private readonly IMapper _mapper;

        public MembersController(IBaseService<Member> memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers(
            [FromQuery] string name = null,
            [FromQuery] string email = null,
            [FromQuery] string membershipType = null)
        {
            var members = await _memberService.GetAllAsync();
            var memberDtos = _mapper.Map<List<MemberDto>>(members);

            return Ok(new { data = memberDtos, count = memberDtos.Count });
        }

        //[HttpPost]
        //public async Task<IActionResult> RegisterMember([FromBody] RegisterMemberDto memberDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var member = _mapper.Map<Member>(memberDto);
        //    var result = await _memberService.RegisterAsync(member);

        //    return CreatedAtAction(
        //        nameof(GetMemberById),
        //        new { id = result.Id },
        //        result
        //    );
        //}
    }
}
