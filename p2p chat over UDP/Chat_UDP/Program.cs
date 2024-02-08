using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat_UDP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }
        
        
        
        

        public static object Params2Object(Socket _socket, EndPoint Remote)
        {
            ArrayList objects = new ArrayList();
            objects.Add(_socket);
            objects.Add(Remote);
            return objects;
        }

        public static void Object2Params(object parameters, ref Socket aSocket, ref EndPoint Remote)
        {
            ArrayList objects = (ArrayList)parameters;
            aSocket = (Socket)objects[0];
            Remote = (EndPoint)objects[1];
        }

        
        
        
        
        public static byte[] Encrypt_Sym(string plainText, string key)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB; // You might want to use a more secure mode like CBC
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return msEncrypt.ToArray();
                }
            }
        }
        public static string Decrypt_Sym(byte[] cipherText, string key)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.Mode = CipherMode.ECB; // You might want to use a more secure mode like CBC
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        
        
        
        
        public static byte[] Encrypt_ASym(string plainText, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                return rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
            }
        }
        public static string Decrypt_ASym(byte[] cipherText, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(cipherText, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

    }

    
    
    
    
    class Semaphore
    {
        public int count;

        public Semaphore()
        {
            count = 0;
        }
        public Semaphore(int InitialVal)
        {
            count = InitialVal;
        }
        public void Wait()
        {
            lock (this)
            {
                count--;
                if (count < 0)
                    Monitor.Wait(this, Timeout.Infinite);
            }
        }
        public void Signal()
        {
            lock (this)
            {
                count++;
                if (count <= 0)
                    Monitor.Pulse(this);
            }
        }
    }
    
    
    
}
