using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleRSA
{
    class RSA_Encryptor
    {
        public (int e, int d, int n) GenerateKeys(int p, int q)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);
            int e = GenerateE(phi);
            int d = GenerateD(phi, phi, e, 1, phi);
            return (e, d, n);
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
                result2 = result2 + phi;
            }
            if (result1 == 1)
            {
                return result2;
            }
            return GenerateD(e, d, result1, result2, phi);
        }
        public int Cipher(int entrada, int e, int n)
        {
            BigInteger x = BigInteger.Pow(entrada, e);
            BigInteger BigN = new BigInteger(n);
            return (int)(x % BigN);
        }
        public int Decipher(int entrada, int d, int n)
        {
            BigInteger x = BigInteger.Pow(entrada, d);
            var BigN = new BigInteger(n);
            return (int)(x % BigN);
        }
    }
}
