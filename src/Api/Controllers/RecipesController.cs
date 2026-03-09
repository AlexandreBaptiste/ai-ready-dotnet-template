using Application.Features.Recipes.Commands;
using Application.Features.Recipes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>CRUD endpoints for pastry recipes.</summary>
[ApiController]
[Route("api/recipes")]
[Produces("application/json")]
public sealed class RecipesController(ISender mediator) : ControllerBase
{
    /// <summary>Returns all recipes.</summary>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RecipeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<RecipeDto> recipes = await mediator.Send(new GetRecipesQuery(), cancellationToken);
        return Ok(recipes);
    }

    /// <summary>Returns a single recipe by identifier.</summary>
    /// <param name="id">The recipe identifier.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RecipeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        Application.Common.Models.Result<RecipeDto> result =
            await mediator.Send(new GetRecipeByIdQuery(id), cancellationToken);

        return result.Match<IActionResult>(Ok, _ => NotFound());
    }

    /// <summary>Creates a new recipe.</summary>
    /// <param name="command">Recipe creation payload.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateRecipeCommand command,
        CancellationToken cancellationToken)
    {
        Application.Common.Models.Result<Guid> result =
            await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            id => CreatedAtAction(nameof(GetById), new { id }, id),
            error => UnprocessableEntity(new { error }));
    }

    /// <summary>Updates an existing recipe.</summary>
    /// <param name="id">Route identifier — must match command Id.</param>
    /// <param name="command">Recipe update payload.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRecipeCommand command,
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

    /// <summary>Deletes a recipe.</summary>
    /// <param name="id">The recipe identifier.</param>
    /// <param name="cancellationToken">Propagated cancellation token.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        Application.Common.Models.Result result =
            await mediator.Send(new DeleteRecipeCommand(id), cancellationToken);

        return result.Match<IActionResult>(
            () => NoContent(),
            _ => NotFound());
    }
}
