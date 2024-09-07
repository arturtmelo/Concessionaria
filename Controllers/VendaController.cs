using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Controllers
{
    public class VendaController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // Injeta BD via construtor
        public VendaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Venda
        public ActionResult Index()
        {
            try
            {
                var vendas = _dbContext.Vendas.Include(v => v.Veiculo)
                                              .Include(v => v.Concessionaria)
                                              .Include(v => v.Cliente)
                                              .Where(v => v.BitAtivo)
                                              .ToList();
                ViewBag.Veiculos = _dbContext.Veiculos.Where(v => v.BitAtivo).ToList();
                ViewBag.Concessionarias = _dbContext.Concessionarias.ToList();
                ViewBag.Clientes = _dbContext.Clientes.ToList();
                return View(vendas);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar as vendas: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Venda-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Venda venda)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    venda.ProtocoloVenda = Guid.NewGuid().ToString().Substring(0, 20);

                    _dbContext.Vendas.Add(venda);
                    _dbContext.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao criar venda: " + ex.Message });
                }
            }
            else
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errorMessage = string.Join(" ", errorMessage) });
            }
        }

        // GET: Venda-Edit
        [HttpGet]
        public ActionResult GetVenda(int id)
        {
            try
            {
                var venda = _dbContext.Vendas.Find(id);
                if (venda == null || !venda.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    venda.VendaID,
                    venda.VeiculoID,
                    venda.ConcessionariaID,
                    venda.ClienteID,
                    venda.DataVenda,
                    venda.PrecoVenda
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar venda: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Venda-Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Venda venda)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // N.º Protocolo sempre o mesmo
                    var originalVenda = _dbContext.Vendas.AsNoTracking().FirstOrDefault(v => v.VendaID == venda.VendaID);
                    if (originalVenda != null)
                    {
                        venda.ProtocoloVenda = originalVenda.ProtocoloVenda;
                    }

                    _dbContext.Entry(venda).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao editar venda: " + ex.Message });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorMessage = string.Join(" ", errors);
                return Json(new { success = false, errorMessage });
            }
        }

        // GET: Venda-Delete
        public ActionResult GetVendaDetails(int id)
        {
            try
            {
                var venda = _dbContext.Vendas.Include(v => v.Veiculo)
                                             .Include(v => v.Concessionaria)
                                             .Include(v => v.Cliente)
                                             .FirstOrDefault(v => v.VendaID == id && v.BitAtivo);
                if (venda == null)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    venda.VendaID,
                    venda.Veiculo.Modelo,
                    venda.DataVenda,
                    venda.PrecoVenda,
                    Concessionaria = venda.Concessionaria.Nome,
                    Cliente = venda.Cliente.Nome
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar detalhes da venda: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Venda-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var venda = _dbContext.Vendas.Find(id);
                if (venda != null && venda.BitAtivo)
                {
                    try
                    {
                        venda.BitAtivo = false;
                        _dbContext.Entry(venda).State = EntityState.Modified;
                        _dbContext.SaveChanges();

                        TempData["SuccessMessage"] = "Venda deletada com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Erro ao deletar venda: " + ex.Message;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Venda não encontrada.";
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
