using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Manually added
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;

namespace PubEnt.GlobalUtils
{
    //Uses RijndaelManaged Cryptographic Class
    public class Cryptography
    {
        public Cryptography()
        {

        }

        #region Cryptographic Methods
        //Reference: The Code Project article "Simple encrypting and decrypting data in C#" by DotNetThis
        //http://www.codeproject.com/KB/security/DotNetCrypto.aspx

        // Encrypt a byte array into a byte array using a key and an IV 
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = null;
            Rijndael alg = null;
            byte[] encryptedData = null;
            CryptoStream cs = null;

            try
            {
                // Create a MemoryStream to accept the encrypted bytes 
                ms = new MemoryStream();

                // Create a symmetric algorithm. 
                alg = Rijndael.Create();

                // Now set the key and the IV. 
                alg.Key = Key;
                alg.IV = IV;

                // Create a CryptoStream and write to the memory stream
                cs = new CryptoStream(ms,
                   alg.CreateEncryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the encryption 
                cs.Write(clearData, 0, clearData.Length);

                // Close the crypto stream
                cs.Close();
                

                // Now get the encrypted data from the MemoryStream.
                encryptedData = ms.ToArray();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally 
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }

                if (alg != null)
                    alg.Clear();

                if (cs != null)
                {
                    cs.Close();
                    cs.Dispose();
                }

            }

            return encryptedData;
        }         
        
        // Encrypt a string into a string using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static string Encrypt(string clearText)
        {
            string Password = ConfigurationManager.AppSettings["PubEntEncryptKey"];
            
            // Turn the input string into a byte array. 
            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);

            //Turn the password into Key and IV
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Get the key/IV and do the encryption
            byte[] encryptedData = Encrypt(clearBytes,
                  pdb.GetBytes(32), pdb.GetBytes(16));

            // Turn the resulting byte array into a string.
            return Convert.ToBase64String(encryptedData);

        }

        // Decrypt a byte array into a byte array using a key and an IV 
        public static byte[] Decrypt(byte[] cipherData,
                                    byte[] Key, byte[] IV)
        {
            MemoryStream ms = null;
            Rijndael alg = null;
            byte[] decryptedData = null;
            CryptoStream cs = null;

            try
            {
                // Create a MemoryStream to accept the decrypted bytes 
                ms = new MemoryStream();

                // Create a symmetric algorithm. 
                alg = Rijndael.Create();

                // Now set the key and the IV.
                alg.Key = Key;
                alg.IV = IV;

                // Create a CryptoStream and write to the memory stream
                cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);

                // Close the crypto stream
                cs.Close();

                // Now get the decrypted data from the MemoryStream.
                decryptedData = ms.ToArray();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }

                if (alg != null)
                    alg.Clear();

                if (cs != null)
                {
                    cs.Close();
                    cs.Dispose();
                }
            }

            return decryptedData;
        }

        // Decrypt a string into a string using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static string Decrypt(string cipherText)
        {
            string Password = ConfigurationManager.AppSettings["PubEntEncryptKey"];

            // Turn the input string into a byte array.
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Turn the password into Key and IV
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Get the key/IV and do the decryption
            byte[] decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));

            // Turn the resulting byte array into a string.
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }
        #endregion

    }
}
