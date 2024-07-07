using Xunit;

namespace Fluent.CodeGen.Tests
{
    public class FieldGenTests
    {
        [Fact]
        public void TestNormalClass()
        {
            var fieldGen = new FieldGen();


            var expectedCode = @"";
            Assert.Equal(expectedCode, "");

        }
        [Fact]
        public void TestAddSingleAttribute()
        {
            var fieldGen = new FieldGen("TestField", "string")
            .WithAttribute("MyAttribute");

            var expectedCode = @"MyAttribute\r\nstring TestField;";
            Assert.Equal(expectedCode, fieldGen.GenerateCode());
        }
    
    }
}