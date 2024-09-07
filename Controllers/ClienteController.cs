using System;
using System.Linq;
using System.Web.Mvc;
using ConcessionariaMVC.Models;
using ConcessionariaMVC.Repositories;

namespace ConcessionariaMVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        // Injeção de dependência via construtor
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // GET: Cliente
        public ActionResult Index()
        {
            try
            {
                var clientes = _clienteRepository.GetClientesAtivos().ToList();
                return View(clientes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar clientes: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (_clienteRepository.ClienteExiste(cliente.CPF))
                {
                    return Json(new { success = false, errorMessage = "CPF já cadastrado. Por favor, utilize outro." });
                }

                try
                {
                    _clienteRepository.Add(cliente);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao criar cliente: " + ex.Message });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorMessage = string.Join(" ", errors);
                return Json(new { success = false, errorMessage });
            }
        }

        // GET: Cliente/GetCliente/5
        [HttpGet]
        public ActionResult GetCliente(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null || !cliente.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    cliente.ClienteID,
                    cliente.Nome,
                    cliente.CPF,
                    cliente.Telefone
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar cliente: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (_clienteRepository.ClienteExiste(cliente.CPF, cliente.ClienteID))
                {
                    return Json(new { success = false, errorMessage = "CPF já cadastrado para outro cliente. Por favor, utilize outro." });
                }

                try
                {
                    _clienteRepository.Update(cliente);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao editar cliente: " + ex.Message });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorMessage = string.Join(" ", errors);
                return Json(new { success = false, errorMessage });
            }
        }

        // GET: Cliente/GetClienteDetails/5
        public ActionResult GetClienteDetails(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null || !cliente.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    cliente.ClienteID,
                    cliente.Nome,
                    cliente.CPF,
                    cliente.Telefone
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar detalhes do cliente: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente != null && cliente.BitAtivo)
                {
                    try
                    {
                        cliente.BitAtivo = false;
                        _clienteRepository.Update(cliente);
                        TempData["SuccessMessage"] = "Cliente deletado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Erro ao deletar cliente: " + ex.Message;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Cliente não encontrado.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao processar a exclusão: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
