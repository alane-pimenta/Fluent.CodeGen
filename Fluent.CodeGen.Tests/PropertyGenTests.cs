using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen.Tests
{
    public class PropertyGenTests
    {

        [Fact]
        public void TestNormalProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test");

            var expectedCode = "string Test { get; set; }";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.Equal("string", propertyGen.Type);
            Assert.Equal("Test", propertyGen.Name);
            Assert.True(propertyGen.HasGet);
            Assert.True(propertyGen.HasSet);
        }

        [Fact]
        public void TestNormalPropertyWithAssignment()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Assign("\"Test\"");

            var expectedCode = "string Test { get; set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.Equal("\"Test\"", propertyGen.AssignedValue);
        }

        [Fact]
        public void TestNormalPublicPropertyWithAssignment()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Public()
                .Assign("\"Test\"");

            var expectedCode = "public string Test { get; set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.Equal("public", propertyGen.AccessModifier);
        }

        [Fact]
        public void TestNormalPrivateProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Private()
                .Assign("\"Test\"");

            var expectedCode = "private string Test { get; set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
        }

        [Fact]
        public void TestNormalProtectedProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Protected()
                .Assign("\"Test\"");

            var expectedCode = "protected string Test { get; set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
        }

        [Fact]
        public void TestNormalPrivateStaticProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Private()
                .Static()
                .Assign("\"Test\"");

            var expectedCode = "private static string Test { get; set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
        }

        [Fact]
        public void TestPrivateSetProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Set(AccessModifiers.Private, "");

            var expectedCode = "string Test { get; private set; }";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
        }

        [Fact]
        public void TestPrivateSetPropertyWithAssignment()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Set(AccessModifiers.Private, "")
                .Assign("\"Test\"");

            var expectedCode = "string Test { get; private set; } = \"Test\";";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.Equal("private", propertyGen.SetAccessModifier);
        }

        [Fact]
        public void TestNoSetProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .NoSet();

            var expectedCode = "string Test { get; }";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.True(propertyGen.HasGet);
            Assert.False(propertyGen.HasSet);
        }

        [Fact]
        public void TestNoGetAndSetProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .NoGet()
                .NoSet();

            var expectedCode = "string Test;";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.False(propertyGen.HasGet);
            Assert.False(propertyGen.HasSet);
        }

        [Fact]
        public void TestSingleLineProperty()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Get("throw new Exception()")
                .Set("throw new Exception()");

            var expectedCode = "string Test { get => throw new Exception(); set => throw new Exception(); }";

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
            Assert.Equal("throw new Exception()", propertyGen.GetBody);
            Assert.Equal("throw new Exception()", propertyGen.SetBody);
        }

        [Fact]
        public void TestMultiLineProperty()
        {
            var getBody = """
                string final = DateTime.Now.ToShortDateString() + " - " + test;
                return final;
                """;

            var setBody = """
                string final = value.ToLower();
                this.test = final;
                """;

            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Get(getBody)
                .Set(setBody);

            var expectedCode = """
                string Test
                {
                    get
                    {
                        string final = DateTime.Now.ToShortDateString() + " - " + test;
                        return final;
                    }
                    set
                    {
                        string final = value.ToLower();
                        this.test = final;
                    }
                }
                """;

            Assert.Equal(expectedCode, propertyGen.GenerateCode());
        }

        [Fact]
        public void TestMultiLineAndSingleLineProperty()
        {

            var getBody = """
                string final = DateTime.Now.ToShortDateString() + " - " + test;
                return final;
                """;

            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Get(getBody)
                .Set("throw new Exception()");

            var expectedCode = """
                string Test
                {
                    get
                    {
                        string final = DateTime.Now.ToShortDateString() + " - " + test;
                        return final;
                    }
                    set => throw new Exception();
                }
                """;

            var actualCode = propertyGen.GenerateCode();

            Assert.Equal(expectedCode, actualCode);
        }

        [Fact]
        public void TestOnlyMultiLineProperty()
        {

            var getBody = """
                string final = DateTime.Now.ToShortDateString() + " - " + test;
                return final;
                """;

            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Get(getBody)
                .NoSet();

            var expectedCode = """
                string Test
                {
                    get
                    {
                        string final = DateTime.Now.ToShortDateString() + " - " + test;
                        return final;
                    }
                }
                """;

            var actualCode = propertyGen.GenerateCode();

            Assert.Equal(expectedCode, actualCode);
        }


        [Fact]
        public void TestMultiLinePropertyWithEmptySet()
        {

            var getBody = """
                string final = DateTime.Now.ToShortDateString() + " - " + test;
                return final;
                """;

            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .Get(getBody);

            //This is wrong for C# specs, but since the default of get and set is empty, this would be the correct output
            var expectedCode = """
                string Test
                {
                    get
                    {
                        string final = DateTime.Now.ToShortDateString() + " - " + test;
                        return final;
                    }
                    set;
                }
                """;

            var actualCode = propertyGen.GenerateCode();

            Assert.Equal(expectedCode, actualCode);
        }

        [Fact]
        public void TestPropertyWithAttributes()
        {
            var propertyGen = new PropertyGen(type: "string", name: "Test")
                .AddAttribute("[JsonProperty(\"test\")]")
                .AddAttribute("[DataMember]");

            var expectedCode = """
                [JsonProperty("test")]
                [DataMember]
                string Test { get; set; }
                """;

            var actualCode = propertyGen.GenerateCode();

            Assert.Equal(expectedCode, actualCode);
        }
    }
}