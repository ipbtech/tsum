using Microsoft.AspNetCore.Mvc;
using TSUMProject.DAL;
using TSUMProject.DTO;

namespace TSUMProject.Controllers;

[Route("api/file-project")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class FileProjectApiController(
    ApplicationDbContext dbContext,
    ILogger<FileProjectApiController> logger) : ControllerBase
{
    
    [HttpPatch("/{id:int}/file-url")]
    public async Task<ActionResult<BaseResultDto>> UpdateFileUrl([FromRoute] int id, [FromBody] RequestDto requestDto)
    {
        try
        {
            var fileProj = await dbContext.FileProject.FindAsync(id);
            if (fileProj is null)
            {
                return NotFound(new ErrorDto(false, "Файл с таким ID не найден"));
            }

            fileProj.UrlFile = requestDto.UrlFile;
            dbContext.FileProject.Update(fileProj);
            await dbContext.SaveChangesAsync();

            return Ok(new SuccessDto(
                true,
                "URL файла обновлён",
                new FileProjectDto(fileProj.Id, fileProj.Name, fileProj.ProjectId, fileProj.UrlFile)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            return StatusCode(500, new ErrorDto(false, "Something went wrong"));
        }
    }
}