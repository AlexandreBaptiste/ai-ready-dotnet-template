using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>CRUD endpoints for recipe categories.</summary>
[ApiController]
[Route("api/categories")]
[Produces("application/json")]
public sealed class CategoriesController(ISender mediator) : ControllerBase
{
    /// <summary>Returns all categories.</summary>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<CategoryDto> categories =
            await mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    /// <summary>Creates a new category.</summary>
    /// <param name="command">Category creation payload.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        Application.Common.Models.Result<Guid> result =
            await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            id => CreatedAtAction(nameof(GetAll), new { }, new { id }),
            error => UnprocessableEntity(new { error }));
    }

    /// <summary>Updates an existing category.</summary>
    /// <param name="id">Route identifier — must match command Id.</param>
    /// <param name="command">Category update payload.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(new { error = "Route id and command Id must match." });

        Application.Common.Models.Result result =
            await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            () => NoContent(),
            _ => NotFound());
    }

    /// <summary>Deletes a category.</summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        Application.Common.Models.Result result =
            await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

        return result.Match<IActionResult>(
            () => NoContent(),
            _ => NotFound());
    }
}
