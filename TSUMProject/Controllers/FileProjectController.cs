using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSUMProject.DAL;
using TSUMProject.DAL.Entities;
using TSUMProject.ViewModels;

namespace TSUMProject.Controllers;

public class FileProjectController(
    ApplicationDbContext dbContext,
    ILogger<FileProjectController> logger) : Controller
{

    // GET: FileProject
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var fileProjs = await dbContext.FileProject.AsNoTracking()
                .Include(f => f.Project)
                .ToListAsync();

            var viewModels = fileProjs
                .Select(f => new FileProjectViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    UrlFile = f.UrlFile,
                    ProjectId = f.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Id = f.Project.Id,
                        Name = f.Project.Name
                    }
                }).ToList();

            return View(viewModels);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw;  // pass to exception handler middleware
        }
    }

    // GET: FileProject/Details/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            if (!await IsExistFileProject(id))
            {
                return NotFound();
            }
            
            var fileProj = await dbContext.FileProject.AsNoTracking()
                .Include(f => f.Project)
                .FirstOrDefaultAsync(f => f.Id == id);

            var viewModel = new FileProjectViewModel
            {
                Id = fileProj.Id,
                Name = fileProj.Name,
                UrlFile = fileProj.UrlFile,
                ProjectId = fileProj.ProjectId,
                Project = new ProjectViewModel
                {
                    Id = fileProj.Project?.Id ?? 0,
                    Name = fileProj.Project?.Name
                }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw;  // pass to exception handler middleware
        }
    }

    // GET: FileProject/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            ViewData["ProjectId"] = await GetProjectsSelectList();
            return View(new FileProjectViewModel());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    // POST: FileProject/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,ProjectId,UrlFile")] FileProjectViewModel fileProjectViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewData["ProjectId"] = await GetProjectsSelectList();
                return View(fileProjectViewModel);
            }

            var entity = new FileProject
            {
                Name = fileProjectViewModel.Name,
                ProjectId = fileProjectViewModel.ProjectId,
                UrlFile = fileProjectViewModel.UrlFile,
            };

            await dbContext.FileProject.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            TempData["RedirectResult"] = "FileProject was successfully created"; // can use through redirect into another action
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    // GET: FileProject/Edit/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            if (!await IsExistFileProject(id))
            {
                return NotFound();
            }

            var fileProject = await dbContext.FileProject.FindAsync(id);
            var viewModel = new FileProjectViewModel
            {
                Id = fileProject.Id,
                Name = fileProject.Name,
                ProjectId = fileProject.ProjectId,
                UrlFile = fileProject.UrlFile
            };

            ViewData["ProjectId"] = GetProjectsSelectList(viewModel.ProjectId);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    // POST: FileProject/Edit/5
    [HttpPost("{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, 
        [Bind("Name,ProjectId,UrlFile")] FileProjectViewModel fileProjectViewModel)
    {
        try
        {
            if (!await IsExistFileProject(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProjectId"] = await GetProjectsSelectList(fileProjectViewModel.ProjectId);
                return View(fileProjectViewModel);
            }

            var entity = new FileProjectViewModel()
            {
                Id = id,
                Name = fileProjectViewModel.Name,
                ProjectId = fileProjectViewModel.ProjectId,
                UrlFile = fileProjectViewModel.UrlFile
            };

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            TempData["RedirectResult"] = "FileProject was successfully updated"; // can use through redirect into another action
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    // GET: FileProject/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!await IsExistFileProject(id))
            {
                return NotFound();
            }

            var fileProj = await dbContext.FileProject.AsNoTracking()
                .Include(f => f.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            var viewModel = new FileProjectViewModel
            {
                Id = fileProj.Id,
                Name = fileProj.Name,
                UrlFile = fileProj.UrlFile,
                ProjectId = fileProj.ProjectId,
                Project = new ProjectViewModel
                {
                    Id = fileProj.Project.Id,
                    Name = fileProj.Project.Name,
                }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    // POST: FileProject/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            if (!await IsExistFileProject(id))
            {
                return NotFound();
            }

            var fileProj = await dbContext.FileProject.FindAsync(id);
            dbContext.FileProject.Remove(fileProj);
            await dbContext.SaveChangesAsync();
            TempData["RedirectResult"] = "FileProject was successfully deleted"; // can use through redirect into another action
            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong");
            throw; // pass to exception handler middleware
        }
    }

    private Task<bool> IsExistFileProject(int id) =>
        dbContext.FileProject.AnyAsync(e => e.Id == id);

    private async Task<SelectList> GetProjectsSelectList(object? selectedValue = null)
    {
        var projIds = await dbContext.Project.AsNoTracking()
            .ToListAsync();

        if (selectedValue is not null)
        {
            return new SelectList(
                projIds.Select(x => new ProjectViewModel { Id = x.Id, Name = x.Name }),
                "Id",
                "Name",
                selectedValue);
        }

        return new SelectList(
            projIds.Select(x => new ProjectViewModel { Id = x.Id, Name = x.Name }),
            "Id",
            "Name");
    }
}
