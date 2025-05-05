using Application.LogicInterfaces;
using Domain.DTOs.LessonDTO;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonLogic _lessonLogic;

    public LessonsController(ILessonLogic lessonLogic)
    {
        _lessonLogic = lessonLogic;
    }

    [HttpGet("{lessonId}", Name = "GetLessonByIdAsync")]
    [Authorize]
    public async Task<ActionResult<Lesson>> GetByIdAsync([FromRoute] string lessonId)
    {
        try
        {
            Lesson lesson = await _lessonLogic.GetByIdAsync(lessonId);
            return Ok(lesson);
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

    [HttpPost("{lessonId}/attendance", Name = "MarkAttendanceAsync")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<int>> MarkAttendanceAsync([FromRoute] string lessonId,
        [FromBody] List<String> studentUsernames)
    {
        try
        {
            var markAttendanceDto = new MarkAttendanceDTO { LessonId = lessonId, StudentUsernames = studentUsernames };
            int amountOfParticipants = await _lessonLogic.MarkAttendanceAsync(markAttendanceDto);
            return Ok(amountOfParticipants.ToString());
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

    [HttpGet("{lessonId}/attendance", Name = "GetAttendanceAsync")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<IEnumerable<User>>> GetAttendanceAsync([FromRoute] string lessonId)
    {
        try
        {
            IEnumerable<User> attendees = await _lessonLogic.GetAttendanceAsync(lessonId);

            return Ok(attendees);
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

    [HttpPost]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Lesson>> CreateAsync([FromBody] LessonCreationDTO lessonCreationDto)
    {
        try
        {
            Lesson createdLesson = await _lessonLogic.CreateAsync(lessonCreationDto);

            return Created($"lessons/{createdLesson.Id}", createdLesson);
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

    [HttpDelete("{lessonId}")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Boolean>> DeleteAsync([FromRoute] string lessonId)
    {
        try
        {
            var deleted = await _lessonLogic.DeleteAsync(lessonId);

            return Ok(deleted);
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

    [HttpPut]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Boolean>> UpdateLessonAsync([FromBody] LessonUpdateDTO lessonUpdateDto)
    {
        try
        {
            var updated = await _lessonLogic.UpdateLessonAsync(lessonUpdateDto);
            return Ok(updated);
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