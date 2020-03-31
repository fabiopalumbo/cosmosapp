using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BRComos.IO.Utilities
{
    internal class WorksetHack
    {
        internal WorksetHack(Plt.IComosDWorkset workset)
        {
            string clStr = workset.InitAOWM();
            workset.FinishAOWM(ProcessString(clStr));
        }

        private static string ProcessString(string clStr)
        {
            byte[] numArray1 = new byte[20]
            {
                40,0,107,0,57,0,121,0,61,0,90,0,118,0,80,0,45,0,52,0
            };

            byte[] bytes = Encoding.ASCII.GetBytes(numArray1.Length.ToString());
            byte[] numArray2 = new byte[8];
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            string empty = string.Empty;
            try
            {
                byte[] rgbKey = new PasswordDeriveBytes(numArray1, bytes).CryptDeriveKey("TripleDES", "MD5", 168, numArray2);
                byte[] buffer = Convert.FromBase64String(clStr);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateDecryptor(rgbKey, numArray2), CryptoStreamMode.Write);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                memoryStream.Position = 0L;
                byte[] array = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                empty = Encoding.Unicode.GetString(array);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ex:" + ex.Message);
            }
            finally
            {
                ChangeBuffer(numArray1);
                ChangeBuffer(bytes);
                ChangeBuffer(numArray2);
            }
            return empty;
        }

        public static void ChangeBuffer(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentException(nameof(buffer));
            for (int index = 0; index < buffer.Length; ++index)
                buffer[index] = (byte)0;
        }
    }

}

