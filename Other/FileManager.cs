using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;

namespace EmergencySituations.Other
{
    public static class FileManager
    {
        public static bool CreateDoc(string fileName, DateTime date)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document, true))
            {
                var emergency = MyDataBase.Select<Emergency>().Where(i => i.DateAndTime.Date == date.Date);
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                SectionProperties props = new SectionProperties();
                body.AppendChild(props);

                AddText(doc, CreateText("ABOBA", 14 , true));

                AddText(doc, CreateText("BOBA", 14, false, new Bold()));


                InsertWordTable(doc, new string[,] { { "1", "2", "C" }, { "A", "B", "3" } });

            }
            return true;
        }

        private static void AddText(WordprocessingDocument doc, Paragraph paragraph)
        {
            MainDocumentPart mainPart = doc.MainDocumentPart;
            Body body = mainPart.Document.Body;
            body.AppendChild(paragraph);
        }

        private static Paragraph CreateText(string text, int size = 14, bool center = false, params OpenXmlElement[] ex)
        {
            Paragraph paragraph = new Paragraph();

            Run run = paragraph.AppendChild(new Run());
            run.AppendChild(new Text(text));
            var prop = new RunProperties(new FontSize() { Val = new StringValue((size * 2).ToString()) },
                new RunFonts() { Ascii = "Times New Roman" });
            foreach (var e in ex)
            {
                prop.AddChild(e);
            }
            var pp = new ParagraphProperties();
            if(center)
                pp.AddChild(new Justification() { Val = JustificationValues.Center });

            run.PrependChild(prop);
            paragraph.AppendChild(pp);
            return paragraph;
        }

        private static void InsertWordTable(WordprocessingDocument doc, string[,] table)
        {
            Table dTable = new Table();

            TableProperties props = new TableProperties();

            var tableWidth = new TableWidth()
            {
                Width = "5000",
                Type = TableWidthUnitValues.Pct,
            };

            var borderValues = new EnumValue<BorderValues>(BorderValues.Single);
            var tableBorders = new TableBorders(
                                 new TopBorder { Val = borderValues, Size = 4 },
                                 new BottomBorder { Val = borderValues, Size = 4 },
                                 new LeftBorder { Val = borderValues, Size = 4 },
                                 new RightBorder { Val = borderValues, Size = 4 },
                                 new InsideHorizontalBorder { Val = borderValues, Size = 4 },
                                 new InsideVerticalBorder { Val = borderValues, Size = 4 });

            props.Append(tableWidth);
            props.Append(tableBorders);


            dTable.AppendChild(props);

            for (int i = 0; i < table.GetLength(0); i++)
            {
                var tr = new TableRow();

                for (int j = 0; j < table.GetLength(1); j++)
                {
                    var tc = new TableCell();
                    Paragraph p = new Paragraph();
                    if (i == 0)
                    {
                       p = CreateText(table[i, j], 14, true, new Bold());
                    }
                    else
                    {
                        p = CreateText(table[i, j], 14, true);
                    }
                    tc.Append(p);
                    tc.Append(new TableCellProperties());

                    tr.Append(tc);
                }
                dTable.Append(tr);
            }
            doc.MainDocumentPart.Document.Body.Append(dTable);
        }
    }
}
