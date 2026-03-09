using Application.Common.Models;
using FluentValidation;

namespace Application.Features.Samples.Commands;

// ── Command ───────────────────────────────────────────────────────────────────

/// <summary>
/// Example command: creates a new sample item.
///
/// Pattern:
///   - Commands return Result&lt;T&gt; so callers handle failures explicitly.
///   - The validator runs automatically via ValidationBehaviour in the MediatR pipeline.
///   - The handler keeps domain logic in the domain layer — it only orchestrates.
/// </summary>
public sealed record CreateSampleCommand(string Name) : IRequest<Result<Guid>>;

// ── Validator ─────────────────────────────────────────────────────────────────

public sealed class CreateSampleCommandValidator : AbstractValidator<CreateSampleCommand>
{
    public CreateSampleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}

// ── Handler ───────────────────────────────────────────────────────────────────

public sealed class CreateSampleCommandHandler
    : IRequestHandler<CreateSampleCommand, Result<Guid>>
{
    // Inject your repositories and IUnitOfWork here via constructor.
    // Example:
    //   private readonly IRepository<Sample> _repository;
    //   private readonly IUnitOfWork _unitOfWork;
    //
    //   public CreateSampleCommandHandler(
    //       IRepository<Sample> repository,
    //       IUnitOfWork unitOfWork)
    //   {
    //       _repository = repository;
    //       _unitOfWork = unitOfWork;
    //   }

    public async Task<Result<Guid>> Handle(
        CreateSampleCommand request,
        CancellationToken cancellationToken)
    {
        // TODO: replace with your domain entity creation logic
        // var sample = Sample.Create(request.Name);
        // await _repository.AddAsync(sample, cancellationToken);
        // await _unitOfWork.SaveChangesAsync(cancellationToken);
        // return Result.Success(sample.Id);

        await Task.CompletedTask; // remove when implementing
        return Result.Success(Guid.NewGuid());
    }
}
