using CITER.Models;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CITER.Models
{
    public class Questionnaire
    {

        [MapTo("topic")]
        public string topic { get; set; }
        [MapTo("section")]
        public string section { get; set; }
        [MapTo("question")]
        public string question { get; set; }
        [MapTo("qImage")]
        public string qImage { get; set; }
        [MapTo("choices")]
        public string[] choices { get; set; }
        [MapTo("correctAnswer")]
        public string correctAnswer { get; set; }
       

    }
}
