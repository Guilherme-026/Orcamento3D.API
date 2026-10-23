namespace Orcamento3D.API.Services;
using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
    
    public byte[] GerarPdf(string ticketEscolhido)
{
    // 1. Define qual será o Valor Final com base no botão clicado na interface
    decimal valorFinal = ticketEscolhido?.ToLower() switch
    {
        "low" => LowTicketOrcamento(),
        "high" => HighTicketOrcamento(),
        _ => AverageTicketOrcamento() // Padrão caso venha "average"
    };

    // 2. Calcula a "Mão de Obra / Acabamento" (Diferença entre o Preço Cobrado e o Custo Base)
    decimal custoBase = CustoTotal();
    decimal maoDeObra = valorFinal - custoBase;

    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(1.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Grey.Darken3));

            // CABEÇALHO
            page.Header().BorderBottom(2).BorderColor(Colors.Blue.Darken2).PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("ESTÚDIO DE IMPRESSÃO 3D").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text("Orçamento Técnico e Comercial").FontSize(10).FontColor(Colors.Grey.Medium);
                });

                row.RelativeItem().AlignRight().Column(col =>
                {
                    col.Item().Text("ORÇAMENTO OFICIAL").FontSize(14).Bold();
                    col.Item().Text($"Data: {DateTime.Now:dd/MM/yyyy}").FontSize(10);
                });
            });

            // CORPO DO DOCUMENTO
            page.Content().PaddingVertical(20).Column(col =>
            {
                col.Spacing(20);

                // Bloco A: Especificações da Peça
                col.Item().Background(Colors.Grey.Lighten4).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12).Column(box =>
                {
                    box.Spacing(8);
                    box.Item().Text($"PEÇA: {Peca.Nome}").FontSize(14).Bold().FontColor(Colors.Black);
                    box.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    box.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text($"• Cor do Filamento: {Filamento.Cor}");
                            c.Item().Text($"• Impressora: {Impressora.ModeloImpressora}");
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text($"• Peso Estimado: {Peca.PesoPorGrama}g");
                            
                            // Exemplo de formatação limpa no QuestPDF:
                            double horasTotais = Peca.TempoImpressaoHora; // Ex: 1.01666...
                            int horas = (int)horasTotais;
                            int minutos = (int)Math.Round((horasTotais - horas) * 60);

                            string tempoFormatado = minutos > 0 ? $"{horas}h {minutos}min" : $"{horas}h";

                            // No QuestPDF, você usa a string formatada:
                            c.Item().Text($"• Tempo de Impressão: {tempoFormatado}");   
                        });
                    });
                });

                // Bloco B: Composição de Custos + Mão de Obra (Justificativa do Preço!)
                col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12).Column(box =>
                {
                    box.Spacing(8);
                    box.Item().Text("DETALHAMENTO DOS CUSTOS E SERVIÇOS").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                    box.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    box.Item().Text($"• Consumo de Material (Filamento): R$ {CustoMaterial():F2}");
                    box.Item().Text($"• Operação e Energia do Equipamento: R$ {CustoImpressora():F2}");
                    box.Item().Text($"• Preparação (Fatiamento), Mão de Obra e Acabamento: R$ {maoDeObra:F2}");
                });

                // Bloco C: Valor Único Final para o Cliente!
                col.Item().Border(2).BorderColor(Colors.Blue.Darken2).Background(Colors.Blue.Lighten5).Padding(16).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("VALOR TOTAL DO ORÇAMENTO").FontSize(14).Bold().FontColor(Colors.Blue.Darken3);
                        c.Item().Text("Inclui material, tempo de máquina e acabamento técnico").FontSize(10).FontColor(Colors.Grey.Darken2);
                    });

                    row.RelativeItem().AlignRight().AlignMiddle()
                        .Text($"R$ {valorFinal:F2}").FontSize(22).Bold().FontColor(Colors.Blue.Darken4);
                });
            });

            // RODAPÉ
            page.Footer().BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(8).AlignCenter()
                .Text("Orçamento válido por 7 dias • Agradecemos a preferência!")
                .FontSize(9).FontColor(Colors.Grey.Medium);
        });
    }).GeneratePdf();
}   
    
}