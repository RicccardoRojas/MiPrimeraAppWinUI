using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser.Data;

namespace ManejadoresPaleteria
{
    public class RecortarPDF
    {
        public class TextBoundingBoxExtractionStrategy : IEventListener
        {
            private readonly List<Rectangle> _rectangles = new();

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type == EventType.RENDER_TEXT)
                {
                    var renderInfo = (TextRenderInfo)data;
                    // Obtenemos los rectángulos de descenso y ascenso
                    var descent = renderInfo.GetDescentLine().GetBoundingRectangle();
                    var ascent = renderInfo.GetAscentLine().GetBoundingRectangle();

                    // Calculamos manualmente la unión de ambos
                    var rect = Union(descent, ascent);
                    _rectangles.Add(rect);
                }
            }

            public ICollection<EventType> GetSupportedEvents() => new[] { EventType.RENDER_TEXT };

            public Rectangle GetBoundingBox()
            {
                if (_rectangles.Count == 0)
                    return null;

                var bounding = _rectangles[0];
                for (int i = 1; i < _rectangles.Count; i++)
                {
                    bounding = Union(bounding, _rectangles[i]);
                }
                return bounding;
            }

            private Rectangle Union(Rectangle a, Rectangle b)
            {
                float x1 = Math.Min(a.GetX(), b.GetX());
                float y1 = Math.Min(a.GetY(), b.GetY());
                float x2 = Math.Max(a.GetX() + a.GetWidth(), b.GetX() + b.GetWidth());
                float y2 = Math.Max(a.GetY() + a.GetHeight(), b.GetY() + b.GetHeight());
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
        }

        public void RecortarPdfAlContenido(Stream inputStream, Stream outputStream)
        {
            using var reader = new PdfReader(inputStream);
            using var writer = new PdfWriter(outputStream);
            using var pdfDoc = new PdfDocument(reader, writer);

            var page = pdfDoc.GetFirstPage();

            // Suponiendo que TextBoundingBoxExtractionStrategy es tu clase que extrae el bounding box de texto
            var strategy = new TextBoundingBoxExtractionStrategy();
            var processor = new PdfCanvasProcessor(strategy);
            processor.ProcessPageContent(page);

            var box = strategy.GetBoundingBox() ?? page.GetPageSize();

            float margin = 10f;
            var crop = new iText.Kernel.Geom.Rectangle(
                box.GetX() - margin,
                box.GetY() - margin,
                box.GetWidth() + margin * 2,
                box.GetHeight() + margin * 2
            );

            page.SetMediaBox(crop);
            page.SetCropBox(crop);

            pdfDoc.Close();
        }

    }
}
