using Microsoft.AspNetCore.Mvc;
using Orcamento3D.API.Models;
using Orcamento3D.API.Services;

namespace Orcamento3D.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrcamentosController : ControllerBase
{

    private static List<Peca3D> _peca = new List<Peca3D>();
    
    
    [HttpGet("peca")]
    public IActionResult ListarOrcamento()
    {
        return Ok(_peca);
    }

    
    [HttpPost("peca")]
    public IActionResult CadastrarPeca([FromBody] Peca3D novaPeca)
    {
        _peca.Add(novaPeca);
        return Created("", novaPeca);
    }


    [HttpPost("calcular-orcamento")]
    public IActionResult CalcularOrcamento([FromBody] DadosCalculoDto dados)
    {
        var filamento = new Filamento(dados.Cor, dados.PrecoCarretel, dados.PesoTotalGramas);
        var impressora = new Impressora(dados.ModeloImpressora, dados.CustoPorHora);
        
        // Converte as horas e minutos vindos da tela em horas decimais para a sua regra de negócio
        double tempoTotalEmHoras = dados.TempoImpressaoHoras + (dados.TempoImpressaoMinutos / 60.0);
        var peca3d = new Peca3D(dados.Nome, dados.PesoPorGrama, tempoTotalEmHoras);
        

        var orcamento = new Orcamento(peca3d, filamento, impressora);

        return Ok(new
        {
            custoPorGrama = filamento.CalcularCustoPorGrama(),
            custoMaterial = orcamento.CustoMaterial(),
            custoImpressora = orcamento.CustoImpressora(),
            custoTotal = orcamento.CustoTotal(),
            lowTicket = orcamento.LowTicketOrcamento(),
            averageTicket = orcamento.AverageTicketOrcamento(),
            highTicket = orcamento.HighTicketOrcamento()
        });
    }
    
    
    [HttpPost("gerar-pdf")]
    public IActionResult GerarPdfOrcamento([FromBody] DadosCalculoDto dados)
    {
        var filamento = new Filamento(dados.Cor, dados.PrecoCarretel, dados.PesoTotalGramas);
        var impressora = new Impressora(dados.ModeloImpressora, dados.CustoPorHora);
            
        // Converte as horas e minutos vindos da tela em horas decimais para a sua regra de negócio
        double tempoTotalEmHoras = dados.TempoImpressaoHoras + (dados.TempoImpressaoMinutos / 60.0);
        var peca3d = new Peca3D(dados.Nome, dados.PesoPorGrama, tempoTotalEmHoras);

        var orcamento = new Orcamento(peca3d, filamento, impressora);

        // Gera os bytes do arquivo PDF
        byte[] arquivoPdf = orcamento.GerarPdf(dados.TicketEscolhido ?? "Average");

        // Retorna o arquivo PDF para download com Status 200 OK!
        return File(arquivoPdf, "application/pdf", $"Orcamento-{peca3d.Nome}.pdf");
    }


    public class DadosCalculoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public double PesoPorGrama { get; set; } // Aceita 72.01
        public int TempoImpressaoHoras { get; set; } // Ex: 4 horas
        public int TempoImpressaoMinutos { get; set; } // Ex: 23 minutos
        public decimal PrecoCarretel { get; set; }
        public double PesoTotalGramas { get; set; }
        public string ModeloImpressora { get; set; } = string.Empty;
        public decimal CustoPorHora { get; set; }
        public string? TicketEscolhido { get; set; }
    }
}