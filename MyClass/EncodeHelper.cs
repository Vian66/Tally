using System;
using System.Security.Cryptography;
using System.Text;

namespace Tally.MyClass {
    public class EncodeHelper {
        private String APIKey1 = "BCBC85DF098A4E+91!8?A4C!@#15E4]01F46A4F";
        private String APIKey2 = "#!@#F447DDFBC848_)4C26@#B046&B2153BD677B8";

        /// <summary>
        /// AES加密,使用預設的Key
        /// </summary>
        /// <param name="Data">要加密的字串</param>
        /// <returns></returns>
        public string EnryptString (String Data) {
            String Result = enprypt (Data, APIKey1, APIKey2);
            return Result;
        }

        /// <summary>
        /// AES解密,使用預設的Key
        /// </summary>
        /// <param name="Data">要解密的字串</param>
        /// <returns></returns>
        public string DecryptString (String Data) {
            String Result = decrypt (Data, APIKey1, APIKey2);
            return Result;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">要加密的字串</param>
        /// <param name="key">金鑰1</param>
        /// <param name="key2">金鑰2</param>
        /// <returns></returns>
        private string enprypt (string plainText, string key, string key2) {
            String Result = "";
            try {
                var AES = new RijndaelManaged ();
                var MD5 = new MD5CryptoServiceProvider ();
                byte[] plainTextData = Encoding.Unicode.GetBytes (plainText);
                byte[] keyData = MD5.ComputeHash (Encoding.Unicode.GetBytes (key));
                byte[] IVData = MD5.ComputeHash (Encoding.Unicode.GetBytes (key2));
                ICryptoTransform transform = AES.CreateEncryptor (keyData, IVData);
                byte[] outputData = transform.TransformFinalBlock (plainTextData, 0, plainTextData.Length);
                Result = Convert.ToBase64String (outputData);
            } catch { }
            return Result;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherTextString">要解密的字串</param>
        /// <param name="key">金鑰1</param>
        /// <param name="key2">金鑰2</param>
        /// <returns></returns>
        private string decrypt (String cipherTextString, string key, string key2) {
            String Result = "";
            try {
                byte[] cipherTextData = Convert.FromBase64String (cipherTextString);
                var AES = new RijndaelManaged ();
                var MD5 = new MD5CryptoServiceProvider ();
                byte[] keyData = MD5.ComputeHash (Encoding.Unicode.GetBytes (key));
                byte[] IVData = MD5.ComputeHash (Encoding.Unicode.GetBytes (key2)); //可以放入其它值
                ICryptoTransform transform = AES.CreateDecryptor (keyData, IVData);
                byte[] outputData = transform.TransformFinalBlock (cipherTextData, 0, cipherTextData.Length);
                Result = Encoding.Unicode.GetString (outputData);
            } catch { }
            return Result;
        }
    }
}