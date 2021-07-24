using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Skills
    {

        public string job_uuid { get; set; }
        public string job_title { get; set; }
        public string normalized_job_title { get; set; }
        public List<Skill> skills { get; set; }
    }

    public class Skill
    {
        public string skill_uuid { get; set; }
        public string skill_name { get; set; }
        public string description { get; set; }
        public string normalized_skill_name { get; set; }
        public decimal importance { get; set; }
        public decimal level { get; set; }
    }
}
