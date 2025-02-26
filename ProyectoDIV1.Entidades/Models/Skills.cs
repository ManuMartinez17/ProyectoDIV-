﻿using System.Collections.Generic;

namespace ProyectoDIV1.Entidades.Models
{
    public class Skills
    {
        public Attribution[] attributions { get; set; }
        public List<Skill> data { get; set; }
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
