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
    }
}