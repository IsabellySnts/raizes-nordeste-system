using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Infrastructure.Security;

namespace RaizesDoNordeste.Infrastructure;

public class RaizesDoNordesteDbContext : DbContext
{
    public RaizesDoNordesteDbContext(DbContextOptions<RaizesDoNordesteDbContext> options) : base(options) {}
    public DbSet<Usuario> Usuarios{ get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Funcionario> Funcionario { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Cardapio> Cardapios { get; set; }
    public DbSet<Unidade> Unidades { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<MovimentacaoPontos> MovimentacoesPontos { get; set; }
    public DbSet<Estoque> Estoque { get; set; }
    public DbSet<Fidelidade> Fidelidades { get; set; }
    public DbSet<Campanha> Campanhas { get; set; }
    public DbSet<Auditoria> Auditorias { get; set; }
    public DbSet<ConsentimentoLGPD> Consentimentos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Usuario>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(o => o.Id).UseIdentityColumn();
            e.Property(x => x.Email).IsRequired().HasMaxLength(100);
            e.Property(x => x.Senha).IsRequired().HasMaxLength(100);
        });

        builder.Entity<Cliente>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(o => o.Id).UseIdentityColumn();
            e.Property(x => x.NomeCompleto).IsRequired().HasMaxLength(100);
            e.Property(x => x.Cpf)
                .IsRequired()
                .HasMaxLength(200)
                .HasConversion(
                    cpf => CriptografiaService.Criptografar(cpf), 
                    cpf => CriptografiaService.Descriptografar(cpf) 
                );
            e.Property(x => x.Email).IsRequired().HasMaxLength(100);
            e.Property(x => x.Telefone).HasMaxLength(20);
            e.Property(x => x.DataNascimento).IsRequired();
            e.Property(x => x.DataCadastro).IsRequired();

            e.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<Cliente>(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Fidelidade)
                .WithOne(x => x.Cliente)
                .HasForeignKey<Fidelidade>(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Consentimentos)
               .WithOne(x => x.Cliente)
               .HasForeignKey(x => x.IdCliente)
               .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Pedidos)
                .WithOne(x => x.Cliente)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Funcionario>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            e.Property(x => x.Cpf)
                .IsRequired()
                .HasMaxLength(200)
                .HasConversion(
                    cpf => CriptografiaService.Criptografar(cpf),
                    cpf => CriptografiaService.Descriptografar(cpf)
                );
            e.Property(x => x.Email).IsRequired().HasMaxLength(200);
            e.Property(x => x.Telefone).HasMaxLength(20);
            e.Property(x => x.Cargo).IsRequired().HasConversion<string>().HasMaxLength(30);
            e.Property(x => x.Ativo).IsRequired().HasDefaultValue(true);

            e.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<Funcionario>(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Unidade)
                .WithMany(x => x.Funcionarios)
                .HasForeignKey(x => x.IdUnidade)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Unidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            e.Property(x => x.Cidade).IsRequired().HasMaxLength(100);
            e.Property(x => x.Estado).IsRequired().HasMaxLength(50);
            e.Property(x => x.Pais).IsRequired().HasMaxLength(50);
            e.Property(x => x.Logradouro).IsRequired().HasMaxLength(300);
            e.Property(x => x.Complemento).HasMaxLength(200);
            e.Property(x => x.DiasFuncionamento).IsRequired().HasMaxLength(100);
            e.Property(x => x.HorarioFuncionamento).IsRequired().HasMaxLength(50);
            e.Property(x => x.TipoCozinha).IsRequired().HasConversion<string>().HasMaxLength(20);
        });

        builder.Entity<Categoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Nome).IsRequired().HasMaxLength(100);
            e.Property(x => x.Descricao).HasMaxLength(500);

            e.HasMany(x => x.Produtos)
                .WithOne(x => x.Categoria)
                .HasForeignKey(x => x.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Produto>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            e.Property(x => x.Descricao).HasMaxLength(1000);
            e.Property(x => x.Preco).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(x => x.FlagSazonal).IsRequired().HasDefaultValue(false);
            e.Property(x => x.DataInicioDisponibilidade);
            e.Property(x => x.DataFimDisponibilidade);
            e.Property(x => x.Foto).HasMaxLength(500);
        });

        builder.Entity<Cardapio>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Disponivel).IsRequired().HasDefaultValue(true);
            e.Property(x => x.PrecoLocal).HasColumnType("decimal(10,2)");
            e.Property(x => x.VariacaoRegional).HasMaxLength(500);

            e.HasOne(x => x.Produto)
                .WithMany(x => x.Cardapios)
                .HasForeignKey(x => x.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Unidade)
                .WithMany(x => x.Cardapios)
                .HasForeignKey(x => x.IdUnidade)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.IdProduto, x.IdUnidade }).IsUnique();
        });

        builder.Entity<Estoque>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Quantidade).IsRequired();
            e.Property(x => x.QuantidadeMinima).IsRequired().HasDefaultValue(0);
            e.Property(x => x.DataInsercao).IsRequired();
            e.Property(x => x.DataAtualizacao).IsRequired();

            e.HasOne(x => x.Produto)
                .WithMany(x => x.Estoques)
                .HasForeignKey(x => x.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Unidade)
                .WithMany(x => x.Estoques)
                .HasForeignKey(x => x.IdUnidade)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.IdProduto, x.IdUnidade }).IsUnique();
        });

        builder.Entity<Pedido>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.CanalOrigem).IsRequired().HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
            e.Property(x => x.ValorTotal).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(x => x.DataCriacao).IsRequired();
            e.Property(x => x.DataAtualizacao).IsRequired();

            e.HasOne(x => x.Unidade)
                .WithMany(x => x.Pedidos)
                .HasForeignKey(x => x.IdUnidade)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Funcionario)
                .WithMany()
                .HasForeignKey(x => x.IdFuncionario)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Pagamento)
                .WithOne(x => x.Pedido)
                .HasForeignKey<Pagamento>(x => x.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Itens)
                .WithOne(x => x.Pedido)
                .HasForeignKey(x => x.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ItemPedido>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Quantidade).IsRequired();
            e.Property(x => x.PrecoUnitario).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(x => x.Observacao).HasMaxLength(500);

            e.HasOne(x => x.Produto)
                .WithMany()
                .HasForeignKey(x => x.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Pagamento>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Valor).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(x => x.TipoPagamento).IsRequired().HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.DataSolicitacao).IsRequired();
            e.Property(x => x.DataEfetivacao);
            e.Property(x => x.CodigoTransacao).HasMaxLength(200);
            e.Property(x => x.Tentativas).IsRequired().HasDefaultValue(0);

            e.HasIndex(x => x.IdPedido).IsUnique();
        });

        builder.Entity<Fidelidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.SaldoPontos).IsRequired().HasDefaultValue(0);
            e.Property(x => x.Nivel).IsRequired().HasConversion<string>().HasMaxLength(20);

            e.HasIndex(x => x.IdCliente).IsUnique();

            e.HasMany(x => x.Movimentacoes)
                .WithOne(x => x.Fidelidade)
                .HasForeignKey(x => x.IdFidelidade)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<MovimentacaoPontos>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Tipo).IsRequired().HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Pontos).IsRequired();
            e.Property(x => x.Data).IsRequired();

            e.HasOne(x => x.Pedido)
                .WithMany()
                .HasForeignKey(x => x.IdPedido)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<ConsentimentoLGPD>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Permissao).IsRequired().HasConversion<string>().HasMaxLength(30);
            e.Property(x => x.Aceite).IsRequired();
            e.Property(x => x.Data).IsRequired();
        });

        builder.Entity<Auditoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Acao).IsRequired().HasConversion<string>().HasMaxLength(50);
            e.Property(x => x.TipoEntidadeAfetada).IsRequired().HasMaxLength(50);
            e.Property(x => x.IdEntidadeAfetada).IsRequired();
            e.Property(x => x.Detalhes).HasMaxLength(2000);
            e.Property(x => x.DataHora).IsRequired();

            e.HasOne(x => x.Funcionario)
                .WithMany()
                .HasForeignKey(x => x.IdFuncionario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Campanha>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            e.Property(x => x.Descricao).HasMaxLength(1000);
            e.Property(x => x.Criterios).HasMaxLength(2000); 
            e.Property(x => x.DataInicio).IsRequired();
            e.Property(x => x.DataFim).IsRequired();
            e.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Beneficio).HasMaxLength(200);
        });


        base.OnModelCreating(builder);
    }
}
