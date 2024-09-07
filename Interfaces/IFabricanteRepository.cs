using System.Collections.Generic;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public interface IFabricanteRepository : IRepository<Fabricante>
    {
        IEnumerable<Fabricante> GetFabricantesAtivos();
        bool FabricanteExiste(string nome, int? fabricanteId = null);
    }
}
