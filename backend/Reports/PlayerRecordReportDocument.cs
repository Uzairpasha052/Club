using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using backend.DTOs.Reports;

namespace backend.Report;

public class PlayerRecordReportDocuments : IDocument
{
    private readonly List<PlayerRecordReportDto> _players;
    private readonly string _status;
     public PlayerRecordReportDocuments(
        List<PlayerRecordReportDto> players,
        string status)
    {
        _players = players;
        _status = status;
    }

    public DocumentMetadata GetMetadata()
    {
        return new DocumentMetadata
        {
          Title = "Player Record Report",
          Author = "Taekwondo Club Management System",
          Subject = "Player Record"
        };
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            // page.Size(PageSizes.A4);
            page.Margin(30);

            page.DefaultTextStyle(x => 
                x.FontSize(9)
            );

            page.Header()
                .Element(ComposeHeader);

            page.Content()
                .Element(ComposeContent);

            page.Footer()
                .Element(ComposeFooter);
            // .AlignCenter()
            // .Text(text =>
            // {
            //     text.Span("Taekwondo Club Management System  |  Page ");
            //     text.CurrentPageNumber();
            // });           
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item()
                .AlignCenter()
                .Text("ZULFI INTERNATIONAL TAEKWONDO ACADEMY")
                .FontSize(22)
                .Bold();

            column.Item()
                .PaddingTop(5)
                .AlignCenter()
                .Text("PLAYER RECORD REPORT")
                .FontSize(15)
                .Bold();

            column.Item()
                .PaddingTop(10)
                .LineHorizontal(1);

            column.Item()
                .PaddingTop(8)
                .Row(row =>
                {
                    row.RelativeItem()
                    .Text($"Status: {GetStatusText()}")
                    .FontSize(10)
                    .Bold();

                    row.RelativeItem()
                        .AlignRight()
                        .Text($"Total Players: {_players.Count}")
                        .FontSize(10)
                        .Bold();               
                });

            column.Item()
                .PaddingTop(5)
                .AlignRight()
                .Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}")
                .FontSize(9);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingTop(20).Table(table =>
        {
           table.ColumnsDefinition(columns =>
           {
                columns.ConstantColumn(25);  // #
                columns.RelativeColumn(2.2f);   // Name
                columns.RelativeColumn(2.2f);   // Father
                columns.ConstantColumn(40);  // Age
                columns.ConstantColumn(50);  // Gender
                columns.RelativeColumn(1.6f); // Contact
                columns.RelativeColumn(1.2f); // Belt
                columns.RelativeColumn(1.5f); // History
                columns.RelativeColumn(1.1f);   // Fee
                columns.RelativeColumn(1.2f); // Status
           });

           table.Header(header =>
           {
                header.Cell().Element(HeaderCellStyle).Text("#"); 
                header.Cell().Element(HeaderCellStyle).Text("Player Name");
                header.Cell().Element(HeaderCellStyle).Text("Father Name");
                header.Cell().Element(HeaderCellStyle).Text("Age");
                header.Cell().Element(HeaderCellStyle).Text("Gender");
                header.Cell().Element(HeaderCellStyle).Text("Contact");
                header.Cell().Element(HeaderCellStyle).Text("Belt");
                header.Cell().Element(HeaderCellStyle).Text("History");
                header.Cell().Element(HeaderCellStyle).Text("Fee");
                header.Cell().Element(HeaderCellStyle).Text("Status");
           });

           for(int i = 0; i < _players.Count; i++)
            {
                var player = _players[i];

                table.Cell().Element(DataCellStyle).Text((i + 1).ToString());
                table.Cell().Element(DataCellStyle).Text(player.stdName);
                table.Cell().Element(DataCellStyle).Text(player.stdFName);
                table.Cell().Element(DataCellStyle).Text(player.stdAge);
                table.Cell().Element(DataCellStyle).Text(player.stdGender);
                table.Cell().Element(DataCellStyle).Text(player.stdContact);
                table.Cell().Element(DataCellStyle).Text(player.stdBelt);
                table.Cell().Element(DataCellStyle).Text(player.stdHistory);
                table.Cell().Element(DataCellStyle).Text(player.stdFee);
                table.Cell().Element(DataCellStyle).Text(player.stdStatus);
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.PaddingTop(10)
            .Row(row =>
            {
               row.RelativeItem()
                .Text("Taekwondo Club Management System")
                .FontSize(10);

                row.RelativeItem()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                    // .FontSize(8);
            });
    }

    private string GetStatusText()
    {
        return _status.ToLower() switch
        {
            "active" => "Active Players",
            "inactive" => "Inactive Players",
            _ => "All Players"            
        };
    }

    private static IContainer HeaderCellStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten2)
            .Border(1)
            .BorderColor(Colors.Grey.Medium)
            .PaddingVertical(6)
            .PaddingHorizontal(4)
            .AlignCenter()
            .AlignMiddle();
    }

     private static IContainer DataCellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(5)
            .PaddingHorizontal(4)
            .AlignMiddle();
    }

}
