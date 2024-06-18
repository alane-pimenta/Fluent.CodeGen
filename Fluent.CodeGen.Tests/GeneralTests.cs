namespace Fluent.CodeGen.Tests
{
    public class GeneralTests
    {
        [Fact]
        void RunGenerateCodeMultipleTimes()
        {
            var classGen = new ClassGen("Test")
                .WithField(new FieldGen("int", "count").Public());

            string firstMethodCall = classGen.GenerateCode();

            string secondMethodCall = classGen.GenerateCode();
            string thirdMethodCall = classGen.GenerateCode();

            Assert.Equal(firstMethodCall, secondMethodCall);
            Assert.Equal(firstMethodCall, thirdMethodCall);

        }
    }
}
