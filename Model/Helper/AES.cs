using System;
namespace GreenWhale.UpdateCore
{
    public class AES
    {
        public AES(byte[] iv)
        {

            this.iv = iv ?? throw new ArgumentNullException(nameof(iv));
            if (iv.Length != 16)
            {
                throw new ArgumentException("向量长度必须为16");
            }
        }
        byte[] iv;
        public byte[] AESEncrypt(byte[] bytes, byte[] key)
        {
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = key,
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return resultArray;
        }
        public byte[] AESDecrypt(byte[] securityTxt, byte[] key)
        {
            try
            {
                System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = key,
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                };

                System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(securityTxt, 0, securityTxt.Length);
                return resultArray;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
