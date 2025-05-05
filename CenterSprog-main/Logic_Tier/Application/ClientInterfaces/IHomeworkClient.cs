using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.ClientInterfaces;

public interface IHomeworkClient
{
    Task<Homework> CreateAsync(HomeworkCreationDTO dto);
}