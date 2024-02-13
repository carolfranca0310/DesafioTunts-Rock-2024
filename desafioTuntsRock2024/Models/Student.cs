namespace desafioTuntsRock2024.Models
{
    public class Student
    {
        public int Registration { get; set; }
        public string Name { get; set; }
        public int Absence { get; set; }
        public float P1 { get; set; }
        public float P2 { get; set; }
        public float P3 { get; set; }
        public SituationEnum Situation { get; set; }
        public float FinalPassing { get; set; }
        public float FinalGrade { get; set; }
    }
}
