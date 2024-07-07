using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class ClassGen
    {
        private readonly StringBuilder usings;
        private string? @namespace;
        private string className = "";
        private string? extends;
        private bool isStatic;
        private List<string> implements;
        private string accessModifier = string.Empty;
        private List<string> attributesList = new List<string>();

        private readonly IndentedTextWriter indentedTextWriter;
        private readonly StringWriter stringWriter;

        public ClassGen(string name)
        {
            this.className = name;
            this.usings = new StringBuilder();
            this.stringWriter = new StringWriter();
            this.indentedTextWriter = new IndentedTextWriter(stringWriter);
            this.implements = new List<string>();
        }

        public ClassGen Using(params string[] @using)
        {
            foreach (string @namespace in @using)
            {
                this.usings.AppendLine($"using {@namespace};");
            }
            return this;
        }

        public ClassGen Implements(params string[] @using)
        {
            this.implements.AddRange(@using);
            return this;
        }

        public ClassGen Public()
        {
            this.accessModifier = AccessModifiers.Public;
            return this;
        }

        public ClassGen Extends(string className)
        {
            this.extends = className;
            return this;
        }


        public ClassGen Namespace(string @namespace)
        {
            this.@namespace = @namespace;
            return this;
        }



        public ClassGen WithAttribute(string attribute)
        {
            this.attributesList.Add(attribute);
            return this;
        }

        public string GenerateCode()
        {
            this.indentedTextWriter.Write(usings.ToString());
            if (!string.IsNullOrEmpty(@namespace))
            {
                this.indentedTextWriter.WriteLine($"namespace {@namespace}");
                this.indentedTextWriter.WriteLine("{");
                this.indentedTextWriter.Indent++;
                this.GenerateClassBody();
                this.indentedTextWriter.Indent--;
                this.indentedTextWriter.WriteLine("}");
            }
            else
            {
                this.GenerateClassBody();
            }

            return stringWriter.GetStringBuilder().ToString();
        }

        private void GenerateClassBody()
        {
            var classDeclaration = new StringBuilder();
            foreach (var attribute in attributesList) 
            {
                classDeclaration.AppendLine($"{attribute}");
            }

            classDeclaration.Append(accessModifier);
            if (!string.IsNullOrEmpty(accessModifier))
            {
                classDeclaration.Append(' ');
            }

            if (isStatic)
            {
                classDeclaration.Append("static ");
            }
            classDeclaration.Append($"class {className}");

            if (this.implements.Count > 0 || !string.IsNullOrEmpty(extends))
            {
                classDeclaration.Append(" : ");
                if (!string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(extends);
                }

                if (this.implements.Count > 0 && !string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(", ");
                }

                if (this.implements.Count > 0)
                {
                    var implemented = string.Join(", ", implements.ToArray());
                    classDeclaration.Append(implemented);
                }
            }

            this.indentedTextWriter.WriteLine(classDeclaration.ToString());
            this.indentedTextWriter.WriteLine("{");
            this.indentedTextWriter.WriteLine("}");
        }

        public ClassGen Static()
        {
            this.isStatic = true;
            return this;
        }
    }
}
