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
        var peca3d = new Peca3D(dados.Nome, dados.PesoPorGrama, dados.TempoImpressaoHora);

        var orcamento = new Orcamento(peca3d, filamento, impressora);

        var resposta = new
        {
            Item = peca3d.Nome,
            CustoPorGramaFilamento = filamento.CalcularCustoPorGrama(),
            CustoMaterial = orcamento.CustoMaterial(),
            CustoMaquina = orcamento.CustoImpressora(),
            CustoTotalProducao = orcamento.CustoTotal(),
            SugestaoDeVenda = new
            {
                LowTicket = orcamento.LowTicketOrcamento(),
                AverageTicket = orcamento.AverageTicketOrcamento(),
                HighTicket = orcamento.HighTicketOrcamento()
            }
        };

        return Ok(resposta);
    }


    public class DadosCalculoDto
    {
        // informações da classe Filamento
        public string Cor { get; set; } = string.Empty;
        public decimal PrecoCarretel { get; set; }
        public double PesoTotalGramas { get; set; }
        
        // informações da classe Impressora
        public string ModeloImpressora { get; set; } = string.Empty;
        public decimal CustoPorHora { get; set; }
        
        // informações da classe Peca3D
        public string Nome { get; set; } = string.Empty;
        public double PesoPorGrama { get; set; }
        public double TempoImpressaoHora { get; set; }
    }
}