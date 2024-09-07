using System.Data.Entity;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("BancoDados")
        {
        }

        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Concessionaria> Concessionarias { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

    }
}
