using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    [Table("tbl_questions")]
    public class Exam
    {
        [Column("questionnaireID")]
        public int QuestionnaireID { get; set; }
        [Column("activityID"), PrimaryKey]
        public int ActivityID { get; set; }
        [Column("topic")]
        public string Topic { get; set; }
        [Column("section")]
        public string Section { get; set; }
        [Column("question")]
        public string Question { get; set; }
        [Column("qImage")]
        public string qImage { get; set; }
        [Column("choices")]
        public string choices { get; set; }
        [Column("correctAnswer")]
        public string CorrectAnswer { get; set; }
        [Column("userAnswer")]
        public string UserAnswer { get; set; }
    }
}
