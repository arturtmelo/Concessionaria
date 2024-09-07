using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;

namespace ConcessionariaMVC.Controllers
{
    public class VeiculoController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injeta BD via construtor
        public VeiculoController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            try
            {
                var veiculos = _context.Veiculos.Include(v => v.Fabricante).Where(v => v.BitAtivo).ToList();
                ViewBag.Fabricantes = _context.Fabricantes.Where(f => !f.BitAtivo).ToList();
                return View(veiculos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar veículos: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult GetVeiculo(int id)
        {
            try
            {
                var veiculo = _context.Veiculos.Find(id);
                if (veiculo == null || !veiculo.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    veiculo.VeiculoID,
                    veiculo.Modelo,
                    veiculo.AnoFabricacao,
                    veiculo.Preco,
                    veiculo.FabricanteID,
                    veiculo.TipoVeiculo,
                    veiculo.Descricao
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar veículo: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Veiculos.Add(veiculo);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = $"Erro ao criar veículo: {ex.Message}" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorMessage = string.Join(" ", errors);
            return Json(new { success = false, errorMessage });
        }

        // Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(veiculo).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = $"Erro ao editar veículo: {ex.Message}" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorMessage = string.Join(" ", errors);
            return Json(new { success = false, errorMessage });
        }

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var veiculo = _context.Veiculos.Find(id);
                if (veiculo != null && veiculo.BitAtivo)
                {
                    try
                    {
                        veiculo.BitAtivo = false;
                        _context.Entry(veiculo).State = EntityState.Modified;
                        _context.SaveChanges();
                        TempData["SuccessMessage"] = "Veículo deletado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"Erro ao deletar veículo: {ex.Message}";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Veículo não encontrado.";
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
