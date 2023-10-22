namespace Fluent.CodeGen
{
    public class MethodGen
    {
        private readonly string type;
        private readonly string name;
        public MethodGen(string type, string name) 
        {
            this.type = type;
            this.name = name;
        }

        public MethodGen Public()
        {
            return this;
        }

        public MethodGen Static()
        {
            return this;
        }

        public MethodGen WithParameter(string type, string name)
        {
            return this;
        }

        public MethodGen WithBody(string body)
        {
            return this;
        }


        public string GenerateCode()
        {
            return string.Empty;
        }
    }
}
