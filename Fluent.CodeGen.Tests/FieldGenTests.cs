using Xunit;

namespace Fluent.CodeGen.Tests
{
    public class FieldGenTests
    {
        [Fact]
        public void TestNormalField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test");

            var expectedCode = "string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }

        [Fact]
        public void TestStaticField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Static();

            var expectedCode = "static string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }

        public readonly string test;

        [Fact]
        public void TestPublicStaticField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Public()
                .Static();

            var expectedCode = "public static string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }

        [Fact]
        public void TestPublicStaticFieldAssigned()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Public()
                .Static()
                .Assign("\"value\"");

            var expectedCode = "public static string test = \"value\";";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }

        [Fact]
        public void TestPublicStaticReadonlyField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Public()
                .Static()
                .Readonly();

            var expectedCode = "public static readonly string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }

        [Fact]
        public void TestPublicStaticReadonlyFieldAssigned()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Public()
                .Static()
                .Readonly()
                .Assign("\"value\"");

            var expectedCode = "public static readonly string test = \"value\";";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }
    }
}