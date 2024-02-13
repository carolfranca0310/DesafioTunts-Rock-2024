using System.ComponentModel;

namespace desafioTuntsRock2024.Models
{
    public enum SituationEnum
    {
        [Description("Reprovado Por Falta")]
        AbsenteeFailure,

        [Description("Aprovado")]
        Passed,

        [Description("Reprovado Por Nota")]
        GradeFailure,

        [Description("Exame Final")]
        FinalExam

    }
}
