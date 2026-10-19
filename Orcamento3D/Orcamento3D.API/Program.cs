namespace Orcamento3D.API;

using Orcamento3D.API.Models;
using Orcamento3D.API.Services;

class Program
{
    static void Main(string[] args)
    {
        var filamento = new Filamento("Preto", 80, 1000);
        var impressora = new Impressora("Bambu LabA1", 5);
        var peca = new Peca3D("Suporte de Celular", 34, 1);

        var orcamento = new Orcamento(peca, filamento, impressora);
        
        Console.WriteLine($"Custo da grama do filamento: R$ {filamento.CalcularCustoPorGrama()}");
        Console.WriteLine($"Custo de tempo de impressão: R$ {orcamento.CustoImpressora()}");
        Console.WriteLine($"Custo de material: R$ {orcamento.CustoMaterial()}");
        Console.WriteLine($"Custo total: R$ {orcamento.CustoTotal()}");
        
        Console.WriteLine($"Orçamento de LowTicket: R$ {orcamento.LowTicketOrcamento():F2}");
        Console.WriteLine($"Orçamento AveregeTicket: R$ {orcamento.AverageTicketOrcamento():F2}");
        Console.WriteLine($"Orçamento de HighTicket: R$ {orcamento.HighTicketOrcamento():F2}");

    }
}