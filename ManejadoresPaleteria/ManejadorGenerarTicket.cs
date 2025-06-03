using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EntidadPeleteria;
using System.Globalization;
namespace ManejadoresPaleteria
{
    public class ManejadorGenerarTicket
    {
        public void GenerarTicketPDF(List<ItemVenta> productos, double total)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "ticket.pdf");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(225, PageSizes.A4.Height); // 80mm de ancho, alto variable
                    page.Margin(10);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Courier"));

                    page.Content().Column(col =>
                    {
                        // Encabezado con logo y texto lado a lado
                        col.Item().Row(row =>
                        {
                            var logoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "image.png");
                            var instagramIconPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "instagram.png");

                            if (File.Exists(logoPath))
                            {
                                row.ConstantColumn(90)
                                   .Height(90)
                                   .AlignMiddle()
                                   .Image(logoPath, ImageScaling.FitHeight);
                            }
                            else
                            {
                                row.ConstantColumn(90).Height(90);
                            }

                            row.RelativeColumn()
                               .PaddingLeft(10)
                               .Column(headerCol =>
                               {
                                   headerCol.Item().Text("PALETERIA Y NEVERIA").Bold().FontSize(10);
                                   headerCol.Item().Text("REAL ROJAS").Bold().FontSize(12).AlignCenter();
                                   headerCol.Item().Text(" ").FontSize(9);
                                   headerCol.Item().Text("Tel: 474-4031955").FontSize(9);
                                   headerCol.Item().Text("Dir: Av. Martin Diaz #548").FontSize(9);
                                   headerCol.Item().Text("Horario: 1:00 - 9:30 PM").FontSize(9);

                                   // 👇 Instagram sección justo después de la fecha
                                   headerCol.Item().Row(igRow =>
                                   {
                                       if (File.Exists(instagramIconPath))
                                       {
                                           igRow.ConstantColumn(11)
                                                .Height(11)
                                                .Image(instagramIconPath, ImageScaling.FitHeight);
                                       }

                                       igRow.RelativeColumn().Column(col =>
                                       {
                                           col.Item().PaddingLeft(5).Text("heladeria.realrojas").FontSize(9);
                                       });

                                   });

                               });
                        });

                        col.Item().Text("-------------------------------------------------------");
                        col.Item().Text($"Fecha: { DateTime.Now.ToString("dddd, dd 'de' MMMM yyyy 'Hora:' h:mm tt", new CultureInfo("es-MX"))}").FontSize(9);
                        col.Item().Text("-------------------------------------------------------");

                        // Columnas para los encabezados
                        // Encabezado de columnas
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("Producto").FontSize(9).Bold().AlignCenter();
                            row.RelativeColumn().Text("Prec Unit").FontSize(9).Bold().AlignCenter();
                            row.RelativeColumn().Text("Cant").FontSize(9).Bold().AlignCenter();
                            row.RelativeColumn().Text("Subtotal").FontSize(9).Bold().AlignCenter();
                        });

                        // Línea divisoria
                        col.Item().Text("-------------------------------------------------------");

                        // Productos
                        foreach (var item in productos)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeColumn().Text(item.Nombre).FontSize(5);
                                row.RelativeColumn().Text($"${item.PrecioUnitario:F2}").FontSize(5).AlignCenter();
                                row.RelativeColumn().Text(item.Cantidad.ToString()).FontSize(5).AlignCenter();
                                row.RelativeColumn().Text($"${item.Subtotal:F2}").FontSize(5).AlignCenter();
                            });
                        }


                        col.Item().Text("-------------------------------------------------------");
                        double importe = 200.00; // Importe recibido

                        col.Item().Text("Forma de Pago: Efectivo").FontSize(7);

                        // TOTAL
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn(7).Text("TOTAL:\t").Bold().FontSize(9).AlignRight();
                            row.RelativeColumn(3).PaddingLeft(8).Text($"${total:F2}").Bold().FontSize(9).AlignLeft();
                        });

                        // Importe Recibido
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn(7).Text("Importe Recibido:\t").FontSize(8).AlignRight();
                            row.RelativeColumn(3).PaddingLeft(8).Text($"${importe:F2}").FontSize(8).AlignLeft();
                        });

                        // Cambio
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn(7).Text("Cambio:\t").FontSize(8).AlignRight();
                            row.RelativeColumn(3).PaddingLeft(8).Text($"${(importe - total):F2}").FontSize(8).AlignLeft();
                        });

                        col.Item().Text("\n¡Gracias por su compra!").AlignCenter();
                    });

                });
            });

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                documento.GeneratePdf(fs);
            }

        }
    }
}
