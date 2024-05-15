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
            this.Name = name;
            this.parameters = new Dictionary<string, string>();
        }

        public MethodGen(string returnType, string name) 
        {
            this.ReturnType = returnType;
            this.Name = name;
            this.parameters = new Dictionary<string, string>();
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
            this.IsOverride = true;
            return this;
        }

        public MethodGen Static()
        {
            this.IsStatic = true;
            return this;
        }

        public MethodGen WithParameter(string type, string name)
        {
            parameters.Add(name, type);
            return this;
        }

        public MethodGen WithBody(string body)
        {
            this.Body = body;
            return this;
        }

        public MethodGen WithReturnType(string returnType)
        {
            this.ReturnType = returnType;
            return this;
        }

        public override string GenerateCode()
        {
            this.indentedTextWriter.NewLine = "\n";

            if(!AccessModifiers.Default.Equals(accessModifier))
            {
                this.indentedTextWriter.Write($"{accessModifier} ");
            }
            
            if(IsOverride)
            {
                this.indentedTextWriter.Write("override ");
            }
            
            if(IsStatic)
            {
                this.indentedTextWriter.Write("static ");
            }
            this.indentedTextWriter.Write(ReturnType);
            this.indentedTextWriter.Write(" ");

            this.indentedTextWriter.Write(Name);
            this.indentedTextWriter.Write("(");

            var @params = string.Join(", ", parameters.Select(parameter => $"{parameter.Value} {parameter.Key}"));
            this.indentedTextWriter.Write(@params);
            
            this.indentedTextWriter.WriteLine(")");
            this.indentedTextWriter.WriteLine("{");
            this.indentedTextWriter.Indent++;
            Body.Split("\n").ToList().ForEach(this.indentedTextWriter.WriteLine);
            this.indentedTextWriter.Indent--;
            this.indentedTextWriter.Write("}");

            return stringWriter.GetStringBuilder().ToString();
        }
    }
}
