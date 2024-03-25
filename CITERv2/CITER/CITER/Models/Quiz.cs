using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    [Table("tbl_questionnaires")]
    public class Quiz
    {
        [Column("questionnaireID"), PrimaryKey]
        public int questionID { get; set; }
        [Column("topic")]
        public string topic { get; set; }
        [Column("section")]
        public string section { get; set; }
        [Column("question")]
        public string question { get; set; }
        [Column("qImage")]
        public string qImage { get; set; }
        [Column("choices")]
        public string choices { get; set; }
        [Column("userAnswer")]
        public string userAnswer { get; set; }
        [Column("correctAnswer")]
        public string correctAnswer { get; set; }
    }
}
