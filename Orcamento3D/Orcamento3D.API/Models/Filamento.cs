namespace Orcamento3D.API.Models;

public class Filamento
{
    public string Cor { get; set; }
    public decimal PrecoCarretel { get; set; }
    public double PesoTotalGramas { get; set; }


    public Filamento(string cor, decimal precoCarretel, double pesoTotalGramas)
    {
        Cor = cor;
        PrecoCarretel = precoCarretel;
        PesoTotalGramas = pesoTotalGramas;
    }

    public decimal CalcularCustoPorGrama()
    {
        return PrecoCarretel / (decimal)PesoTotalGramas;
    }
    
}