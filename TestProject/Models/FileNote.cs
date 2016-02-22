using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
    public class FileNote
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}