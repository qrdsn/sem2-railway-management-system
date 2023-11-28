using System.Security.Cryptography;
using System.Text;

namespace Logic
{
    public class Functions
    {
        /// <summary>
        /// gets a randomly generated x character string of numbers and letters
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueKey(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }


        /// <summary>
        /// writes email and key to .txt file in format of link (ish)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task WriteToTxt(string email, string key)
        {
            string registerLink = $"/User/Register/?email={email}&key={key}";

            using StreamWriter file = new("wwwroot/keys.txt", append: true);
            await file.WriteLineAsync(registerLink);
        }
    }
}
