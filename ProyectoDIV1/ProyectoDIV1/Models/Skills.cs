using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Skills
    {
        public Attribution[] attributions { get; set; }
        public Skill[] data { get; set; }
    }

    public class Attribution
    {
        public string name { get; set; }
        public string text { get; set; }
    }

    public class Skill
    {
        public string id { get; set; }
        public string name { get; set; }
        public Type type { get; set; }
        public string infoUrl { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
