using Application.LogicInterfaces;
using IHandInHomeworkLogic = Application.LogicInterfaces.IHandInHomeworkLogic;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HandInsController : ControllerBase
{
    private readonly IHandInHomeworkLogic _handInHomeworkLogic;
    private readonly IFeedbackLogic _feedbackLogic;

    public HandInsController(IHandInHomeworkLogic handInHomeworkLogic, IFeedbackLogic feedbackLogic)
    {
        _feedbackLogic = feedbackLogic;
        _handInHomeworkLogic = handInHomeworkLogic;
    }

    [HttpPost]
    [Authorize("MustBeStudent")]
    public async Task<ActionResult<HandInHomework>> HandInHomework([FromBody] HomeworkHandInDTO dto)
    {
        try
        {
            HandInHomework handedInHomework = await _handInHomeworkLogic.HandInHomework(dto);
            return Created($"/handIns/{handedInHomework.Id}, {handedInHomework.Answer}", handedInHomework);
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

    [HttpGet("{handInId}/feedback", Name = "GetFeedbackByHomeworkIdAndStudentUsernameAsync")]
    [Authorize("MustBeStudent")]
    public async Task<ActionResult<Feedback>> GetFeedbackByHandInIdAndStudentUsernameAsync([FromRoute] string handInId,
        [FromQuery] string username)
    {
        try
        {
            Console.WriteLine("Getting feedback webApi...");
            Feedback feedback = await _feedbackLogic.GetFeedbackByHandInIdAndStudentUsernameAsync(handInId, username);

            return Ok(feedback);
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