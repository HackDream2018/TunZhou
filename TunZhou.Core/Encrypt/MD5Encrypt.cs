using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TunZhou.Core.Encrypt
{
    public static class MD5Encrypt
    {
        /// <summary>
        /// 计算32位大写MD5值
        /// </summary>
        public static string CalculateMD5Value(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException($"{nameof(input)}", "计算MD5值的输入不允许为空");
            }
            // 加盐
            input += "homeinns20190807";
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var md5ValueBytes = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
            var strBuilder = new StringBuilder();
            foreach (byte b in md5ValueBytes)
            {
                strBuilder.Append(b.ToString("x2").ToUpper());
            }
            return strBuilder.ToString();
        }
    }
}
