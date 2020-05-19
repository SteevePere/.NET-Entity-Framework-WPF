using System;
using System.Security.Cryptography;
using System.Text;


namespace Client.src.helpers
{

	public static class Hasher
	{
		public static string getHash(string passwordToHash)
		{
			string hash = "";

			if (passwordToHash is string && passwordToHash != "")
			{
				using (SHA1 sha1Hash = SHA1.Create())
				{
					byte[] sourceBytes = Encoding.UTF8.GetBytes(passwordToHash);
					byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
					hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

				}
			}

			return hash;
		}
	}

}
