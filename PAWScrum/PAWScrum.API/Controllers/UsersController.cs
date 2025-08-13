using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/UserAPI/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT api/UserAPI
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null || user.UserId == 0)
                return BadRequest("Invalid user data");

            var updated = await _userService.UpdateAsync(user);
            if (!updated)
                return NotFound("User not found");

            return Ok("User updated successfully");
        }

        // DELETE api/UserAPI/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound("User not found");

            return Ok("User deleted successfully");
        }
    }
}
