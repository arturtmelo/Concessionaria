using System.Collections.Generic;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public interface IConcessionariaRepository : IRepository<Concessionaria>
    {
        IEnumerable<Concessionaria> GetConcessionariasAtivas();
        bool ConcessionariaExiste(string nome, int? concessionariaId = null);
    }
}
