using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Veiculo> GetVeiculosAtivos()
        {
            return _context.Veiculos.Include(v => v.Fabricante).Where(v => v.BitAtivo).ToList();
        }
    }
}
