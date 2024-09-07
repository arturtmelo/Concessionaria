using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ConcessionariaMVC.Data;
using ConcessionariaMVC.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;

namespace ConcessionariaMVC.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static readonly string ApplicationName = "ConcessionariaMVC";
        private static readonly string SpreadsheetId = "1mutpya3mIHFj0yjPOZXXE9YAv_K6qjlPnxQAIJLZAH0";
        private static readonly string SheetName = "Relatório Vendas";

        // Injeta BD via construtor
        public RelatorioController()
        {
            _dbContext = new ApplicationDbContext();
        }

        private SheetsService GetSheetsService()
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(Server.MapPath("~/App_Data/credentials"), FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                }

                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao configurar o serviço do Google Sheets: " + ex.Message);
            }
        }

        // GET: Relatorio
        public ActionResult Index()
        {
            try
            {
                var vendasPorMes = _dbContext.Vendas
                    .GroupBy(v => new { v.DataVenda.Month, v.DataVenda.Year })
                    .Select(g => new
                    {
                        Month = g.Key.Month,
                        Year = g.Key.Year,
                        Total = g.Count()
                    })
                    .ToList();

                var averagePrice = _dbContext.Vendas.Average(v => v.PrecoVenda);
                ViewBag.AveragePrice = averagePrice;

                ViewBag.VendasPorMes = vendasPorMes;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar os dados do relatório: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Relatorio/GerarRelatorioMensal
        public ActionResult GerarRelatorioMensal()
        {
            try
            {
                var vendas = _dbContext.Vendas
                    .Include("Veiculo.Fabricante")
                    .Include("Concessionaria")
                    .Include("Cliente")
                    .OrderBy(v => v.DataVenda)
                    .ToList();

                return View(vendas);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao gerar relatório mensal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Relatorio/ExportarPDF
        [HttpPost]
        public FileResult ExportarPDF()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var doc = new Document();
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    doc.Add(new Paragraph("Relatório Mensal de Vendas"));
                    doc.Add(new Paragraph(" "));

                    var averagePrice = _dbContext.Vendas.Average(v => v.PrecoVenda);
                    doc.Add(new Paragraph($"Valor Médio das Vendas: {averagePrice:C}"));
                    doc.Add(new Paragraph(" "));

                    var table = new PdfPTable(6);
                    table.AddCell("Veículo");
                    table.AddCell("Fabricante");
                    table.AddCell("Concessionária");
                    table.AddCell("Cliente");
                    table.AddCell("Data Venda");
                    table.AddCell("Valor");

                    var vendas = _dbContext.Vendas
                        .Include("Veiculo.Fabricante")
                        .Include("Concessionaria")
                        .Include("Cliente")
                        .ToList();

                    foreach (var venda in vendas)
                    {
                        table.AddCell(venda.Veiculo.Modelo);
                        table.AddCell(venda.Veiculo.Fabricante.Nome);
                        table.AddCell(venda.Concessionaria.Nome);
                        table.AddCell(venda.Cliente.Nome);
                        table.AddCell(venda.DataVenda.ToShortDateString());
                        table.AddCell(venda.PrecoVenda.ToString("C"));
                    }

                    doc.Add(table);
                    doc.Close();

                    return File(ms.ToArray(), "application/pdf", "RelatorioVendas.pdf");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao exportar relatório em PDF: " + ex.Message;
                return null;
            }
        }

        // POST: Relatorio/ExportarExcel
        [HttpPost]
        public FileResult ExportarExcel()
        {
            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Relatório Vendas");

                    worksheet.Cells[1, 1].Value = "Veículo";
                    worksheet.Cells[1, 2].Value = "Fabricante";
                    worksheet.Cells[1, 3].Value = "Concessionária";
                    worksheet.Cells[1, 4].Value = "Cliente";
                    worksheet.Cells[1, 5].Value = "Data Venda";
                    worksheet.Cells[1, 6].Value = "Valor";

                    var averagePrice = _dbContext.Vendas.Average(v => v.PrecoVenda);
                    worksheet.Cells[2, 1].Value = "Valor Médio das Vendas:";
                    worksheet.Cells[2, 2].Value = averagePrice.ToString("C");

                    var row = 3;
                    var vendas = _dbContext.Vendas
                        .Include("Veiculo.Fabricante")
                        .Include("Concessionaria")
                        .Include("Cliente")
                        .ToList();

                    foreach (var venda in vendas)
                    {
                        worksheet.Cells[row, 1].Value = venda.Veiculo.Modelo;
                        worksheet.Cells[row, 2].Value = venda.Veiculo.Fabricante.Nome;
                        worksheet.Cells[row, 3].Value = venda.Concessionaria.Nome;
                        worksheet.Cells[row, 4].Value = venda.Cliente.Nome;
                        worksheet.Cells[row, 5].Value = venda.DataVenda.ToShortDateString();
                        worksheet.Cells[row, 6].Value = venda.PrecoVenda.ToString("C");
                        row++;
                    }

                    var stream = new MemoryStream(package.GetAsByteArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RelatorioVendas.xlsx");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao exportar relatório em Excel: " + ex.Message;
                return null;
            }
        }

        // POST: Relatorio/ExportarGoogleSheets
        [HttpPost]
        public async Task<ActionResult> ExportarGoogleSheets()
        {
            try
            {
                var service = GetSheetsService();

                var vendas = _dbContext.Vendas
                    .Include("Veiculo.Fabricante")
                    .Include("Concessionaria")
                    .Include("Cliente")
                    .OrderBy(v => v.DataVenda)
                    .ToList();

                var values = new List<IList<object>>
                {
                    new List<object> { "Veículo", "Fabricante", "Concessionária", "Cliente", "Data Venda", "Valor" }
                };

                foreach (var venda in vendas)
                {
                    values.Add(new List<object>
                    {
                        venda.Veiculo.Modelo,
                        venda.Veiculo.Fabricante.Nome,
                        venda.Concessionaria.Nome,
                        venda.Cliente.Nome,
                        venda.DataVenda.ToShortDateString(),
                        venda.PrecoVenda.ToString("C")
                    });
                }

                var requestBody = new ValueRange
                {
                    Values = values
                };

                var request = service.Spreadsheets.Values.Update(requestBody, SpreadsheetId, $"{SheetName}!A1");
                request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

                var response = await request.ExecuteAsync();

                TempData["Message"] = "Dados exportados com sucesso para o Google Sheets.";
            }
            catch (TaskCanceledException)
            {
                TempData["Error"] = "A operação de exportação para o Google Sheets foi cancelada devido ao tempo limite excedido. Por favor, tente novamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocorreu um erro ao exportar os dados: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
