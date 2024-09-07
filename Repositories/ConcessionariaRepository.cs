using System.Collections.Generic;
using System.Linq;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public class ConcessionariaRepository : Repository<Concessionaria>, IConcessionariaRepository
    {
        public ConcessionariaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Concessionaria> GetConcessionariasAtivas()
        {
            return _context.Concessionarias.Where(c => c.BitAtivo).ToList();
        }

        public bool ConcessionariaExiste(string nome, int? concessionariaId = null)
        {
            if (concessionariaId.HasValue)
            {
                return _context.Concessionarias.Any(c => c.Nome == nome && c.ConcessionariaID != concessionariaId && c.BitAtivo);
            }
            return _context.Concessionarias.Any(c => c.Nome == nome && c.BitAtivo);
        }
    }
}
