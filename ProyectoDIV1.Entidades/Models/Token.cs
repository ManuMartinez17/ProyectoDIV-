﻿using System;

namespace ProyectoDIV1.Entidades.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public DateTime Expiration;
        public DateTime ExpirationLocal => Expiration.ToLocalTime();
    }
    public class Mensaje
    {
        public string message { get; set; }
    }
}
