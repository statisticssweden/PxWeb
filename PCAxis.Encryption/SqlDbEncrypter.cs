using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace PCAxis.Encryption
{
    /// <summary>
    /// Class for encrypting/decrypting connection elements in SqlDb.config files
    /// </summary>
    public class SqlDbEncrypter
    {
        /// <summary>
        /// This method will encrypt all connection elements in the specified SqlDb.config file
        /// </summary>
        /// <param name="filePath">Path to the SqlDb.config file</param>
        /// <returns>True if the file was encrypted successfully, else false</returns>
        public static bool Encrypt(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(filePath);
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                RijndaelManaged key = GetKey();
                // Encrypt all "DefaultUser" elements
                Encrypt(xmlDoc, "DefaultUser", key);
                // Encrypt all "DefaultPassword" elements
                Encrypt(xmlDoc, "DefaultPassword", key);
                xmlDoc.Save(filePath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Decrypt the specified SqlDb.config file
        /// </summary>
        /// <param name="filePath">Path to the SqlDb.config file</param>
        /// <returns>True if the file was decrypted successfully, else false</returns>
        public static bool Decrypt(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();
            RijndaelManaged key = GetKey();
            
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(filePath);
                Decrypt(xmlDoc, key);
                xmlDoc.Save(filePath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Decrypt the XmlDocument holding the SqlDb.config file
        /// </summary>
        /// <param name="Doc">The XmlDocument holding the SqlDb.config file</param>
        public static bool Decrypt(XmlDocument xmlDoc)
        {
            try
            {
                RijndaelManaged key = GetKey();
                Decrypt(xmlDoc, key);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Encrypt document
        /// </summary>
        /// <param name="Doc">XmlDocument to encrypt</param>
        /// <param name="ElementName">Encrypt all elements of this type</param>
        /// <param name="Key">Encryption key to use</param>
        private static void Encrypt(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementName == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Key == null)
                throw new ArgumentNullException("Alg");

            ////////////////////////////////////////////////
            // Find the specified element in the XmlDocument
            // object and create a new XmlElemnt object.
            ////////////////////////////////////////////////
            //XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;

            XmlNodeList lst = Doc.GetElementsByTagName(ElementName);

            for (int i = lst.Count - 1; i >= 0; i--)
            {
                XmlElement elementToEncrypt = (XmlElement)lst[i];

                // Throw an XmlException if the element was not found.
                if (elementToEncrypt == null)
                {
                    throw new XmlException("The specified element was not found");

                }

                //////////////////////////////////////////////////
                // Create a new instance of the EncryptedXml class 
                // and use it to encrypt the XmlElement with the 
                // symmetric key.
                //////////////////////////////////////////////////

                EncryptedXml eXml = new EncryptedXml();

                byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);
                ////////////////////////////////////////////////
                // Construct an EncryptedData object and populate
                // it with the desired encryption information.
                ////////////////////////////////////////////////

                EncryptedData edElement = new EncryptedData();
                edElement.Type = EncryptedXml.XmlEncElementUrl;

                // Create an EncryptionMethod element so that the 
                // receiver knows which algorithm to use for decryption.
                // Determine what kind of algorithm is being used and
                // supply the appropriate URL to the EncryptionMethod element.

                string encryptionMethod = null;

                if (Key is TripleDES)
                {
                    encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
                }
                else if (Key is DES)
                {
                    encryptionMethod = EncryptedXml.XmlEncDESUrl;
                }
                if (Key is Rijndael)
                {
                    switch (Key.KeySize)
                    {
                        case 128:
                            encryptionMethod = EncryptedXml.XmlEncAES128Url;
                            break;
                        case 192:
                            encryptionMethod = EncryptedXml.XmlEncAES192Url;
                            break;
                        case 256:
                            encryptionMethod = EncryptedXml.XmlEncAES256Url;
                            break;
                    }
                }
                else
                {
                    // Throw an exception if the transform is not in the previous categories
                    throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
                }

                edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

                // Add the encrypted element data to the 
                // EncryptedData object.
                edElement.CipherData.CipherValue = encryptedElement;

                ////////////////////////////////////////////////////
                // Replace the element from the original XmlDocument
                // object with the EncryptedData element.
                ////////////////////////////////////////////////////
                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
            }
        }

        /// <summary>
        /// Decrypt document
        /// </summary>
        /// <param name="Doc">XmlDocument to decrypt</param>
        /// <param name="Alg">Encryption key to use</param>
        private static void Decrypt(XmlDocument Doc, SymmetricAlgorithm Alg)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (Alg == null)
                throw new ArgumentNullException("Alg");

            // Find the EncryptedData element in the XmlDocument.
            //XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;
            XmlNodeList lst = Doc.GetElementsByTagName("EncryptedData");

            for (int i = lst.Count - 1; i >= 0; i--)
            {
                XmlElement encryptedElement = (XmlElement)lst[i];

                // If the EncryptedData element was not found, throw an exception.
                if (encryptedElement == null)
                {
                    throw new XmlException("The EncryptedData element was not found.");
                }


                // Create an EncryptedData object and populate it.
                EncryptedData edElement = new EncryptedData();
                edElement.LoadXml(encryptedElement);

                // Create a new EncryptedXml object.
                EncryptedXml exml = new EncryptedXml();


                // Decrypt the element using the symmetric key.
                byte[] rgbOutput = exml.DecryptData(edElement, Alg);

                // Replace the encryptedData element with the plaintext XML element.
                exml.ReplaceData(encryptedElement, rgbOutput);
            }

        }

        /// <summary>
        /// Get the encryption key
        /// </summary>
        /// <returns></returns>
        private static RijndaelManaged GetKey()
        {
            RijndaelManaged key = new RijndaelManaged();
            key.Key = new byte[32] { 118, 123, 23, 17, 161, 152, 35, 68, 126, 213, 16, 115, 68, 217, 58, 108, 56, 218, 5, 78, 28, 128, 113, 208, 61, 56, 10, 87, 187, 162, 233, 38 };
            key.IV = new byte[16] { 33, 241, 14, 16, 103, 18, 14, 248, 4, 54, 18, 5, 60, 76, 16, 191 };
            return key;
        }


    }
}
