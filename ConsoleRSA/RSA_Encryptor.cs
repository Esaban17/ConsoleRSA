﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.IO.Compression;

namespace ConsoleRSA
{
    class RSA_Encryptor
    {
        public static int buffer = 100000;
      
        List<int> byteOutputList = new List<int>();
        public (int e, int d, int n) GenerateKeys(int p, int q)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);
            int e = GenerateE(phi);
            int d = GenerateD(phi, phi, e, 1, phi);
           
            return (e, d, n);
        }
        public void Encrypt(string filePath, string[] fileName, string pathEncryption, int key, int n)
        {
            using (var Fs = new FileStream(filePath, FileMode.Open))
            {
                using (var Br = new BinaryReader(Fs))
                {
                    using (var Fs2 = new FileStream($"{pathEncryption}/{fileName[0]}.rsa", FileMode.OpenOrCreate))
                    {
                        using (var Bw = new BinaryWriter(Fs2))
                        {
                            var bytes = new byte[buffer];
                            while (Br.BaseStream.Position != Br.BaseStream.Length)
                            {
                                bytes = Br.ReadBytes(buffer);
                                foreach (var item in bytes)
                                {
                                    int newVal = Cipher(item, key, n);
                                    byteOutputList.Add(newVal);
                                }
                            }
                            int maxBitSize = Convert.ToString(n, 2).Length;
                            string tempCode = "";
                            //Bw.Write(Convert.ToByte(maxBitSize));

                            while (byteOutputList.Count > 0)
                            {
                                tempCode += Convert.ToString(byteOutputList[0], 2).PadLeft(maxBitSize, '0');
                                byteOutputList.RemoveAt(0);
                                if (byteOutputList.Count == 0)
                                {
                                    while (tempCode.Length >= 8)
                                    {
                                        var decimalNumber = Convert.ToInt32(tempCode.Substring(0, 8), 2);
                                        var character = Convert.ToByte(decimalNumber);
                                        Bw.Write(character);
                                        tempCode = tempCode.Remove(0, 8);
                                    }
                                    if (tempCode.Length > 0)
                                    {
                                        tempCode = tempCode.PadRight(8, '0');
                                        var decimalNumber = Convert.ToInt32(tempCode.Substring(0, 8), 2);
                                        var character = Convert.ToByte(decimalNumber);
                                        Bw.Write(character);
                                        tempCode = tempCode.Remove(0, 8);
                                    }
                                }
                                if (tempCode.Length >= 8)
                                {
                                    while (tempCode.Length >= 8)
                                    {
                                        var decimalNumber = Convert.ToInt32(tempCode.Substring(0, 8), 2);
                                        var character = Convert.ToByte(decimalNumber);
                                        Bw.Write(character);
                                        tempCode = tempCode.Remove(0, 8);
                                    }
                                }
                            }
                            Bw.Close();
                        };
                        Fs2.Close();
                    };
                    Br.Close();
                };
                Fs.Close();
            };
        }
        public void Decrypt(string pathEncryption, string[] fileName, string pathDecrypt, int key, int n)
        {
            using (var Fs = new FileStream(pathEncryption, FileMode.Open))
            {
                using (var Br = new BinaryReader(Fs))
                {
                    using (var Fs2 = new FileStream($"{pathDecrypt}/{fileName[0]}.txt", FileMode.OpenOrCreate))
                    {
                        using (var Bw = new BinaryWriter(Fs2))
                        {
                            List<int> outputValsList = new List<int>();
                            //var bytes = new byte[1];
                            string tempVal = "";
                            //bytes = Br.ReadBytes(1);
                            //int maxBitSize = Convert.ToInt32(bytes[0]);
                            int maxBitSize = Convert.ToString(n, 2).Length;
                            while (Br.BaseStream.Position != Br.BaseStream.Length)
                            {
                               var bytes = new byte[buffer];
                                bytes = Br.ReadBytes(buffer);
                                foreach (var item in bytes)
                                {
                                    tempVal += Convert.ToString(item, 2).PadLeft(8, '0');
                                    if (tempVal.Length >= maxBitSize)
                                    {
                                        do
                                        {
                                            var val = Convert.ToInt32(tempVal.Substring(0, maxBitSize), 2);
                                            outputValsList.Add(val);
                                            tempVal = tempVal.Remove(0, maxBitSize);
                                        } while (tempVal.Length >= maxBitSize);
                                    }
                                }
                            }
                            while (outputValsList.Count > 0)
                            {
                                Bw.Write(Convert.ToByte(Cipher(outputValsList[0], key, n)));
                                outputValsList.RemoveAt(0);
                            }


                            Bw.Close();
                        };
                        Fs2.Close();
                    };
                    Br.Close();
                };
                Fs.Close();
            };
        }
        private static IEnumerable<int> BruteForcePrimes(int max)
        {
            if (max < 2) yield break;
            yield return 2;

            List<int> found = new List<int>();

            found.Add(3);
            int candidate = 3;

            while (candidate <= max)
            {
                bool isPrime = true;
                foreach (int prime in found)
                {
                    if (prime * prime > candidate)
                    {
                        break;
                    }
                    if (candidate % prime == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    found.Add(candidate);
                    yield return candidate;
                }
                candidate += 2;
            }
        }
        private bool MCD(int a, int b)
        {
            int mcd = 0;
            while (b != 0)
            {
                mcd = b;
                b = a % b;
                a = mcd;
            }
            if (mcd == 1)
            {
                return true;
            }
            return false;
        }
        private int GenerateE(int phi)
        {
            foreach (var item in BruteForcePrimes(phi))
            {
                if (MCD(phi, item))
                {
                    return item;
                }
            }
            return 0;
        }
        private int GenerateD(int phi1, int phi2, int e, int d, int phi)
        {
            int division = phi1 / e;
            int result1 = phi1 - (e * division);
            int result2 = phi2 - d * division;
            if (result2 < 0)
            {
                do
                {
                    result2 += phi;
                } while (result2<0);
            }
            if (result1 == 1)
            {
                return result2;
            }
            return GenerateD(e, d, result1, result2, phi);
        }
        
        public int Cipher(int entry, int key, int n)
        {
            BigInteger x = BigInteger.Pow(entry, key);
            BigInteger BigN = new BigInteger(n);
            return (int)(x % BigN);
        }
        //public int Decipher(int entrada, int d, int n)
        //{
        //    BigInteger x = BigInteger.Pow(entrada, d);
        //    var BigN = new BigInteger(n);
        //    return (int)(x % BigN);
        //}
    }
}
