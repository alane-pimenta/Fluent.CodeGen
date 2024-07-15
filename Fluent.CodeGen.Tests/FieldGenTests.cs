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
            Assert.Equal("string", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Null(fieldGen.AssignedValue);
            Assert.False(fieldGen.IsStatic);
            Assert.False(fieldGen.IsReadonly);
            Assert.Empty(fieldGen.AccessModifier);
        }

        [Fact]
        public void TestStaticField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Static();

            var expectedCode = "static string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
            Assert.Equal("string", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Null(fieldGen.AssignedValue);
            Assert.True(fieldGen.IsStatic);
            Assert.False(fieldGen.IsReadonly);
            Assert.Empty(fieldGen.AccessModifier);
        }

        [Fact]
        public void TestPublicStaticField()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Protected()
                .Static();

            var expectedCode = "protected static string test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
            Assert.Equal("string", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Null(fieldGen.AssignedValue);
            Assert.True(fieldGen.IsStatic);
            Assert.False(fieldGen.IsReadonly);
            Assert.Equal("protected", fieldGen.AccessModifier);
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
            Assert.Equal("string", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Equal("\"value\"", fieldGen.AssignedValue);
            Assert.True(fieldGen.IsStatic);
            Assert.False(fieldGen.IsReadonly);
            Assert.Equal("public", fieldGen.AccessModifier);
        }

        [Fact]
        public void TestPublicStaticReadonlyField()
        {
            var fieldGen = new FieldGen(type: "int", name: "test")
                .Private()
                .Static()
                .Readonly();

            var expectedCode = "private static readonly int test;";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
            Assert.Equal("int", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Null(fieldGen.AssignedValue);
            Assert.True(fieldGen.IsStatic);
            Assert.True(fieldGen.IsReadonly);
            Assert.Equal("private", fieldGen.AccessModifier);
        }

        [Fact]
        public void TestInternalStaticReadonlyFieldAssigned()
        {
            var fieldGen = new FieldGen(type: "string", name: "test")
                .Internal()
                .Static()
                .Readonly()
                .Assign("\"value\"");

            var expectedCode = "internal static readonly string test = \"value\";";

            Assert.Equal(expectedCode, fieldGen.GenerateCode());
            Assert.Equal("string", fieldGen.Type);
            Assert.Equal("test", fieldGen.Name);
            Assert.Equal("\"value\"", fieldGen.AssignedValue);
            Assert.True(fieldGen.IsStatic);
            Assert.True(fieldGen.IsReadonly);
            Assert.Equal("internal", fieldGen.AccessModifier);
        }

        [Fact]
        public void TestAddingAndRemovingAttributes()
        {
            // Arrange
            var fieldGen = new FieldGen(type: "string", name: "test");

            fieldGen.AddAttribute("[Obsolete]");
            var expectedCodeWithAttribute = "string test [Obsolete];";
            Assert.Equal(expectedCodeWithAttribute, fieldGen.GenerateCode());
        }
    }
}
