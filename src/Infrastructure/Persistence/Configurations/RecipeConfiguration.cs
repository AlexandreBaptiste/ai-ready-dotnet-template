using Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>EF Core configuration for the <see cref="Recipe"/> aggregate.</summary>
internal sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Instructions)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(r => r.PrepTimeMinutes)
            .IsRequired();

        builder.Property(r => r.Difficulty)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(r => r.CategoryId)
            .IsRequired();

        // Ingredients are owned — they live in the same table as Recipe (table-splitting)
        // or their own table via OwnsMany.
        builder.OwnsMany(r => r.Ingredients, ingredientBuilder =>
        {
            ingredientBuilder.ToTable("Ingredients");

            ingredientBuilder.WithOwner().HasForeignKey("RecipeId");

            ingredientBuilder.HasKey(i => i.Id);

            ingredientBuilder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            ingredientBuilder.Property(i => i.Quantity)
                .IsRequired()
                .HasPrecision(10, 3);

            ingredientBuilder.Property(i => i.Unit)
                .IsRequired()
                .HasMaxLength(50);
        });
    }
}
