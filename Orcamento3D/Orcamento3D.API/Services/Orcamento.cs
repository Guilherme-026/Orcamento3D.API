namespace Orcamento3D.API.Services;

using Orcamento3D.API.Models;

public class Orcamento
{
    public Peca3D Peca { get; set; }
    public Filamento Filamento { get; set; }
    public Impressora Impressora { get; set; }


    public Orcamento(Peca3D peca, Filamento filamento,  Impressora impressora)
    {
        Peca = peca;
        Filamento = filamento;
        Impressora = impressora;
    }


    public decimal CustoMaterial()
    {
        return Filamento.CalcularCustoPorGrama() * (decimal)Peca.PesoPorGrama;
    }

    public decimal CustoImpressora()
    {
        return Impressora.CustoPorHora * (decimal)Peca.TempoImpressaoHora;
    }

    public decimal CustoTotal()
    {
       return CustoMaterial() + CustoImpressora();
    }
    
    //LowTicket(competitividade com mercado - valor "mais" baixo)
    public decimal LowTicketOrcamento()
    {
        return CustoTotal() * 1.30m;
    }
    
    //AverageTicket(orçamento padrão imposto pelos responsaveis)
    public decimal AverageTicketOrcamento()
    {
        return CustoTotal() * 1.50m;
    }
    
    //HighTicket(Tempo de urgencia, trabalho adicional na peça, nível de dificuldade maior na criação)
    public decimal HighTicketOrcamento()
    {
        return CustoTotal() * 2.00m;
    }
    
}