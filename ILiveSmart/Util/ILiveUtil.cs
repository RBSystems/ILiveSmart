using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ILiveSmart
{
    public class ILiveUtil
    {
        public static byte[] AddByteToBytes(byte[] source, byte to)
        {
            List<byte> lTemp = new List<byte>();
            if (source!=null)
            {
                lTemp.AddRange(source);
            }
            lTemp.Add(to);
            byte[] result = new byte[lTemp.Count];
            lTemp.CopyTo(result);
            return result;

            
        }
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;

            if (bytes != null)
            {

                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {

                    strB.Append(bytes[i].ToString("X2"));

                }

                hexString = strB.ToString();

            } return hexString;

        }

    }
}