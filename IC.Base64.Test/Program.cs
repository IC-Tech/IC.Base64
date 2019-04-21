using System;
namespace IC.Base64Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicTest();
            BasicComparer();
            AdvancedComparer();
        }
        static void BasicTest()
        {
            Console.WriteLine("Decode Test 0 >> {0}", IC.Base64.ToString("VGhpcyBpcyB0aGUgYmFzaWMgdGVzdCAw"));
            Console.WriteLine("Encode Test 1 >> {0}", IC.Base64.StringTo("This is the basic test 1"));
        }
        static void BasicComparer()
        {
            int pos = 0;
            var str = "";
            int EncodeFails = 0;
            while (pos++ < 1000)
            {
                str += pos.ToString();
                var encode1 = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(str));
                var encode2 = IC.Base64.StringTo(str);
                if (encode1 != encode2) EncodeFails++;
                var decode1 = System.Text.Encoding.Default.GetString(Convert.FromBase64String(encode1));
                var decode2 = IC.Base64.ToString(encode2);
                if (decode1 != decode2) EncodeFails++;
            }

            if (EncodeFails == 0)
                Console.WriteLine("No deferent between The IC.Base64 and The System.");
            else
                Console.WriteLine("{0} deferent found between The IC.Base64 and The System.", EncodeFails);
        }
        static void AdvancedComparer()
        {
            int pos = 0;
            var str = "";
            int SystemEncodeFails = 0;
            int EncodeFails = 0;
            while (pos++ < 1000)
            {
                str += pos.ToString();
                var res = "";
                try
                {
                    res = System.Text.Encoding.Default.GetString(Convert.FromBase64String(str));
                }
                catch { }
                var b = IC.Base64.ToString(str);
                if (res != "" && res != b) EncodeFails++;
                else if (res == "") SystemEncodeFails++;
            }
            Console.WriteLine("{0} times System Encoder failed and {1} times IC.Base64 failed.", SystemEncodeFails, EncodeFails);
        }
    }
}
