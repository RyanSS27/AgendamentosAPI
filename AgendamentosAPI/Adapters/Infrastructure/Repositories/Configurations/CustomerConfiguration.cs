using AgendamentosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Ensina o EF Core a usar a variável privada para gravar e ler no banco
        builder.Property(c => c.PhoneNumbers)
            .HasField("_phoneNumbers");
    }
}