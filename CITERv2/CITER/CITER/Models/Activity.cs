using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    [Table("tbl_activities")]
    public class Activity
    {
        [Column("activityID"), PrimaryKey]
        public int ActivitiesID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("score")]
        public string Score { get; set; }
        [Column("percentage")]
        public string Percentage { get; set; }
        [Column("time")]
        public string Time { get; set; }
        [Column("date")]
        public string Date { get; set; }
    }
}
