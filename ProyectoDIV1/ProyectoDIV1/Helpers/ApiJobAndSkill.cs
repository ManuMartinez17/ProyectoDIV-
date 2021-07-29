using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Helpers
{
    public class ApiJobAndSkill
    {

        public static RestClient apiClient = new RestClient();
        static ApiJobAndSkill()
        {
            var SERVIDOR = new Uri("https://emsiservices.com/");
            apiClient.BaseUrl = SERVIDOR;
        }
    }
    public class Auth
    {

        public static RestClient apiClient = new RestClient();
        static Auth()
        {
            var SERVIDOR = new Uri("https://auth.emsicloud.com/connect/token");
            apiClient.BaseUrl = SERVIDOR;
        }
    }

}
