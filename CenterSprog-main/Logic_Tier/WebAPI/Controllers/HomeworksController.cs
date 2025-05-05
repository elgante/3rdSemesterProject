using Application.LogicInterfaces;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeworksController : ControllerBase
{
    private readonly IHomeworkLogic _homeworkLogic;
    private readonly IHandInHomeworkLogic _handInHomeworkLogic;

    public HomeworksController(IHomeworkLogic homeworkLogic, IHandInHomeworkLogic handInHomeworkLogic)
    {
        _homeworkLogic = homeworkLogic;
        _handInHomeworkLogic = handInHomeworkLogic;
    }

    [HttpPost]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Homework>> CreateHomework([FromBody] HomeworkCreationDTO dto)
    {
        try
        {
            Homework createdHomework = await _homeworkLogic.CreateAsync(dto);
            return Created($"/homeworks/{createdHomework.Id}", createdHomework);
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

    [HttpGet("{homeworkId}/handIns", Name = "GetHandInsByHomeworkIdAsync")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<IEnumerable<HandInHomework>>> GetHandInsByHomeworkIdAsync(
        [FromRoute] string homeworkId)
    {
        try
        {
            IEnumerable<HandInHomework> handIns = await _handInHomeworkLogic.GetHandInsByHomeworkIdAsync(homeworkId);
            return new OkObjectResult(handIns);
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

    [HttpGet("{homeworkId}/handIn", Name = "GetHandInByHomeworkIdAndStudentUsernameAsync")]
    [Authorize("MustBeUser")]
    public async Task<ActionResult<HandInHomework>> GetHandInByHomeworkIdAndStudentUsernameAsync(
        [FromRoute] string homeworkId,
        [FromQuery] string username)
    {
        try
        {
            HandInHomework handIn =
                await _handInHomeworkLogic.GetHandInByHomeworkIdAndStudentUsernameAsync(homeworkId, username);

            return Ok(handIn);
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