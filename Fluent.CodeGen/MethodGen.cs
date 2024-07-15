using System.Collections.Generic;
using System.Linq;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class MethodGen : CodeGen
    {
        public string Name { get; private set; }
        public string AccessModifier { get; private set; } = AccessModifiers.Default;
        public IDictionary<string, string> Parameters { get; private set; }
        public bool IsStatic { get; private set; } = false;
        public string Body { get; private set; } = string.Empty;
        public string ReturnType { get; private set; } = "void";
        public bool IsOverride { get; private set; } = false;
        private readonly List<string> attributesList = new List<string>();

        public MethodGen(string name)
        {
            Name = name;
            Parameters = new Dictionary<string, string>();
        }

        public MethodGen(string returnType, string name)
        {
            ReturnType = returnType;
            Name = name;
            Parameters = new Dictionary<string, string>();
        }

        public MethodGen Public()
        {
            AccessModifier = AccessModifiers.Public;
            return this;
        }

        public MethodGen Private()
        {
            AccessModifier = AccessModifiers.Private;
            return this;
        }

        public MethodGen Protected()
        {
            AccessModifier = AccessModifiers.Protected;
            return this;
        }

        public MethodGen Internal()
        {
            AccessModifier = AccessModifiers.Internal;
            return this;
        }

        public MethodGen Override()
        {
            IsOverride = true;
            return this;
        }

        public MethodGen Static()
        {
            IsStatic = true;
            return this;
        }

        public MethodGen WithParameter(string type, string name)
        {
            Parameters[name] = type;
            return this;
        }

        public MethodGen WithBody(string body)
        {
            Body = body;
            return this;
        }

        public MethodGen WithReturnType(string returnType)
        {
            ReturnType = returnType;
            return this;
        }

        public MethodGen AddAttribute(string attribute)
        {
            attributesList.Add(attribute);
            return this;
        }

        public override string GenerateCode()
        {
            Flush();

            foreach (var attribute in attributesList)
            {
                indentedTextWriter.WriteLine(attribute);
            }

            if (!AccessModifiers.Default.Equals(AccessModifier))
            {
                indentedTextWriter.Write($"{AccessModifier} ");
            }

            if (IsOverride)
            {
                indentedTextWriter.Write("override ");
            }

            if (IsStatic)
            {
                indentedTextWriter.Write("static ");
            }
            indentedTextWriter.Write(ReturnType);
            indentedTextWriter.Write(" ");

            indentedTextWriter.Write(Name);
            indentedTextWriter.Write("(");

            var @params = string.Join(", ", Parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
            indentedTextWriter.Write(@params);

            indentedTextWriter.WriteLine(")");
            indentedTextWriter.WriteLine("{");
            indentedTextWriter.Indent++;

            WriteMultipleLines(Body);

            indentedTextWriter.Indent--;
            indentedTextWriter.Write("}");

            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
