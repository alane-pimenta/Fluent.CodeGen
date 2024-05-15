namespace Fluent.CodeGen
{
    public class FieldGen
    {
        private bool isStatic = false;
        private string type;
        private string name;

        public FieldGen Static()
        {
            isStatic = true;
            return this;
        }
    }
}
