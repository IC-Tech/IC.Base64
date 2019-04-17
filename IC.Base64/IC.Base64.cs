/*
|| @file IC.Base64.cs
|| @version 1.0
|| @author Imesh Chamara
|| @email imesh1chamara@gmail.com, ic.imesh.chamara@gmail.com
|| @copyright Copyright © Imesh Chamara 2019
||
|| @description
|| | IC.Base64 for lossless base64 Decode and Encode
|| #
||
|| @license
|| | This library is free software; you can redistribute it and/or
|| | modify it under the terms of the GNU Lesser General Public
|| | License as published by the Free Software Foundation; version
|| | 2.1 of the License.
|| |
|| | This library is distributed in the hope that it will be useful,
|| | but WITHOUT ANY WARRANTY; without even the implied warranty of
|| | MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
|| | Lesser General Public License for more details.
|| |
|| | You should have received a copy of the GNU Lesser General Public
|| | License along with this library; if not, write to the Free Software
|| | Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
|| #
||
*/
namespace IC
{
    public class Base64
    {
        private const string Table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public static bool UsePaddingCharacters = true;
        public static string To(byte[] data)
        {
            int pos0 = -1, pad = 0;
            var ret = "";
            while (data.Length > (pos0++ + 1))
            {
                byte v = (byte)(data[pos0] >> 2);
                ret += Table.Substring(v, 1);
                v = (byte)(((data[pos0] << 4) | ((pos0++ == (data.Length - 1) ? 0 : data[pos0]) >> 4)) & 0x3f);
                ret += Table.Substring(v, 1);
                if (pos0 == data.Length)
                {
                    pad = 2;
                    break;
                }
                v = (byte)(((data[pos0] << 2) | ((pos0++ == (data.Length - 1) ? 0 : data[pos0]) >> 6)) & 0x3f);
                ret += Table.Substring(v, 1);
                if (pos0 == data.Length)
                {
                    pad = 1;
                    break;
                }
                v = (byte)(data[pos0] & 0x3f);
                ret += Table.Substring(v, 1);
            }
            if (pad > 0 && UsePaddingCharacters)
                while (pad-- > 0)
                    ret += "=";
            return ret;
        }
        public static byte[] From(string data)
        {
            while (data.EndsWith("=")) data = data.Remove(data.Length - 1);
            var ca = data.ToCharArray();
            int pos0 = data.Length / 4 * 3, pos1 = pos0 / 3 * 4;
            bool rem = false;
            if(pos1 != ca.Length)
            {
                pos0 += ca.Length - (data.Length / 4 * 4);
                rem = true;
            }
            var ret = new byte[pos0];
            pos0 = -1;
            pos1 = 0;
            while (ca.Length > (pos0++ + 1))
            {
                ret[pos1++] = (byte)((Table.IndexOf(ca[pos0++]) << 2) | (Table.IndexOf(pos0 == ca.Length ? 'A' : ca[pos0]) >> 4));
                if (pos1 == ret.Length) break;
                ret[pos1++] = (byte)((Table.IndexOf(ca[pos0++]) << 4) | (Table.IndexOf(pos0 == ca.Length ? 'A' : ca[pos0]) >> 2));
                if (pos1 == ret.Length) break;
                ret[pos1++] = (byte)((Table.IndexOf(ca[pos0++]) << 6) | Table.IndexOf(pos0 == ca.Length ? 'A' : ca[pos0]));
            }
            if (rem && ret[pos1 - 1] == 0)
            {
                var tmp = new byte[ret.Length - 1];
                for (int i = 0; i < tmp.Length; i++) tmp[i] = ret[i];
                ret = tmp;
                tmp = null;
            }
            return ret;
        }
        public static string ToSting(string str) => System.Text.Encoding.Default.GetString(From(str));
        public static string StingTo(string str) => To(System.Text.Encoding.Default.GetBytes(str));
    }
}