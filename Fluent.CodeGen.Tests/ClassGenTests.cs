namespace Fluent.CodeGen.Tests
{
    public class ClassGenTests
    {

        [Fact]
        public void TestClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .GenerateCode();

            var expectedCode = """"
                using System;
                class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        public void TestClassWithoutNamespaceAndMultipleUsings()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System", "System.Text")
                .GenerateCode();

            var expectedCode = """"
                using System;
                using System.Text;
                class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        public void TestPublicClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Public()
                .GenerateCode();

            var expectedCode = """"
                using System;
                public class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }



        [Fact]
        public void TestStaticClassWithoutNamespace()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Public()
                .Static()
                .GenerateCode();

            var expectedCode = """"
                using System;
                public static class Program
                {
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        public void TestNormalClass()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public class Program
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);

        }



        [Fact]
        public void TestExtends()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Extends("MyClass")
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public class Program : MyClass
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }



        [Fact]
        public void TestExtendsAndImplements()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Extends("MyClass")
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public class Program : MyClass, IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }


        [Fact]
        public void TestImplements()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }



        [Fact]
        public void TestStatic()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .Static()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public static class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        [Fact]
        public void TestDefaultStatic()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Static()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();


            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    static class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }

        
        [Fact]
        public void TestInternalClass()
        {
            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Internal()
                .Implements("IInterface", "IInterface2")
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    internal class Program : IInterface, IInterface2
                    {
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }



        [Fact]
        public void TestClassWithMethod()
        {
            var methodGen = new MethodGen(name: "MethodTest")
                .Public()
                .WithReturnType("int")
                .WithBody("return 0;");

            var classGen = new ClassGen(name: "Program");

            var generatedCode = classGen
                .Using("System")
                .Namespace("My.Test")
                .Public()
                .WithMethod(methodGen)
                .GenerateCode();

            var expectedCode = """"
                using System;
                namespace My.Test
                {
                    public class Program
                    {
                        public int MethodTest()
                        {
                            return 0;
                        }
                    }
                }
                """";

            Assert.Equal(expectedCode, generatedCode);
        }
    }
}