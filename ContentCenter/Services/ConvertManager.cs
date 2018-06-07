using System;
using System.Text;

namespace ContentCenter.Services
{
    public class ConvertManager : IConvertManager
    {
        public string DecodePasswordStructure(byte[] bytes_Input, ref int KeySize, ref byte[] KeyData)
        {
            var Base = Convert.ToInt16(bytes_Input[0]);
            KeySize = Base;
            KeyData = new byte[Base];
            for (var a = 1; a <= Base; a++)
            {
                KeyData[a - 1] = bytes_Input[a];
            }

            var i = Base + 1;
            var j = 1;
            var MaxI = bytes_Input.Length;

            byte[] BytesResult = new byte[MaxI - Base];

            while (i < MaxI)
            {
                if (j > Base)
                {
                    j = 1;
                }

                var AA = Convert.ToInt16(bytes_Input[i]);
                var BB = Convert.ToInt16(bytes_Input[j]);
                int CC = AA ^ BB; // 239 for first

                BytesResult[i - Base - 1] = byte.Parse(CC.ToString());

                i = i + 1;
                j = j + 1;

            }

            return Encoding.UTF8.GetString(BytesResult);
        }
    }
}
