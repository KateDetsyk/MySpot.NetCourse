﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySpot.Infrastructure.Auth
{
    internal sealed class Authenticator : IAuthenticator
    {
        private readonly IClock _clock;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly TimeSpan _expiry;
        private readonly SigningCredentials _signingCredetials;
        private readonly JwtSecurityTokenHandler _jwtSecurityToken = new JwtSecurityTokenHandler();

        public Authenticator(IOptions<AuthOptions> options, IClock clock) 
        {
            _clock = clock;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
            _signingCredetials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.Value.SigningKey)), 
                SecurityAlgorithms.HmacSha256);
        }

        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = _clock.Current();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var expires = now.Add(_expiry);
            var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredetials);
            var token = _jwtSecurityToken.WriteToken(jwt);

            return new JwtDto
            {
                AccessToken = token,
            };
        }
    }
}
