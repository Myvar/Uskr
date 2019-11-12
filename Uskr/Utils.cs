using System.IO;
using System.Reflection;
using System.Text;

namespace Uskr
{
    public class Utils
    {
        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
        
        public static string GetResourceFile(string file)
        {
            var assembly = typeof(Utils).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream("Uskr._build." + file))
            using (var textReader = new StreamReader(stream))
            {
                return textReader.ReadToEnd();
            }
        }
        
        public static string GetResourceFileNoPath(string file, Assembly assembly)
        {

            using (var stream = assembly.GetManifestResourceStream( file))
            using (var textReader = new StreamReader(stream))
            {
                return textReader.ReadToEnd();
            }
        }
    }
}