using System.Collections.Generic;

namespace ProyectoDIV1.Entidades.Models
{
    public class Jobs
    {
        public List<Job> data { get; set; }
    }

    public class Job
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
