using System.ComponentModel.DataAnnotations;

namespace TSUMProject.DTO;

public abstract record BaseResultDto(
    bool Success,
    string Message);

public record SuccessDto(
    bool Success,
    string Message,
    FileProjectDto FileProject
    ) : BaseResultDto(Success, Message);

public record ErrorDto(
    bool Success,
    string Message) : BaseResultDto(Success, Message);

public record RequestDto(
    [Required, Url] string UrlFile);

public record FileProjectDto(
    int Id,
    string Name,
    int ProjectId,
    string UrlFile);
