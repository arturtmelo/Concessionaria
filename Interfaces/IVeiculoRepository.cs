using System.Collections.Generic;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        IEnumerable<Veiculo> GetVeiculosAtivos();
    }
}
