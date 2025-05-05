using Application.LogicInterfaces;
using Domain.DTOs.UserDTO;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public UsersController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<User>> RegisterUser(UserCreationDTO dto)
    {
        try
        {
            
            User createdUser = await _userLogic.CreateUserAsync(dto);
            return Created($"users/{createdUser.Username}",createdUser);
        }
        catch (RpcException e)
        {
            return NotFound(e.Status.Detail);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{username}")]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<User>> GetByUsernameAsync([FromRoute] string username)
    {   
        try
        {
            User existingUser = await _userLogic.GetUserByUsernameAsync(username);
            return Ok(existingUser);
        }
        catch (RpcException e)
        {
            return NotFound(e.Status.Detail);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
    {
        try
        {
            IEnumerable<User> users = await _userLogic.GetAllAsync();
            return Ok(users);
        }
        catch (RpcException e)
        {
            return NotFound(e.Status.Detail);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}