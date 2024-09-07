using System.Collections.Generic;
using System.Linq;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Cliente> GetClientesAtivos()
        {
            return _context.Clientes.Where(c => c.BitAtivo).ToList();
        }

        public bool ClienteExiste(string cpf, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                return _context.Clientes.Any(c => c.CPF == cpf && c.ClienteID != clienteId && c.BitAtivo);
            }
            return _context.Clientes.Any(c => c.CPF == cpf && c.BitAtivo);
        }
    }
}
