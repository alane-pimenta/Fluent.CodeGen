using System.Collections.Generic;
using System.Linq;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class ConstructorGen : CodeGen
    {
        public string AccessModifier { get; private set; }
        public string Body { get; private set; }
        public string ClassName { get; private set; }
        public HashSet<string> Base { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }
        public List<string> Attributes { get; private set; }


        public ConstructorGen(string className)
        {
            AccessModifier = AccessModifiers.Default;
            Body = string.Empty;
            ClassName = className;
            Base = new HashSet<string>();
            Parameters = new Dictionary<string, string>();
            Attributes = new List<string>();
        }

        public ConstructorGen Public()
        {
            AccessModifier = AccessModifiers.Public;
            return this;
        }

        public ConstructorGen Private()
        {
            AccessModifier = AccessModifiers.Private;
            return this;
        }

        public ConstructorGen Protected()
        {
            AccessModifier = AccessModifiers.Protected;
            return this;
        }

        public ConstructorGen Internal()
        {
            AccessModifier = AccessModifiers.Internal;
            return this;
        }

        public ConstructorGen WithBody(string body)
        {
            Body = body;
            return this;
        }

        public ConstructorGen WithParameter(string type, string name)
        {
            Parameters[name] = type;
            return this;
        }

        public ConstructorGen WithBase(params string[] arguments)
        {
            foreach (var argument in arguments)
            {
                Base.Add(argument);
            }
            return this;
        }
        public ConstructorGen WithAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                Attributes.Add(attribute);
            }
            return this;
        }

        public override string GenerateCode()
        {
            Flush();
            foreach (var attribute in Attributes)
            {
                indentedTextWriter.WriteLine(attribute);
            }

            indentedTextWriter.Write(AccessModifier);
            if (!AccessModifiers.Default.Equals(AccessModifier))
            {
                indentedTextWriter.Write(" ");
            }

            indentedTextWriter.Write(ClassName);
            indentedTextWriter.Write("(");

            var @params = string.Join(", ", Parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
            indentedTextWriter.Write(@params);

            if (Base.Any())
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
