
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;

namespace backend.Services;

public class UserService : IUserService
{
    // public async Task<User> Create(CreateUserDto dto)
    // {
        
    private readonly AppDbContext _context;

    private readonly PasswordHasher<User> _passwordHasher;

    private readonly IJwtService _jwtService;

    public UserService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = new PasswordHasher<User>();
    }

    //Register
    public async Task<bool> Register(RegisterDto request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if(existingUser != null)
        {
            return false;
        } 

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
        };

        user.Password = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    
    }


    //Login
    public async Task<LoginResponseDto> ValidateLogin(LoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == request.Email 
        );

        if(user == null)
           return null!; // Learn about it
        
        var result = 
            _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                request.Password
            );
        
        if(result == PasswordVerificationResult.Failed)
        {
            return null!; // Learn about it
        }

        return new LoginResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,

            // Token = "fake-jwt-token", 
            Token = _jwtService.GenerateToken(user, request.RememberMe), 
        };
    }


    //get Users
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            }).ToListAsync();
    }


    //delete User
    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if(user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }


    // update User
    public async Task<User?> UpdateUser(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if(user == null)
            return null;
        
        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = dto.Role;

        await _context.SaveChangesAsync();
        return user;
    }

}
