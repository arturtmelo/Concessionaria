using System.Collections.Generic;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        IEnumerable<Cliente> GetClientesAtivos();
        bool ClienteExiste(string cpf, int? clienteId = null);
    }
}
