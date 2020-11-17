using System;
using System.Diagnostics;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA_Encryptor encryptor = new RSA_Encryptor();
            int p = 61, q = 53; // p * q siempre mayor a 255| 61 y 53
            string name = "easy test.txt";
            string[] fileName = name.Split('.');
            var keys = encryptor.GenerateKeys(p, q);

            //RUTA JUANSE
            string filePath = @$"C:\Users\Usuario\Desktop\Archivos de Prueba\{name}";
            string pathEncryption = @$"C:\Users\Usuario\Desktop\Archivos de Prueba\Encryptions";
            string pathDecryption = @$"C:\Users\Usuario\Desktop\Archivos de Prueba\Decryptions";

            //RUTA SABAN
            //string filePath = @$"C:\Users\DELL\Desktop\Archivos de Prueba\{name}";
            //string pathEncryption = @$"C:\Users\DELL\Desktop\Archivos de Prueba\Encryptions";
            //string pathDecryption = @$"C:\Users\DELL\Desktop\Archivos de Prueba\Decryptions";


            var timer2 = new Stopwatch();
            timer2.Start();
            encryptor.Encrypt(filePath, fileName, pathEncryption, keys.e, keys.n);
            encryptor.Decrypt($@"{pathEncryption}\{fileName[0]}.rsa", fileName, pathDecryption, keys.d, keys.n);

            timer2.Stop();
            Console.WriteLine("TIME ELAPSED MILISECONDS: " + timer2.ElapsedMilliseconds);




            //var keys = encryptor.GenerateKeys(p, q);
            //int entry = 69;
            //int enc = encryptor.Cipher(entry, keys.e, keys.n);
            //int desenc = encryptor.Decipher(enc, keys.d, keys.n);

            //if (desenc == entry)
            //{
            //    Console.WriteLine("Descifrado!!");
            //}
        }
    }
}
