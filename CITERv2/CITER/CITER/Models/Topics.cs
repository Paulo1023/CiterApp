using Plugin.CloudFirestore.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    
    public class Topics
    {
        public int TopicID { get; set; }
        [MapTo("title")]
        public string Title { get; set; }
        [MapTo("Description")]
        public string Desc { get; set; }
        [MapTo("Provider")]
        public string Provider { get; set; }
        [MapTo("ProviderID")]
        public string ProviderID { get; set; }

    }
}
