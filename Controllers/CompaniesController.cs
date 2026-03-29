using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pp_back_codex.Contracts.Companies;
using pp_back_codex.Data;
using pp_back_codex.Models;

namespace pp_back_codex.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(ApplicationContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CompanyDto>>> GetAll(CancellationToken cancellationToken)
    {
        var companies = await dbContext.Companies
            .AsNoTracking()
            .Include(company => company.WorkGroup)
            .OrderBy(company => company.Id)
            .Select(company => new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Inn = company.Inn,
                Email = company.Email,
                Phone = company.Phone,
                WorkGroupId = company.WorkGroup != null ? company.WorkGroup.Id : null,
                WorkGroupName = company.WorkGroup != null ? company.WorkGroup.Name : null
            })
            .ToListAsync(cancellationToken);

        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .AsNoTracking()
            .Include(company => company.WorkGroup)
            .FirstOrDefaultAsync(company => company.Id == id, cancellationToken);

        if (company is null)
        {
            return NotFound();
        }

        return Ok(company.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> Create(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.Companies.AnyAsync(company => company.Inn == request.Inn, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.Inn), "Компания с таким ИНН уже существует.");
        }

        if (await dbContext.Companies.AnyAsync(company => company.Email == request.Email, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.Email), "Компания с таким email уже существует.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var company = new Company
        {
            Name = request.Name.Trim(),
            Inn = request.Inn.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            WorkGroup = new WorkGroup()
        };

        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = company.Id }, company.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CompanyDto>> Update(int id, UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .Include(entity => entity.WorkGroup)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (company is null)
        {
            return NotFound();
        }

        if (await dbContext.Companies.AnyAsync(entity => entity.Id != id && entity.Inn == request.Inn, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.Inn), "Компания с таким ИНН уже существует.");
        }

        if (await dbContext.Companies.AnyAsync(entity => entity.Id != id && entity.Email == request.Email, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.Email), "Компания с таким email уже существует.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        company.Name = request.Name.Trim();
        company.Inn = request.Inn.Trim();
        company.Email = request.Email.Trim();
        company.Phone = request.Phone.Trim();
        company.WorkGroup ??= new WorkGroup();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(company.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (company is null)
        {
            return NotFound();
        }

        dbContext.Companies.Remove(company);
        await dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}

internal static class CompanyMappings
{
    public static CompanyDto ToDto(this Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Inn = company.Inn,
            Email = company.Email,
            Phone = company.Phone,
            WorkGroupId = company.WorkGroup?.Id,
            WorkGroupName = company.WorkGroup?.Name ?? company.WorkGroup?.DisplayName
        };
    }
}
