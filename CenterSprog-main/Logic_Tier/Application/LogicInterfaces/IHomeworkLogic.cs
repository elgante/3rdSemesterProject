using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.LogicInterfaces;

public interface IHomeworkLogic
{
    Task<Homework> CreateAsync(HomeworkCreationDTO dto);
}