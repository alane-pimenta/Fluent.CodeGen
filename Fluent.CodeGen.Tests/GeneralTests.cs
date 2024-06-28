namespace Fluent.CodeGen.Tests
{
    public class GeneralTests
    {
        [Fact]
        void RunGenerateCodeWithFieldMultipleTimes()
        {
            var classGen = new ClassGen("Test")
                .WithField(new FieldGen("int", "count").Public());

            string firstMethodCall = classGen.GenerateCode();

            string secondMethodCall = classGen.GenerateCode();
            string thirdMethodCall = classGen.GenerateCode();

            Assert.Equal(firstMethodCall, secondMethodCall);
            Assert.Equal(firstMethodCall, thirdMethodCall);

        }

        [Fact]
        void RunGenerateCodeWithPropertyMultipleTime()
        {
            var classGen = new ClassGen("Test")
                .WithProperty(new PropertyGen("int", "Count").Public());

            string firstMethodCall = classGen.GenerateCode();

            string secondMethodCall = classGen.GenerateCode();
            string thirdMethodCall = classGen.GenerateCode();

            Assert.Equal(firstMethodCall, secondMethodCall);
            Assert.Equal(firstMethodCall, thirdMethodCall);
        }

        [Fact]
        void RunGenerateCodeWithMethodMultipleTime()
        {
            var classGen = new ClassGen("Test")
                .WithMethod(new MethodGen("int", "Count").Public());

            string firstMethodCall = classGen.GenerateCode();

            string secondMethodCall = classGen.GenerateCode();
            string thirdMethodCall = classGen.GenerateCode();

            Assert.Equal(firstMethodCall, secondMethodCall);
            Assert.Equal(firstMethodCall, thirdMethodCall);
        }

        [Fact]
        void RunGenerateCodeWithConstructorMultipleTime()
        {
            var classGen = new ClassGen("Test")
                .Constructor(ctor => ctor.Public());

            string firstMethodCall = classGen.GenerateCode();

            string secondMethodCall = classGen.GenerateCode();
            string thirdMethodCall = classGen.GenerateCode();

            Assert.Equal(firstMethodCall, secondMethodCall);
            Assert.Equal(firstMethodCall, thirdMethodCall);
        }
    }
}
