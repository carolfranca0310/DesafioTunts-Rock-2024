using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json.Linq;

namespace desafioTuntsRock2024.Helper
{
    public class SheetsHelper
    {
        private static readonly string[] _scope = { SheetsService.Scope.Spreadsheets };
        private static readonly string _pageName = "Página1";
        private static readonly string _worksheetId = "1zAf-k_maGFfR78mR269QbSvfHi8DxnFImwHH0FIPGTk";
        private SheetsService _sheetsService;

        public SheetsHelper()
        {
            GoogleCredential credencial;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json");
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                credencial = GoogleCredential.FromStream(stream).CreateScoped(_scope);
            }
            _sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = credencial });
        }

        public IList<IList<object>> SpreadSheet()
        {
            var range = $"{_pageName}!A:F";
            SpreadsheetsResource.ValuesResource.GetRequest request = _sheetsService.Spreadsheets.Values.Get(_worksheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    foreach (var col in row)
                    {
                        Console.Write(col + " | ");
                    }
                    Console.WriteLine();
                }

                return values;
            }
            else
            {
                Console.WriteLine("No data found.");
                return null;
            }
        }
        public void UpdateCell(string cell, string text)
        {

            var range = $"{_pageName}!{cell}";
            var valueRange = new ValueRange();

            var oblist = new List<object>() { text };
            valueRange.Values = new List<IList<object>> { oblist };

            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _worksheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }

        public void CalcularNotas()
        {
            var values = SpreadSheet();
            int qtdAulas = 0;

            if (values != null && values.Count > 0)
            {
                if (values[1].Count == 1 && values[1].First().ToString().Contains("Total de aulas no semestre"))
                    qtdAulas = Convert.ToInt32(values[1].First().ToString().Split(":")[1]);

                for (int i = 3; i < values.Count; i++)
                {
                    if (values[i].Count >= 6)
                    {
                        var row = values[i];
                        int qtdFaltas = Convert.ToInt32(row[2]);
                        float p1 = Convert.ToInt32(row[3]);
                        float p2 = Convert.ToInt64(row[4]);
                        float p3 = Convert.ToInt64(row[5]);

                        if (qtdFaltas > (qtdAulas * 0.25))
                        {
                            UpdateCell($"G{i + 4}", "Reprovado Por Falta");
                            UpdateCell($"H{i + 4}", "0");
                        }
                        else
                        {
                            float media = ((p1 + p2 + p3) / 3) / 10;
                            if (media < 5)
                            {
                                UpdateCell($"G{i + 1}", "Reprovado Por Nota");
                                UpdateCell($"H{i + 1}", "0");
                            }

                            else if (media >= 7)
                            {
                                UpdateCell($"G{i + 1}", "Aprovado");
                                UpdateCell($"H{i + 1}", "0");
                            }
                            else
                            {
                                Console.Write("Insira a nota da Avaliação Final: ");
                                string nafstr = Console.ReadLine().Replace(".", ",");

                                if (float.TryParse(nafstr, out float naf))
                                {
                                    UpdateCell($"G{i + 1}", "Exame Final");
                                    UpdateCell($"H{i + 1}", $"{((media + naf) / 2):n1}");

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

