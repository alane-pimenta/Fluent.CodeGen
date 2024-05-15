namespace Fluent.CodeGen.Tests
{
    public class MethodGenTests
    {
        [Fact]
        void TestStaticMethod()
        {
            var body = """"
            string finalText = initialText;
            for(int i = 0; i < amount; i++)
            {
                string numberText = "the number is" + i;
                finalText = finalText + numberText + ", ";
            }
            return finalText;
            """";

            var methodGen = new MethodGen(name: "GenerateText");

            var generatedCode = methodGen.Public()
                .Static()
                .WithReturnType("string")
                .WithParameter("string", "initialText")
                .WithParameter("int", "amount")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                public static string GenerateText(string initialText, int amount)
                {
                    string finalText = initialText;
                    for(int i = 0; i < amount; i++)
                    {
                        string numberText = "the number is" + i;
                        finalText = finalText + numberText + ", ";
                    }
                    return finalText;
                }
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.False(methodGen.IsOverride);
            Assert.True(methodGen.IsStatic);
            Assert.Equal("GenerateText", methodGen.Name);
        }


        [Fact]
        void TestPrivateMethod()
        {
            var body = """"
            string finalText = "";
            for(int i = 0; i < amount; i++)
            {
                string numberText = "the number is" + i;
                finalText = finalText + numberText + ", ";
            }
            return finalText;
            """";

            var methodGen = new MethodGen(name: "GenerateText");

            var generatedCode = methodGen.Private()
                .WithReturnType("string")
                .WithParameter("int", "amount")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                private string GenerateText(int amount)
                {
                    string finalText = "";
                    for(int i = 0; i < amount; i++)
                    {
                        string numberText = "the number is" + i;
                        finalText = finalText + numberText + ", ";
                    }
                    return finalText;
                }
                """";
            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        void TestProtectedMethod()
        {
            var body = """"
            string finalText = "";
            for(int i = 0; i < amount; i++)
            {
                string numberText = "the number is" + i;
                finalText = finalText + numberText + ", ";
            }
            return finalText;
            """";

            var methodGen = new MethodGen(name: "GenerateText");

            var generatedCode = methodGen.Protected()
                .WithReturnType("string")
                .WithParameter("int", "amount")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                protected string GenerateText(int amount)
                {
                    string finalText = "";
                    for(int i = 0; i < amount; i++)
                    {
                        string numberText = "the number is" + i;
                        finalText = finalText + numberText + ", ";
                    }
                    return finalText;
                }
                """";
            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        void TestOverrideMethod()
        {
            var body = """"
            string finalText = "";
            for(int i = 0; i < amount; i++)
            {
                string numberText = "the number is" + i;
                finalText = finalText + numberText + ", ";
            }
            return finalText;
            """";

            var methodGen = new MethodGen(name: "GenerateText");

            var generatedCode = methodGen
                .Override()
                .WithReturnType("string")
                .WithParameter("int", "amount")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                override string GenerateText(int amount)
                {
                    string finalText = "";
                    for(int i = 0; i < amount; i++)
                    {
                        string numberText = "the number is" + i;
                        finalText = finalText + numberText + ", ";
                    }
                    return finalText;
                }
                """";
            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        void TestPublicOverrideMethod()
        {
            var body = """"
            string finalText = "";
            for(int i = 0; i < amount; i++)
            {
                string numberText = "the number is" + i;
                finalText = finalText + numberText + ", ";
            }
            return finalText;
            """";

            var methodGen = new MethodGen(name: "GenerateText");

            var generatedCode = methodGen.Public()
                .Override()
                .WithReturnType("string")
                .WithParameter("int", "amount")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                public override string GenerateText(int amount)
                {
                    string finalText = "";
                    for(int i = 0; i < amount; i++)
                    {
                        string numberText = "the number is" + i;
                        finalText = finalText + numberText + ", ";
                    }
                    return finalText;
                }
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.True(methodGen.IsOverride);
            Assert.False(methodGen.IsStatic);
        }

    }
}
