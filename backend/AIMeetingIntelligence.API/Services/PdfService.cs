using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace AIMeetingIntelligence.API.Services;

public class PdfService
{
    public byte[] Generate(
        string title,
        string summary,
        List<string> tasks)
    {
        var document = new PdfDocument();

        var page = document.AddPage();

        var gfx = XGraphics.FromPdfPage(page);

        var titleFont = new XFont("Arial", 20, XFontStyle.Bold);
        var heading = new XFont("Arial", 14, XFontStyle.Bold);
        var body = new XFont("Arial", 12);

        int y = 40;

        gfx.DrawString(
            "AI Meeting Report",
            titleFont,
            XBrushes.DarkBlue,
            40,
            y);

        y += 40;

        gfx.DrawString(
            $"Meeting: {title}",
            heading,
            XBrushes.Black,
            40,
            y);

        y += 30;

        gfx.DrawString(
            "AI Summary",
            heading,
            XBrushes.DarkGreen,
            40,
            y);

        y += 20;

        gfx.DrawString(
            summary,
            body,
            XBrushes.Black,
            new XRect(40, y, 500, 120),
            XStringFormats.TopLeft);

        y += 100;

        gfx.DrawString(
            "Action Items",
            heading,
            XBrushes.Purple,
            40,
            y);

        y += 20;

        foreach (var task in tasks)
        {
            gfx.DrawString(
                $"• {task}",
                body,
                XBrushes.Black,
                50,
                y);

            y += 20;
        }

        using var stream = new MemoryStream();
        document.Save(stream);

        return stream.ToArray();
    }
}