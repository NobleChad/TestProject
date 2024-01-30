using System.Security.Cryptography;

namespace TestProject.Services
{
	public class KeyManager
	{
		public KeyManager()
		{
			rsaKey = RSA.Create();
			if (File.Exists("key"))
			{
				rsaKey.ImportRSAPrivateKey(File.ReadAllBytes("key"), out _);
			}
			else
			{
				var privateKey = rsaKey.ExportRSAPrivateKey();
				File.WriteAllBytes("key", privateKey);
			}
		}

		public RSA rsaKey { get; }
	}

}
