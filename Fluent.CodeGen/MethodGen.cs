using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class MethodGen : CodeGen
    {
        public string Name { get; private set; }
        private string accessModifier = AccessModifiers.Default;
        private IDictionary<string, string> parameters;
        public ReadOnlyDictionary<string, string> Parameters { get => new ReadOnlyDictionary<string, string>(parameters); }
        public bool IsStatic { get; private set; } = false;
        public string Body { get; private set; } = string.Empty;
        public string ReturnType { get; private set; } = "void";
        public bool IsOverride { get; private set; } = false;

        public MethodGen(string name)
        {
            Name = name;
            parameters = new Dictionary<string, string>();
        }

        public MethodGen(string returnType, string name)
        {
            ReturnType = returnType;
            Name = name;
            parameters = new Dictionary<string, string>();
        }

        public MethodGen Public()
        {
            accessModifier = AccessModifiers.Public;
            return this;
        }

        public MethodGen Private()
        {
            accessModifier = AccessModifiers.Private;
            return this;
        }

        public MethodGen Protected()
        {
            accessModifier = AccessModifiers.Protected;
            return this;
        }

        public MethodGen Internal()
        {
            accessModifier = AccessModifiers.Internal;
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
            parameters.Add(name, type);
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

        public override string GenerateCode()
        {
            Flush();

            if(!AccessModifiers.Default.Equals(accessModifier))
            {
                indentedTextWriter.Write($"{accessModifier} ");
            }

            if(IsOverride)
            {
                indentedTextWriter.Write("override ");
            }

            if(IsStatic)
            {
                indentedTextWriter.Write("static ");
            }
            indentedTextWriter.Write(ReturnType);
            indentedTextWriter.Write(" ");

            indentedTextWriter.Write(Name);
            indentedTextWriter.Write("(");

            var @params = string.Join(", ", parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
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
