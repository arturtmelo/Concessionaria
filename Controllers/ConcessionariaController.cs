using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ConcessionariaMVC.Models;
using ConcessionariaMVC.Repositories;
using ConcessionariaMVC.Services;

namespace ConcessionariaMVC.Controllers
{
    public class ConcessionariaController : Controller
    {
        private readonly IConcessionariaRepository _concessionariaRepository;
        private readonly CepService _cepService;

        // Injeta repositório e serviço de CEP via construtor
        public ConcessionariaController(IConcessionariaRepository concessionariaRepository, CepService cepService)
        {
            _concessionariaRepository = concessionariaRepository;
            _cepService = cepService;
        }

        // GET: Concessionaria
        public ActionResult Index()
        {
            try
            {
                var concessionarias = _concessionariaRepository.GetConcessionariasAtivas().ToList();
                return View(concessionarias);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar concessionárias: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Concessionaria concessionaria)
        {
            if (ModelState.IsValid)
            {
                if (_concessionariaRepository.ConcessionariaExiste(concessionaria.Nome))
                {
                    ModelState.AddModelError("", "Nome da concessionária já existe. Por favor, escolha outro.");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(concessionaria.CEP))
                    {
                        try
                        {
                            var cepInfo = await _cepService.GetCepInfoAsync(concessionaria.CEP);
                            if (cepInfo != null)
                            {
                                concessionaria.Logradouro = cepInfo.logradouro;
                                concessionaria.Cidade = cepInfo.localidade;
                                concessionaria.Estado = cepInfo.uf;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Erro ao buscar informações de CEP: " + ex.Message);
                            return Json(new { success = false, errorMessage = "Erro ao buscar informações de CEP: " + ex.Message });
                        }
                    }

                    try
                    {
                        _concessionariaRepository.Add(concessionaria);
                        TempData["SuccessMessage"] = "Concessionária criada com sucesso.";

                        // Envia email para concessionária após cadastro
                        var emailController = new EmailController();
                        var destinatario = concessionaria.Email;
                        var assunto = "Bem-vindo à nossa rede de Concessionárias!";
                        var mensagem = $"Olá {concessionaria.Nome},<br/><br/>" +
                                       $"Sua concessionária foi criada com sucesso em nossa plataforma.<br/>" +
                                       $"Localização: {concessionaria.Logradouro}, {concessionaria.Cidade}, {concessionaria.Estado}.<br/>" +
                                       $"Capacidade Máxima de Veículos: {concessionaria.CapacidadeMaximaVeiculos}.<br/><br/>" +
                                       $"Agradecemos por fazer parte de nossa rede.<br/><br/>";

                        emailController.EnviarEmail(destinatario, assunto, mensagem);

                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Erro ao criar concessionária: " + ex.Message);
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorMessage = string.Join(" ", errors);
            return Json(new { success = false, errorMessage });
        }

        // POST: Concessionaria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Concessionaria concessionaria)
        {
            if (ModelState.IsValid)
            {
                if (_concessionariaRepository.ConcessionariaExiste(concessionaria.Nome, concessionaria.ConcessionariaID))
                {
                    ModelState.AddModelError("", "Nome da concessionária já existe. Por favor, escolha outro.");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(concessionaria.CEP))
                    {
                        try
                        {
                            var cepInfo = await _cepService.GetCepInfoAsync(concessionaria.CEP);
                            if (cepInfo != null)
                            {
                                concessionaria.Logradouro = cepInfo.logradouro;
                                concessionaria.Cidade = cepInfo.localidade;
                                concessionaria.Estado = cepInfo.uf;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Erro ao buscar informações de CEP: " + ex.Message);
                            return Json(new { success = false, errorMessage = "Erro ao buscar informações de CEP: " + ex.Message });
                        }
                    }

                    try
                    {
                        _concessionariaRepository.Update(concessionaria);
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Erro ao editar concessionária: " + ex.Message);
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorMessage = string.Join(" ", errors);
            return Json(new { success = false, errorMessage });
        }

        // GET: Concessionaria/GetConcessionaria/5
        [HttpGet]
        public async Task<ActionResult> GetConcessionaria(int id)
        {
            try
            {
                var concessionaria = _concessionariaRepository.GetById(id);
                if (concessionaria == null || !concessionaria.BitAtivo)
                {
                    return HttpNotFound();
                }

                return Json(new
                {
                    concessionaria.ConcessionariaID,
                    concessionaria.Nome,
                    concessionaria.Logradouro,
                    concessionaria.Cidade,
                    concessionaria.Estado,
                    concessionaria.CEP,
                    concessionaria.Telefone,
                    concessionaria.Email,
                    concessionaria.CapacidadeMaximaVeiculos
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Erro ao buscar concessionária: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Concessionaria/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var concessionaria = _concessionariaRepository.GetById(id);
                if (concessionaria != null && concessionaria.BitAtivo)
                {
                    try
                    {
                        concessionaria.BitAtivo = false;
                        _concessionariaRepository.Update(concessionaria);
                        TempData["SuccessMessage"] = "Concessionária excluída com sucesso.";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Erro ao excluir concessionária: " + ex.Message;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Concessionária não encontrada.";
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
