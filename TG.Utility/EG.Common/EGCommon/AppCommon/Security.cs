using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EG.Utility.AppCommon
{
    public class Security
    {
        private const string privatekey = "emperors";

        /// <summary>
        /// DES encryption。 --by edgar on 2013-9-10
        /// </summary>
        /// <param name="strToEncrypt"></param>
        /// <param name="sKey">the key length must equals 8</param>
        /// <returns> Base64 format</returns>
        public string Encrypt(string strToEncrypt, string sKey)
        {
            if (sKey.Length > 8)
            {
                throw new Exception("the key length must be less than or equal to 8!");
            }
            if (sKey.Length < 8)
            {
                string sKeyOther = "abcdefgh";
                sKey += sKeyOther.Substring(0, 8 - sKey.Length);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// DES decryption  --by edgar on 2013-9-10
        /// </summary>
        /// <param name="strToDecrypt"></param>
        /// <param name="sKey">the key's length must equals 8</param>
        /// <returns></returns>
        public string Decrypt(string strToDecrypt, string sKey)
        {
            if (sKey.Length < 8)
            {
                string sKeyOther = "abcdefgh";
                sKey += sKeyOther.Substring(0, 8 - sKey.Length);
            }

            byte[] inputByteArray = Convert.FromBase64String(strToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }


    }
}
