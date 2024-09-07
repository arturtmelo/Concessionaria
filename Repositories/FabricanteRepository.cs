using System.Collections.Generic;
using System.Linq;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public class FabricanteRepository : Repository<Fabricante>, IFabricanteRepository
    {
        public FabricanteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Fabricante> GetFabricantesAtivos()
        {
            return _context.Fabricantes.Where(f => !f.BitAtivo).ToList();
        }

        public bool FabricanteExiste(string nome, int? fabricanteId = null)
        {
            if (fabricanteId.HasValue)
            {
                return _context.Fabricantes.Any(f => f.Nome == nome && f.FabricanteID != fabricanteId && !f.BitAtivo);
            }
            return _context.Fabricantes.Any(f => f.Nome == nome && !f.BitAtivo);
        }
    }
}
