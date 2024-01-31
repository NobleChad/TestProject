using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TestProject.Data;

namespace TestProject.Services
{
	public class TokenService : ITokenService
	{
		private SignInManager<ApplicationUser> _manager;
		private UserManager<ApplicationUser> _userManager;
		private IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
		private KeyManager _keyManager;
        public TokenService(SignInManager<ApplicationUser> manager,
			UserManager<ApplicationUser> userManager,
			IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
			KeyManager keyManager)
        {
			_manager = manager;
			_userManager = userManager;
			_claimsFactory = claimsFactory;
			_keyManager = keyManager;

		}

		public async Task<(string?,string?)> GenerateTokenAsync(string email, string password)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(email);
				if (user != null)
				{
					var result = await _manager.CheckPasswordSignInAsync(user, password, false);
					if (result.Succeeded)
					{
						var principal = await _claimsFactory.CreateAsync(user);
						var identity = principal.Identities.First();
						identity.AddClaim(new Claim("amr", "pwd"));
						identity.AddClaim(new Claim("method", "jwt"));
						var handler = new JsonWebTokenHandler();
						var key = new RsaSecurityKey(_keyManager.rsaKey);
						string? token = handler.CreateToken(new SecurityTokenDescriptor()
						{
							Issuer = "https://localhost:7258",
							Subject = identity,
							Expires = DateTime.UtcNow.AddHours(1),
							SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
						});
						return (token, null); 
					}
				}

				return (null, "Invalid data");
			}
			catch (Exception ex)
			{
				return (null, $"Error:{ex}");
			}
		}
	}
}
