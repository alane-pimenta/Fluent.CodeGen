namespace Fluent.CodeGen.Tests
{
    public class ConstructorGenTests
    {

        [Fact]
        void TestSimpleConstructor()
        {

            var constructorGen = new ConstructorGen(className: "TestClass");

            var generatedCode = constructorGen.Public()
                .GenerateCode();

            var expectedCode = """"
                public TestClass()
                {
                }

                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("public", constructorGen.AccessModifier);
            Assert.Empty(constructorGen.Base);
            Assert.Empty(constructorGen.Parameters);
            Assert.Empty(constructorGen.Body);
        }

        [Fact]
        void TestConstructorWithParameter()
        {

            var constructorGen = new ConstructorGen(className: "TestClass");

            var generatedCode = constructorGen.Internal()
                .WithParameter("int", "numberOfTests")
                .WithParameter("string", "testText")
                .GenerateCode();

            var expectedCode = """"
                internal TestClass(int numberOfTests, string testText)
                {
                }
                
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal("internal", constructorGen.AccessModifier);
            Assert.True(constructorGen.Parameters.ContainsKey("numberOfTests"));
            Assert.True(constructorGen.Parameters.ContainsKey("testText"));
            Assert.Equal("int", constructorGen.Parameters["numberOfTests"]);
            Assert.Equal("string", constructorGen.Parameters["testText"]);
            Assert.True(constructorGen.Parameters.ContainsKey("testText"));
            Assert.Empty(constructorGen.Body);
        }

        [Fact]
        void TestConstructorWithBase()
        {

            var constructorGen = new ConstructorGen(className: "TestClass");

            var generatedCode = constructorGen.Protected()
                .WithBase("arg1", "arg2")
                .GenerateCode();

            var expectedCode = """"
                protected TestClass() : base(arg1, arg2)
                {
                }
                
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.Contains("arg1", constructorGen.Base);
            Assert.Contains("arg2", constructorGen.Base);
        }

        [Fact]
        void TestConstructorWithParameterAndBase()
        {
            var constructorGen = new ConstructorGen(className: "TestClass");

            var generatedCode = constructorGen.Private()
                .WithParameter("string", "arg1")
                .WithParameter("string", "arg2")
                .WithBase("arg1", "arg2")
                .GenerateCode();

            var expectedCode = """"
                private TestClass(string arg1, string arg2) : base(arg1, arg2)
                {
                }
                
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.True(constructorGen.Parameters.ContainsKey("arg1"));
            Assert.True(constructorGen.Parameters.ContainsKey("arg2"));
            Assert.Equal("string", constructorGen.Parameters["arg1"]);
            Assert.Equal("string", constructorGen.Parameters["arg2"]);
            Assert.Contains("arg1", constructorGen.Base);
            Assert.Contains("arg2", constructorGen.Base);
        }


        
        [Fact]
        void TestConstructorWithParameterAndBaseAndBody()
        {
            var body = """"
                this.arg1 = arg1;
                this.arg2 = arg2;
                """";

            var constructorGen = new ConstructorGen(className: "TestClass");

            var generatedCode = constructorGen.Private()
                .WithParameter("string", "arg1")
                .WithParameter("string", "arg2")
                .WithBase("arg1", "arg2")
                .WithBody(body)
                .GenerateCode();

            var expectedCode = """"
                private TestClass(string arg1, string arg2) : base(arg1, arg2)
                {
                    this.arg1 = arg1;
                    this.arg2 = arg2;
                }
                
                """";
            Assert.Equal(expectedCode, generatedCode);
            Assert.Equal(body, constructorGen.Body);
        }


        [Fact]
        void TestReplaceParameter()
        {

            var constructorGen = new ConstructorGen(className: "TestClass");

            constructorGen.WithParameter("object", "numberOfTests");
            constructorGen.WithParameter("int", "numberOfTests");

            var generatedCode = constructorGen.GenerateCode();

            var expectedCode = """"
                TestClass(int numberOfTests)
                {
                }
                
                """";
            Assert.Equal(expectedCode, generatedCode);
        }
    }
}
