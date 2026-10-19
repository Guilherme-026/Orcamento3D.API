namespace Orcamento3D.API.Models;

public class Impressora
{
    public string ModeloImpressora { get; set; }
    public decimal CustoPorHora { get; set; }


    public Impressora(string modeloImpressora, decimal custoPorHora)
    {
        ModeloImpressora = modeloImpressora;
        CustoPorHora = custoPorHora;
    }
    
}


