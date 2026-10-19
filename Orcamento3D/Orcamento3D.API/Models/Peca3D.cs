namespace Orcamento3D.API.Models;

public class Peca3D
{
    public string Nome { get; set; }
    public double PesoPorGrama { get; set; }
    public double TempoImpressaoHora { get; set; }


    public Peca3D(string nome, double pesoPorGrama, double tempoImpressaoHora)
    {
        Nome = nome;
        PesoPorGrama = pesoPorGrama;
        TempoImpressaoHora = tempoImpressaoHora;
    }
    
}