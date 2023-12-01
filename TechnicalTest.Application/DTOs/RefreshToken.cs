using System;

namespace TechnicalTest.Application.DTOs
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = default!;
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
