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
            Assert.Equal("public", methodGen.AccessModifier);
            Assert.Equal("string", methodGen.ReturnType);
            Assert.True(methodGen.Parameters.ContainsKey("initialText"));
            Assert.True(methodGen.Parameters.ContainsKey("amount"));
            Assert.Equal("string", methodGen.Parameters["initialText"]);
            Assert.Equal("int", methodGen.Parameters["amount"]);
            Assert.Equal(body, methodGen.Body);
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

            var generatedCode = methodGen
                .Private()
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
            Assert.False(methodGen.IsOverride);
            Assert.False(methodGen.IsStatic);
            Assert.Equal("GenerateText", methodGen.Name);
            Assert.Equal("private", methodGen.AccessModifier);
            Assert.Equal("string", methodGen.ReturnType);
            Assert.True(methodGen.Parameters.ContainsKey("amount"));
            Assert.Equal("int", methodGen.Parameters["amount"]);
            Assert.Equal(body, methodGen.Body);
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
            Assert.False(methodGen.IsOverride);
            Assert.False(methodGen.IsStatic);
            Assert.Equal("GenerateText", methodGen.Name);
            Assert.Equal("protected", methodGen.AccessModifier);
            Assert.Equal("string", methodGen.ReturnType);
            Assert.True(methodGen.Parameters.ContainsKey("amount"));
            Assert.Equal("int", methodGen.Parameters["amount"]);
            Assert.Equal(body, methodGen.Body);
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
            Assert.True(methodGen.IsOverride);
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

        [Fact]
        void TestReplaceParameter()
        {
            var methodGen = new MethodGen(name: "MethodTest");

            methodGen.WithParameter("object", "test");
            methodGen.WithParameter("string", "test");

            var generatedCode = methodGen.GenerateCode();

            var expectedCode = """"
                void MethodTest(string test)
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }
    }
}
