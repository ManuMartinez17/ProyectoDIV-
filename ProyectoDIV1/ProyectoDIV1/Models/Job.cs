using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Job
    {
        public string uuid { get; set; }
        public string suggestion { get; set; }
        public string normalized_job_title { get; set; }
        public string parent_uuid { get; set; }
    }
}
