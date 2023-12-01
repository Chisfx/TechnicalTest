﻿using System;
using System.Text.Json.Serialization;
namespace TechnicalTest.Application.DTOs
{
    public class TokenResponse
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; } = default!;
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = default!;
    }
}
