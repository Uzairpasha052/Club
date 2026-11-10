using Microsoft.AspNetCore.Mvc;
using backend.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

// [Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;        
    }

    //Register
    [HttpPost("register")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var created = await _userService.Register(request);

        if (!created)
        {
            return BadRequest(new
            {
                message = "User already exist"
            });
        }

        return Ok(new
        {
            success = true,
            message = "User Registered."
        });
    }

    //Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var user = await _userService.ValidateLogin(request);

        if (user == null)
        {
            return Unauthorized(new 
            {
                message = "Invalid Email or Password"
            });
        }

        return Ok(user);
    }

    //get Users 
    [Authorize]
    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }

    //delete User
    [Authorize]
    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);

        if(!result)
            return NotFound();
        
        return NoContent();
    }

    //Update User
    [Authorize]
    [HttpPut("updateUser/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userService.UpdateUser(id, dto);

        if(user == null)
            return NotFound();

        return Ok(user);
    }

}