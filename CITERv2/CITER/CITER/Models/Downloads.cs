using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    [Table("tbl_downloads")]
    public class Downloads
    {
        [Column("topicID"), PrimaryKey]
        public int TopicID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Desc { get; set; }
        [Column("provider")]
        public string Provider { get; set; }
    }
}
