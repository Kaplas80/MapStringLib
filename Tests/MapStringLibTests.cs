using System;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestHalfWidthToFullWidth()
        {
            var input = "Cadena de prueba";

            var sb = new StringBuilder(256);
            LCMapString(0x0800, 0x00800000, input, -1, sb, sb.Capacity);
            var invokeResult = sb.ToString();

            var libResult = MapStringLib.Convert.ToFullWidth(input);
            Assert.AreEqual(invokeResult, libResult);
        }

        [Test]
        public void TestFullWidthToHalfWidth()
        {
            var input = "Ｃａｄｅｎａ　ｄｅ　ｐｒｕｅｂａ";

            var sb = new StringBuilder(256);
            LCMapString(0x0800, 0x00400000, input, -1, sb, sb.Capacity);
            var invokeResult = sb.ToString();

            var libResult = MapStringLib.Convert.ToHalfWidth(input);
            Assert.AreEqual(invokeResult, libResult);
        }

        [Test]
        public void TestFullWidthToHalfWidthWithRandomString()
        {
            var input = GenerateRandomString(10000);

            var sb = new StringBuilder(20000);
            LCMapString(0x0800, 0x00400000, input, -1, sb, sb.Capacity);
            var invokeResult = sb.ToString();

            var libResult = MapStringLib.Convert.ToHalfWidth(input);

            Assert.AreEqual(invokeResult, libResult);
        }

        [Test]
        public void TestHalfWidthToFullWidthWithRandomString()
        {
            var input = GenerateRandomString(10000);

            var sb = new StringBuilder(20000);
            LCMapString(0x0800, 0x00800000, input, -1, sb, sb.Capacity);
            var invokeResult = sb.ToString();

            var libResult = MapStringLib.Convert.ToFullWidth(input);

            Assert.AreEqual(invokeResult, libResult);
        }

        [Test]
        public void Custom()
        {
            var input = "메ﾬ";

            var sb = new StringBuilder(10000);
            LCMapString(0x0800, 0x00800000, input, -1, sb, sb.Capacity);
            var invokeResult = sb.ToString();

            var libResult = MapStringLib.Convert.ToFullWidth(input);

            Assert.AreEqual(invokeResult, libResult);
        }

        private static string GenerateRandomString(int length)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var code = rnd.Next(0x0001, 0xFFFF); // No genero 0x00 porque mi librería lo soporta, pero la dll corta la línea
                if (code < 0xd800 || code > 0xdfff)
                {
                    var str = char.ConvertFromUtf32(code);
                    sb.Append(str);
                }
            }

            return sb.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int LCMapString(uint locale, uint dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest);

    }
}