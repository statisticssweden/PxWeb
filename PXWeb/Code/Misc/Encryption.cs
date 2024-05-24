namespace PXWeb.Misc
{
    /// <summary>
    /// Class for encrypting passwords
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// MD5 encryption
        /// </summary>
        /// <param name="sPassword">Password to encrypt</param>
        /// <returns>String encrypted with the MD5 algorithm</returns>
        public static string md5(string sPassword)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }

        /// <summary>
        /// SHA1 encryption
        /// </summary>
        /// <param name="sPassword">Password to encrypt</param>
        /// <returns>String encrypted with the SHA1 algorithm</returns>
        public static string sha1(string sPassword)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
    }
}
