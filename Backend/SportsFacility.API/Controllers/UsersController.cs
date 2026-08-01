using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using SportsFacility.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public class ResetPasswordDto
        {
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffListDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(_mapper.Map<IEnumerable<StaffListDto>>(users));
        }

        [HttpPost]
        public async Task<ActionResult<StaffListDto>> CreateUser(StaffListDto dto)
        {
            var user = await _userService.CreateUserAsync(dto);
            if (user == null) return BadRequest("Could not create user");

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, _mapper.Map<StaffListDto>(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, StaffListDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(id, dto.NewPassword);
            if (!result) return NotFound();

            return Ok(new { Message = "Password reset successfully" });
        }

        public class ProfilePictureDto
        {
            public string Email { get; set; } = string.Empty;
            public string PicturePath { get; set; } = string.Empty;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<StaffListDto>> GetUserProfile([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound("User not found");

            return Ok(_mapper.Map<StaffListDto>(user));
        }

        [HttpPost("profile/picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] ProfilePictureDto dto)
        {
            var result = await _userService.UpdateProfilePictureAsync(dto.Email, dto.PicturePath);
            if (!result) return NotFound("User not found");

            return Ok(new { Message = "Profile picture updated successfully" });
        }
    }
}
