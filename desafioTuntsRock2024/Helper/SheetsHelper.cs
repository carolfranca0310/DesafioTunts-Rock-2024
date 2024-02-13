using desafioTuntsRock2024.Extensions;
using desafioTuntsRock2024.Models;
using desafioTuntsRock2024.Models.ViewModel;
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
        private static readonly string _pageName = "engenharia_de_software";
        private static readonly string _worksheetId = "1eiRlbDDnbfkN4XjpXdPayqiJpcsQc-tVzcl3jW5lyoo";
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
            var range = $"{_pageName}!A:H";
            SpreadsheetsResource.ValuesResource.GetRequest request = _sheetsService.Spreadsheets.Values.Get(_worksheetId, range);
            var response = request.Execute();
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

        public List<Student> CalculateGrades()
        {
            var values = SpreadSheet();
            int numClasses = 0;
            List<Student> listStudentsRemediation = new();

            if (values != null && values.Count > 0)
            {
                if (values[1].Count == 1 && values[1].First().ToString().Contains("Total de aulas no semestre"))
                    numClasses = Convert.ToInt32(values[1].First().ToString().Split(":")[1]);

                for (int i = 3; i < values.Count; i++)
                {
                    if (values[i].Count >= 6)
                    {

                        var row = values[i];
                        int numAbsences = Convert.ToInt32(row[2]);
                        float p1 = Convert.ToInt32(row[3]);
                        float p2 = Convert.ToInt64(row[4]);
                        float p3 = Convert.ToInt64(row[5]);

                        var student = new Student
                        {
                            Registration = Convert.ToInt32(row[0]),
                            Name = row[1].ToString(),
                            Absence = Convert.ToInt32(row[2]),
                            P1 = Convert.ToInt64(row[3]),
                            P2 = Convert.ToInt64(row[4]),
                            P3 = Convert.ToInt64(row[5]),
                        };

                        if (numAbsences > (numClasses * 0.25))
                        {
                            UpdateCell($"G{i + 1}", SituationEnum.AbsenteeFailure.GetDescription());
                            UpdateCell($"H{i + 1}", "0");

                            student.Situation = SituationEnum.AbsenteeFailure;
                        }
                        else
                        {
                            float avg = ((p1 + p2 + p3) / 3) / 10;
                            if (avg < 5)
                            {
                                UpdateCell($"G{i + 1}", SituationEnum.GradeFailure.GetDescription());
                                UpdateCell($"H{i + 1}", "0");

                                student.Situation = SituationEnum.GradeFailure;
                            }

                            else if (avg >= 7)
                            {
                                UpdateCell($"G{i + 1}", SituationEnum.Passed.GetDescription());
                                UpdateCell($"H{i + 1}", "0");

                                student.Situation = SituationEnum.Passed;
                            }
                            else
                            {
                                student.Situation = SituationEnum.FinalExam;
                                UpdateCell($"G{i + 1}", SituationEnum.FinalExam.GetDescription());
                            }
                        }

                        listStudentsRemediation.Add(student);
                    }
                }
            }

            return listStudentsRemediation;
        }

        public IEnumerable<StudentAvgViewModel> CalculateFinalGrade(List<StudentGradeRecordViewModel> studentsGradeRecord)
        {
            var listSudentAvg = new List<StudentAvgViewModel>();

            foreach (var student in studentsGradeRecord)
            {
                float avg = ((student.P1 + student.P2 + student.P3) / 3) / 10;
                
                UpdateCell($"H{student.Registration + 3}", $"{((avg + student.FinalGrade) / 2):n1}");

                listSudentAvg.Add(new StudentAvgViewModel
                {
                    Registration = student.Registration,
                    FinalGrade = student.FinalGrade
                });
            }

            return listSudentAvg;
        }

        public List<Student> ConvertSpreadsheet(IList<IList<object>> values)
        {
            List<Student> listStudents = new List<Student>();

            for (int i = 3; i < values.Count; i++)
            {
                if (values[i].Count >= 6)
                {

                    var row = values[i];


                    var student = new Student
                    {
                        Registration = Convert.ToInt32(row[0]),
                        Name = row[1].ToString(),
                        Absence = Convert.ToInt32(row[2]),
                        P1 = Convert.ToInt64(row[3]),
                        P2 = Convert.ToInt64(row[4]),
                        P3 = Convert.ToInt64(row[5]),
                    };

                    if (row.ElementAtOrDefault(6).ToString() != 0.ToString())
                    {
                        if (row[6].ToString() == SituationEnum.FinalExam.GetDescription())
                            student.Situation = SituationEnum.FinalExam;

                        else if (row[6].ToString() == SituationEnum.AbsenteeFailure.GetDescription())
                            student.Situation = SituationEnum.AbsenteeFailure;

                        else if (row[6].ToString() == SituationEnum.Passed.GetDescription())
                            student.Situation = SituationEnum.Passed;

                        else if (row[6].ToString() == SituationEnum.GradeFailure.GetDescription())
                            student.Situation = SituationEnum.GradeFailure;
                    }

                    if (row.Count >= 7) 
                        student.FinalPassing = float.Parse((row[7].ToString()));

                    listStudents.Add(student);

                }
            }
            return listStudents;
        }
    }
}


