using System;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA_Encryptor encryptor = new RSA_Encryptor();
            int p = 61, q = 53;
            var keys = encryptor.GenerateKeys(p, q);
            int entry = 69;
            int enc = encryptor.Cipher(entry, keys.e, keys.n);
            int desenc = encryptor.Decipher(enc, keys.d, keys.n);

            if (desenc == entry)
            {
                Console.WriteLine("Descifrado!!");
            }
        }
    }
}
