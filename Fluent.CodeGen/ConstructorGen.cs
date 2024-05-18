using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class ConstructorGen : CodeGen
    {
        private string accessModifier = AccessModifiers.Default;
        public string Body { get; private set; } = string.Empty;
        public string ClassName { get; private set; } = string.Empty;
        public HashSet<string> Base { get; private set; }

        private IDictionary<string, string> parameters;

        public ConstructorGen(string className)
        {
            ClassName = className;
            parameters = new Dictionary<string, string>();
            Base = new HashSet<string>();
        }

        public ConstructorGen Public()
        {
            accessModifier = AccessModifiers.Public;
            return this;
        }

        public ConstructorGen Private()
        {
            accessModifier = AccessModifiers.Private;
            return this;
        }

        public ConstructorGen Protected()
        {
            accessModifier = AccessModifiers.Protected;
            return this;
        }

        public ConstructorGen Internal()
        {
            accessModifier = AccessModifiers.Internal;
            return this;
        }

        public ConstructorGen WithBody(string body)
        {
            Body = body;
            return this;
        }

        public ConstructorGen WithParameter(string type, string name)
        {
            parameters.Add(name, type);
            return this;
        }

        public ConstructorGen WithBase(params string [] arguments)
        {
            foreach (var argument in arguments)
            {
                Base.Add(argument);
            }
            return this;
        }

        public override string GenerateCode()
        {
            indentedTextWriter.Write(accessModifier);
            if(!AccessModifiers.Default.Equals(accessModifier))
            {
                indentedTextWriter.Write(" ");
            }

            indentedTextWriter.Write(ClassName);
            indentedTextWriter.Write("(");

            var @params = string.Join(", ", parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
            this.indentedTextWriter.Write(@params);

            if(Base.Any())
            {
                indentedTextWriter.Write(")");
                indentedTextWriter.Write(" : base(");
                indentedTextWriter.Write(string.Join(", ", Base));
            }

            indentedTextWriter.WriteLine(")");

            indentedTextWriter.WriteLine("{");
            indentedTextWriter.Indent++;

            WriteMultipleLines(Body);

            indentedTextWriter.Indent--;
            indentedTextWriter.WriteLine("}");

            return stringWriter.ToString();
        }
    }
}
