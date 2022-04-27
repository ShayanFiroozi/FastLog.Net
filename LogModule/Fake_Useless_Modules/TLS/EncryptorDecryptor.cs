using System.Numerics;

namespace RSA
{
    internal class EncryptorDecryptor
    {
        public BigInteger c, m;

        public void Encrypt(BigInteger m, BigInteger e, BigInteger n)          //encryption
        {
            c = BigInteger.ModPow(m, e, n);
        }

        public void Decrypt(BigInteger cResult, BigInteger d, BigInteger n)    //decryption
        {
            m = BigInteger.ModPow(cResult, d, n);
        }
    }
}
