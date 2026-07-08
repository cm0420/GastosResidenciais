using GastosResidenciais.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Data;

/// <summary>
/// Contexto do Entity Framework Core responsável pelo acesso ao banco PostgreSQL.
/// Centraliza o mapeamento das entidades (Fluent API) para manter as classes de Model limpas.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("pessoas");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(p => p.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(p => p.Idade)
                .HasColumnName("idade")
                .IsRequired();

            // Uma pessoa pode ter várias transações. Ao deletar a pessoa, suas transações são
            // removidas em cascata diretamente pelo banco de dados (regra de negócio do desafio).
            entity.HasMany(p => p.Transacoes)
                .WithOne(t => t.Pessoa)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.ToTable("transacoes");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(t => t.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(t => t.Valor)
                .HasColumnName("valor")
                .HasColumnType("numeric(14,2)")
                .IsRequired();

            entity.Property(t => t.Tipo)
                .HasColumnName("tipo")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(t => t.PessoaId)
                .HasColumnName("pessoa_id")
                .IsRequired();

            entity.HasIndex(t => t.PessoaId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
