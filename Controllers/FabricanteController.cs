using System;
using System.Linq;
using System.Web.Mvc;
using ConcessionariaMVC.Models;
using ConcessionariaMVC.Repositories;

namespace ConcessionariaMVC.Controllers
{
    public class FabricanteController : Controller
    {
        private readonly IFabricanteRepository _fabricanteRepository;

        // Injeção de dependência via construtor
        public FabricanteController(IFabricanteRepository fabricanteRepository)
        {
            _fabricanteRepository = fabricanteRepository;
        }

        // GET: Fabricante
        public ActionResult Index()
        {
            try
            {
                var fabricantes = _fabricanteRepository.GetFabricantesAtivos().ToList();
                return View(fabricantes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar fabricantes: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Fabricante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                if (_fabricanteRepository.FabricanteExiste(fabricante.Nome))
                {
                    return Json(new { success = false, errorMessage = "Nome do fabricante já existe. Por favor, escolha outro." });
                }
                try
                {
                    _fabricanteRepository.Add(fabricante);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao criar fabricante: " + ex.Message });
                }
            }
            else
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, errorMessage = string.Join(" ", errorMessage) });
            }
        }

        // POST: Fabricante/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                if (_fabricanteRepository.FabricanteExiste(fabricante.Nome, fabricante.FabricanteID))
                {
                    return Json(new { success = false, errorMessage = "Nome do fabricante já existe. Por favor, escolha outro." });
                }
                try
                {
                    _fabricanteRepository.Update(fabricante);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = "Erro ao editar fabricante: " + ex.Message });
                }
            }
            else
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, errorMessage = string.Join(" ", errorMessage) });
            }
        }

        // GET: Fabricante/GetFabricante/5
        [HttpGet]
        public ActionResult GetFabricante(int id)
        {
            try
            {
                var fabricante = _fabricanteRepository.GetById(id);
                if (fabricante == null || fabricante.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    fabricante.FabricanteID,
                    fabricante.Nome,
                    fabricante.PaisOrigem,
                    fabricante.AnoFundacao,
                    fabricante.Site
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar fabricante: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Fabricante/GetFabricanteDetails/5
        public ActionResult GetFabricanteDetails(int id)
        {
            try
            {
                var fabricante = _fabricanteRepository.GetById(id);
                if (fabricante == null || fabricante.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    fabricante.FabricanteID,
                    fabricante.Nome,
                    fabricante.PaisOrigem,
                    fabricante.AnoFundacao,
                    fabricante.Site
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar detalhes do fabricante: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Fabricante/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var fabricante = _fabricanteRepository.GetById(id);
                if (fabricante != null && !fabricante.BitAtivo)
                {
                    try
                    {
                        fabricante.BitAtivo = true;
                        _fabricanteRepository.Update(fabricante);
                        TempData["SuccessMessage"] = "Fabricante deletado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Erro ao deletar fabricante: " + ex.Message;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Fabricante não encontrado.";
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
