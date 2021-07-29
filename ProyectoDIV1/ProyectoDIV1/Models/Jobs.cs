using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Jobs
    {
        public Job[] data { get; set; }
    }

    public class Job
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
