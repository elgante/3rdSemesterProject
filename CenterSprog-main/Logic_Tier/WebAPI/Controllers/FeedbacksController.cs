using Application.LogicInterfaces;
using Domain.DTOs.FeedbackDTO;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbacksController : ControllerBase
{
    private readonly IFeedbackLogic _feedbackLogic;

    public FeedbacksController(IFeedbackLogic feedbackLogic)
    {
        _feedbackLogic = feedbackLogic;
    }

    [HttpPost]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Feedback>> AddFeedback([FromBody] AddFeedbackDTO addFeedbackDto)
    {
        try
        {
            Feedback createdFeedback = await _feedbackLogic.AddFeedbackAsync(addFeedbackDto);

            return Created($"/feedbacks/{createdFeedback.Id}", createdFeedback);
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