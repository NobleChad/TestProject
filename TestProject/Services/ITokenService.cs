namespace TestProject.Services
{
	public interface ITokenService
	{
		Task<(string?,string?)> GenerateTokenAsync(string email, string password);
	}
}
