using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CITER.Models
{
    public class CertProvider
    {
        [MapTo("certProvider")]
        public string Title { get; set; }
    }
}
