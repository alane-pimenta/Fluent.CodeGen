using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class ClassGen : CodeGen
    {
        private string? @namespace;
        public string ClassName { get; private set; }
        private string? extends;
        private bool isStatic;
        private List<string> implements;
        private List<string> namespaces;
        private string accessModifier = AccessModifiers.Default;

        private List<MethodGen> methods;

        public ClassGen(string name)
        {
            ClassName = name;
            implements = new List<string>();
            namespaces = new List<string>();
            methods = new List<MethodGen>();
        }

        public ClassGen Using(params string[] namespaces)
        {
            this.namespaces.AddRange(namespaces);
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

        public ClassGen Internal()
        {
            this.accessModifier = AccessModifiers.Internal;
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

        public ClassGen Static()
        {
            this.isStatic = true;
            return this;
        }

        public ClassGen WithMethod(MethodGen method)
        {
            this.methods.Add(method);
            return this;
        }

        public override string GenerateCode()
        {
            foreach (string @namespace in namespaces)
            {
                this.indentedTextWriter.WriteLine($"using {@namespace};");
            }

            if (!string.IsNullOrEmpty(@namespace))
            {
                this.indentedTextWriter.WriteLine($"namespace {@namespace}");
                this.indentedTextWriter.WriteLine("{");
                this.indentedTextWriter.Indent++;
                this.GenerateClassBody();
                this.indentedTextWriter.Indent--;
                this.indentedTextWriter.Write("}");
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
            classDeclaration.Append(accessModifier);
            if (!string.IsNullOrEmpty(accessModifier))
            {
                classDeclaration.Append(' ');
            }
            
            if(isStatic)
            {
                classDeclaration.Append("static ");
            }
            classDeclaration.Append($"class {ClassName}");

            if(this.implements.Count > 0 || !string.IsNullOrEmpty(extends))
            {
                classDeclaration.Append(" : ");
                if (!string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(extends);
                }
                
                if(this.implements.Count > 0 && !string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(", ");
                }

                if(this.implements.Count > 0)
                {
                    var implemented = string.Join(", ", implements.ToArray());
                    classDeclaration.Append(implemented);
                }
            }

            this.indentedTextWriter.WriteLine(classDeclaration.ToString());
            this.indentedTextWriter.WriteLine("{");
            this.indentedTextWriter.Indent++;
            methods.ForEach(method => 
            {
                this.indentedTextWriter.WriteLine(method.GenerateCode(this.indentedTextWriter.Indent));
            });

            this.indentedTextWriter.Indent--;
            if(!string.IsNullOrEmpty(@namespace))
            {
                this.indentedTextWriter.WriteLine("}");
            }
            else
            {
                this.indentedTextWriter.Write("}");
            }
        }
    }
}
