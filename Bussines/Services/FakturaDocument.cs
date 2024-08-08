using Data.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Bussines.Services;

public class FakturaDocument : IDocument
{
    private readonly Facture _facture;

    public FakturaDocument(Facture facture)
    {
        ArgumentNullException.ThrowIfNull(facture);
        _facture = facture;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.DefaultTextStyle(style => style.FontSize(10));
            page.MarginTop(2, Unit.Centimetre);
            page.MarginHorizontal(2, Unit.Centimetre);
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Height(90).AlignLeft().Text("EM-KA").FontSize(60).ExtraBold();

            row.ConstantItem(150).Height(90).Column(stack =>
            {
                stack.Spacing(5);
                stack.Item().Column(comlumn =>
                {
                    comlumn.Item().Background(Colors.Grey.Lighten2).BorderBottom(1).Text("Miejsce wystawienia:").Bold().AlignCenter();
                    comlumn.Item().AlignCenter().Text(text => text.Span(_facture.City).Style(TextStyle.Default.FontSize(10)));
                });

                stack.Item().Column(comlumn =>
                {
                    comlumn.Item().Background(Colors.Grey.Lighten2).BorderBottom(1).Text("Data wystawienia:").Bold().AlignCenter();
                    comlumn.Item().AlignCenter().Text(text => text.Span($"{_facture.CreationDate:dd-MM-yyyy}").Style(TextStyle.Default.FontSize(10)));
                });

                stack.Item().Column(comlumn =>
                {
                    comlumn.Item().Background(Colors.Grey.Lighten2).BorderBottom(1).Text("Data sprzedaży:").Bold().AlignCenter();
                    comlumn.Item().AlignCenter().Text(text => text.Span($"{_facture.SaleDate:dd-MM-yyyy}").Style(TextStyle.Default.FontSize(10)));
                });
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(stack =>
        {
            stack.Spacing(30);

            stack.Item().Text(_facture.Name).FontSize(20).Bold().AlignCenter();

            stack.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Column(stack =>
                {
                    stack.Item().BorderBottom(1).Background(Colors.Grey.Lighten2).AlignCenter().Text(text => text.Span("Sprzedawca:").Style(TextStyle.Default.FontSize(10).Bold()));
                    stack.Item().Text(_facture.UserCompany.Name);
                    stack.Item().Text(_facture.UserCompany.Address.Street + " " + _facture.UserCompany.Address.HouseNumber + "\\" + _facture.UserCompany.Address.LocalNumber);
                    stack.Item().Text($"{_facture.UserCompany.Address.PosteCode} {_facture.UserCompany.Address.City}, {_facture.UserCompany.Address.Country}");
                    stack.Item().Text("Nip: " + _facture.UserCompany.NIP);
                    stack.Item().Text("Nr konta: 12 3456 7890 1234 5678 9012 3456");
                });

                row.RelativeItem().PaddingLeft(10).Column(stack =>
                {
                    stack.Item().BorderBottom(1).Background(Colors.Grey.Lighten2).AlignCenter().Text(text => text.Span("Nabywca:").Style(TextStyle.Default.FontSize(10).Bold()));
                    stack.Item().Text(_facture.Company.Name);
                    stack.Item().Text(_facture.Company.Address.Street + " " + _facture.Company.Address.HouseNumber + "\\" + _facture.Company.Address.LocalNumber);
                    stack.Item().Text($"{_facture.Company.Address.PosteCode} {_facture.Company.Address.City}, {_facture.Company.Address.Country}");
                    stack.Item().Text("NIP: " + _facture.Company.NIP);
                    stack.Item().Text("Nr konta: 12 3456 7890 1234 5678 9012 3456");
                });
            });

            decimal totalPrice = 0;
            decimal totalPaid = 0;
            stack.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);
                    columns.RelativeColumn(5);
                    columns.ConstantColumn(35);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).AlignLeft().Text("Lp.").Bold();
                    header.Cell().Element(HeaderStyle).AlignLeft().Text("Nazwa").Bold();
                    header.Cell().Element(HeaderStyle).Text("Ilość").Bold();
                    header.Cell().Element(HeaderStyle).Text("Cena Jedn. Netto").Bold();
                    header.Cell().Element(HeaderStyle).Text("Wartość Netto").Bold();
                    header.Cell().Element(HeaderStyle).Text("Vat(%)").Bold();
                    header.Cell().Element(HeaderStyle).Text("Wartość Vat").Bold();
                    header.Cell().Element(HeaderStyle).Text("Wartość Brutto").Bold();
                });

                int i = 1;
                foreach (var factureDetail in _facture.FactureDetails)
                {
                    // Obliczenia
                    var totalNetto = factureDetail.Quantity * factureDetail.UnitPriceNetto;
                    var totalBrutto = factureDetail.Quantity * factureDetail.UnitPriceBrutto;
                    var vatPrice = totalBrutto - totalNetto;
                    table.Cell().Element(CellStyle).AlignLeft().Text(i.ToString());
                    table.Cell().Element(CellStyle).Column(column =>
                    {
                        column.Item()
                        .Text(factureDetail.Name).Bold();
                        column.Item()
                        .Text("Uwagi: " + factureDetail.Comment).FontSize(8);
                    });
                    table.Cell().Element(CellStyle).Text(factureDetail.Quantity.ToString());
                    table.Cell().Element(CellStyle).Text($"{factureDetail.UnitPriceNetto:F2}");
                    table.Cell().Element(CellStyle).Text($"{factureDetail.UnitPriceBrutto:F2}");
                    table.Cell().Element(CellStyle).Text($"{factureDetail.Vat:F2}");
                    table.Cell().Element(CellStyle).Text($"{vatPrice:F2}");
                    table.Cell().Element(CellStyle).Text($"{totalBrutto:F2}");
                    totalPrice += totalBrutto;

                    i++;
                }
            });

            stack.Item().Row(row =>
            {
                row.RelativeItem().PaddingRight(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(20);
                        columns.RelativeColumn(70);
                        columns.RelativeColumn(60);
                        columns.RelativeColumn(60);
                        columns.RelativeColumn(60);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderStyle).Text("Lp.").Bold();
                        header.Cell().Element(HeaderStyle).Text("Metoda").Bold();
                        header.Cell().Element(HeaderStyle).Text("Kwota").Bold();
                        header.Cell().Element(HeaderStyle).Text("Termin płatności").Bold();
                        header.Cell().Element(HeaderStyle).Text("Data płatności").Bold();
                    });

                    int i = 0;
                    foreach (var payment in _facture.Payments)
                    {
                        table.Cell().Element(CellStyle).Text(i.ToString());
                        table.Cell().Element(CellStyle).Text(payment.Method);
                        table.Cell().Element(CellStyle).Text($"{payment.Amount:F2}");
                        table.Cell().Element(CellStyle).Text(payment.PaymentDeadline.ToString("dd.MM.yyyy"));
                        string paymentDate = string.Empty;
                        if (payment.PaymantDate != null)
                        {
                            paymentDate = ((DateTime)payment.PaymantDate).ToString("dd.MM.yyyy");
                            totalPaid += payment.Amount;
                        }

                        table.Cell().Element(CellStyle).Text(paymentDate);
                    }
                });

                row.RelativeItem().PaddingLeft(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderStyle).AlignRight().Text(string.Empty);
                        header.Cell().Element(HeaderStyle).Text("Wartość").Bold();
                    });

                    table.Cell().Element(CellStyle).AlignRight().Text("Razem:").Bold();
                    table.Cell().Element(CellStyle).Text($"{totalPrice:F2}");
                    table.Cell().Element(CellStyle).AlignRight().Text("Zapłacono:").Bold();
                    table.Cell().Element(CellStyle).Text($"{totalPaid:F2}");
                    table.Cell().Element(CellStyle).AlignRight().Text("Pozostało do zapłaty:").Bold();
                    table.Cell().Element(CellStyle).Text($"{totalPrice - totalPaid:F2}");
                });
            });

            stack.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().PaddingRight(5).BorderBottom(1).Background(Colors.Grey.Lighten2).Text("Podpis Sprzedawcy").Bold();
                row.RelativeItem().PaddingLeft(5).BorderBottom(1).Background(Colors.Grey.Lighten2).Text("Podpis Nabywcy").Bold();
            });
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(stack =>
        {
            stack.Item().PaddingBottom(10).AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(x => x.Bold().FontSize(10));
                text.Span("Strna ");
                text.CurrentPageNumber();
                text.Span(" z ");
                text.TotalPages();
            });
        });
    }

    private IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Black).Padding(3).AlignCenter();
    }

    private IContainer HeaderStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten2).Padding(5).AlignCenter();
    }
}