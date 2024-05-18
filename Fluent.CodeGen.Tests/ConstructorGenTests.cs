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
        }


    }
}
