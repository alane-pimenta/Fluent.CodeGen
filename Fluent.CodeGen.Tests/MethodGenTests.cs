using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent.CodeGen.Tests
{
    public class MethodGenTests
    {
        [Fact]
        void TestBasicMethod()
        {
            var body = @"
        string textoFinal = """";
        for(int i = 0; i < 10; i++)
        {
            string a = ""the number is"" + i;
            textoFinal = a + "", "";
        }
        return textoFinal;
    ".Trim();

            var methodGen = new MethodGen(type: "string", name: "GerarString");

            var generatedCode = methodGen.Public()
                .Static()
                .WithParameter("int", "bananas")
                .WithParameter("int", "bananas2")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = @"
        public static string GerarString(int bananas, int bananas2)
        {
            string textoFinal = """";
            for(int i = 0; i < 10; i++)
            {
                string a = ""the number is"" + i;
                textoFinal = a + "", "";
            }
            return textoFinal;
        }
    ".Trim();

            var expectedLines = expectedCode.Split('\n').Select(line => line.Trim());
            var generatedLines = generatedCode.Split('\n').Select(line => line.Trim());

            Assert.Equal(expectedLines, generatedLines);
        }
    }
}
