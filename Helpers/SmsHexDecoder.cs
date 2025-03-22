using System.Text;

namespace RouterPlus.Converters;

public static class SmsHexDecoder
{
    public static string DecodeSmsHex(string hex)
    {
        byte[] bytes = ConvertHexStringToByteArray(hex);
        
        return Encoding.BigEndianUnicode.GetString(bytes);
    }
    

    private static byte[] ConvertHexStringToByteArray(string hex)
    {
        int length = hex.Length / 2;
        byte[] bytes = new byte[length];

        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }

        return bytes;
    }
}