<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

string TextToBeDecrypted = "cJ4ZJD3Vkf3Dv5uxrWiTQg==";
string Password = "123";

RijndaelManaged RijndaelCipher = new RijndaelManaged(); 
string DecryptedData; 
byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted); 
byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString()); 
//Making of the key for decryption 
PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt); 
//Creates a symmetric Rijndael decryptor object. 
byte[] k1 = SecretKey.GetBytes(32);
byte[] iv = SecretKey.GetBytes(16);
ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(k1, iv); 
byte[] plainText = Decryptor.TransformFinalBlock(EncryptedData, 0, EncryptedData.Length); 

//Converting to string 
DecryptedData = Encoding.Unicode.GetString(plainText); 

//EncryptedData.Dump("Enc");
//Salt.Dump("Salt");
//SecretKey.Dump("Key");
//k1.Dump("K1");
Console.Write("Key: ");
foreach (byte b in k1)
{
	Console.Write(string.Format("{0:x2} ", b));
}
Console.WriteLine();
//iv.Dump("IV");
Console.Write("Key: ");
foreach (byte b in iv)
{
	Console.Write(string.Format("{0:x2} ", b));
}
Console.WriteLine();
DecryptedData.Dump();