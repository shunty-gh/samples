<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	/*
	This sample was created in response to this StackOverflow question: http://stackoverflow.com/q/34306804
	There is a matching PHP sample and the question is due to the padding that PHP does automatically.
	*/
	
	//string src1 = "{\"key\":\"value\"}";
	string src1 = "Hello world!";
	string pwd = "masterKeyOfLength29Characters";

	var iv = new byte[] { 72, 163, 99, 62, 219, 111, 163, 114, 15, 47, 65, 99, 231, 108, 110, 87, 72, 163, 99, 62, 219, 111, 163, 114, 15, 47, 65, 99, 231, 108, 110, 87 };

	string enc1 = CryptUtils.EncryptRijndael(src1, pwd, iv);

	"-- Encrypted -- ".Dump();
	enc1.Dump();
	Console.WriteLine("");
	Console.WriteLine("");

	// Decrypt
	var cipherText = Convert.FromBase64String(enc1);

	string d1;
	d1 = CryptUtils.DecryptRijndael(cipherText, pwd, iv);
	
	"-- Decrypted -- ".Dump();
	d1.Dump();
}

public static class CryptUtils
{
	public static string DecryptRijndael(byte[] cipherText, string password, byte[] iv)
	{
		var key = new byte[32];
		Encoding.UTF8.GetBytes(password).CopyTo(key, 0);

		var cipher = new RijndaelManaged();
		cipher.Mode = CipherMode.CBC;
		cipher.Padding = PaddingMode.None;
		cipher.KeySize = 256;
		cipher.BlockSize = 256;
		cipher.Key = key;
		cipher.IV = iv;

		byte[] plain;
		using (var decryptor = cipher.CreateDecryptor())
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
				{
					cs.Write(cipherText, 0, cipherText.Length);
					cs.FlushFinalBlock();
					plain = ms.ToArray();
				}
			}
		}
		return Encoding.UTF8.GetString(plain);
	}

	public static string EncryptRijndael(string source, string password, byte[] iv)
	{
		//var keyhash = (new SHA256Managed()).ComputeHash(Encoding.UTF8.GetBytes(password));
		//var keyhex = string.Join("", keyhash.Select(b => b.ToString("x2")).ToArray());
		//Convert.ToBase64String(keyhash).Dump("Key in B64");

		// Short passwords need to be filled/padded to make them the right size. PHP 
		// automatically adds zero byte padding.
		var key = new byte[32];
		Encoding.UTF8.GetBytes(password).CopyTo(key, 0);
		
		var cipher = new RijndaelManaged();
		cipher.Mode = CipherMode.CBC;
		cipher.Padding = PaddingMode.Zeros;
		cipher.KeySize = 256;
		cipher.BlockSize = 256;
		cipher.Key = key;
		cipher.IV = iv;

		byte[] ciphertext;
		byte[] src = Encoding.UTF8.GetBytes(source);
		using (var encryptor = cipher.CreateEncryptor())
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
				{
					cs.Write(src, 0, src.Length);
					cs.FlushFinalBlock();
					ciphertext = ms.ToArray();
				}
			}
		}

		var result = Convert.ToBase64String(ciphertext);

		// The SO question did this originally:
		//	var result = Convert.ToBase64String(cipher.IV) + Convert.ToBase64String(ciphertext);
		// But what he should have done is this:	
		//  var resultarr = new byte[cipher.IV.Length + ciphertext.Length];
		//	Buffer.BlockCopy(cipher.IV, 0, resultarr, 0, cipher.IV.Length);
		//	Buffer.BlockCopy(ciphertext, 0, resultarr, cipher.IV.Length, ciphertext.Length);
		//	var result = Convert.ToBase64String(resultarr);
		
		return result;
	}

}