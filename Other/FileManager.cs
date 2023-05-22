using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmergencySituations.Controllers;
using EmergencySituations.DataBase;
using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;

namespace EmergencySituations.Other
{
    public static class FileManager
    {
        public static bool CreateDoc(string fileName, int year, int month = 0)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document, true))
            {
                var emergency = MyDataBase.Select<Emergency>().Where(i => i.DateAndTime.Year == year);
                if (emergency.Count() <= 0)
                    return false;
                var statistic = StatisticController.CalculateData(0).Where(y => y.Date == year).First();
                var text = $"{year} рік";
                if(month > 0)
                {
                    emergency = emergency.Where(i => i.DateAndTime.Month == month);
                    if (emergency.Count() <= 0)
                        return false;
                    statistic = StatisticController.CalculateData(year).Where(m => m.Date == month).First();
                    text = $"{new DateTime(2002, month, 28).ToString("MMMM")} {year}";
                }
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                SectionProperties props = new SectionProperties();
                body.AppendChild(props);

                AddText(doc, CreateText($"Звіт за {text}", 16, true));
                AddText(doc, CreateText(""));
                AddText(doc, CreateText($"- Усього подій: {statistic.TotalCount}", 16));
                AddText(doc, CreateText($"- Збитки: {statistic.Losses.Costs}", 16));

                AddText(doc, CreateText("Кількість подій за рівнем:"));
                InsertWordTable(doc, statistic.Level.ToTable());

                AddText(doc, CreateText("Кількість подій за типом:"));
                InsertWordTable(doc, statistic.Type.ToTable());

                AddText(doc, CreateText(""));
                AddText(doc, CreateText(""));
                AddText(doc, CreateText(""));
                int i = 1;
                foreach (var e in emergency)
                {
                    CreateEmergencyData(doc, e, i++);
                    AddText(doc, CreateText(""));
                }

                statistic = null;
                emergency = null;
            }
            return true;
        }


        private static string CheckInfo(string text)
        {
            return (string.IsNullOrEmpty(text) ? "Інформації немає" : text);
        }

        private static void CreateEmergencyData(WordprocessingDocument doc, Emergency emergency, int id)
        {
            AddText(doc, CreateText($"{id}. {emergency.Name}", 16));
            AddText(doc, CreateText($"{emergency.DateAndTime.ToString("D")} | {emergency.DateAndTime.ToString("t")}", 14, false, new Italic()));
            AddText(doc, CreateText(""));
            AddText(doc, CreateText($"- Опис: {CheckInfo(emergency.Description)}"));
            AddText(doc, CreateText($"- Рівень: {emergency.Level.Name}"));
            AddText(doc, CreateText($"- Тип: {emergency.Type.Name}"));
            AddText(doc, CreateText($"- Розташування: {CheckInfo(String.Join(',', emergency.Positions.Select(i => i.Location).Distinct()))}"));

            var e = emergency.Losses;
            AddText(doc, CreateText($"- Збитки: {e.Costs} (тис. гривень)"));
            AddText(doc, CreateText($"- Втрати:"));
            var b = new string[,] {
                { "Назва", "Кількість" },
                { "Кількість осіб з втратою працездатності до 9 днів", e.EasyAccident.ToString() },
                { "Кількість осіб з втратою працездатності понад 9 днів", e.HardAccident.ToString() },
                { "Кількість осіб з отриманою інвалідністю", e.DisabilityPerson.ToString() },
                { "Кількість загиблих до 16 років", e.DeathPersonUndersSixteen.ToString() },
                { "Кількість загиблих від 16 до 60 років", e.DeathPersonUnderSixty.ToString() },
                { "Збитки від пошкоджених або зруйнованих будівель", e.DestroyedBuildings.ToString() },
                { "Збитки від пошкоджених або знищених особистих речей", e.DamagedPersonalItems.ToString() },
            };
            InsertWordTable(doc, b);
            AddText(doc, CreateText(""));
            e = null;
            b = null;
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
            string font = "Times New Roman";
            Run run = paragraph.AppendChild(new Run());
            run.AppendChild(new Text(text));
            var prop = new RunProperties(new FontSize() { Val = new StringValue((size * 2).ToString()) },
                new RunFonts() { HighAnsi = font, Ascii = font });
            foreach (var e in ex)
            {
                prop.AddChild(e);
            }
            var pp = new ParagraphProperties();
            if (center)
                pp.AddChild(new Justification() { Val = JustificationValues.Center });
            paragraph.AppendChild(pp);

            run.RunProperties = prop;
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
