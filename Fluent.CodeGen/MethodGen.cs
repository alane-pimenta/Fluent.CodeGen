using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class MethodGen : CodeGen
    {
        private readonly string name;
        private string accessModifier = AccessModifiers.Default;
        private IDictionary<string, string> parameters;
        private bool isStatic;
        private string body = string.Empty;
        private string returnType = "void";

        public MethodGen(string name) 
        {
            this.name = name;
            this.parameters = new Dictionary<string, string>();
        }

        public MethodGen Public()
        {
            accessModifier = AccessModifiers.Public;
            return this;
        }

        public MethodGen Static()
        {
            this.isStatic = true;
            return this;
        }

        public MethodGen WithParameter(string type, string name)
        {
            parameters.Add(name, type);
            return this;
        }

        public MethodGen WithBody(string body)
        {
            this.body = body;
            return this;
        }

        public MethodGen WithReturnType(string returnType)
        {
            this.returnType = returnType;
            return this;
        }

        public override string GenerateCode()
        {
            this.indentedTextWriter.NewLine = "\n";
            this.indentedTextWriter.Write(accessModifier);
            this.indentedTextWriter.Write(" ");
            if(isStatic)
            {
                this.indentedTextWriter.Write("static");
                this.indentedTextWriter.Write(" ");
            }
            this.indentedTextWriter.Write(returnType);
            this.indentedTextWriter.Write(" ");

            this.indentedTextWriter.Write(name);
            this.indentedTextWriter.Write("(");

            var @params = string.Join(", ", parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
            this.indentedTextWriter.Write(@params);
            
            this.indentedTextWriter.WriteLine(")");
            this.indentedTextWriter.WriteLine("{");
            this.indentedTextWriter.Indent++;
            body.Split("\n").ToList().ForEach(this.indentedTextWriter.WriteLine);
            this.indentedTextWriter.Indent--;
            this.indentedTextWriter.Write("}");

            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
