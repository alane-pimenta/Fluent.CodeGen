using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using Fluent.CodeGen.Consts;
using System.Linq;
using System;

namespace Fluent.CodeGen
{
    public class ClassGen : CodeGen
    {
        private string? @namespace;
        public string ClassName { get; private set; }
        private string? extends;
        private bool isStatic;
        private HashSet<string> implements;
        private HashSet<string> namespaces;
        private string accessModifier = AccessModifiers.Default;

        private List<MethodGen> methods;
        private ConstructorGen constructor;
        private List<FieldGen> fields;

        public ClassGen(string name)
        {
            ClassName = name;
            implements = new HashSet<string>();
            namespaces = new HashSet<string>();
            methods = new List<MethodGen>();
            fields = new List<FieldGen>();
        }

        public ClassGen Using(params string[] namespaces)
        {
            foreach(var @namespace in namespaces)
            {
                this.namespaces.Add(@namespace);
            }
            return this;
        }

        public ClassGen Implements(params string[] usings)
        {
            foreach (var @using in usings)
            {
                implements.Add(@using);
            }
            return this;
        }

        public ClassGen Public()
        {
            accessModifier = AccessModifiers.Public;
            return this;
        }

        public ClassGen Internal()
        {
            accessModifier = AccessModifiers.Internal;
            return this;
        }

        public ClassGen Extends(string className)
        {
            extends = className;
            return this;
        }
        
        public ClassGen Namespace(string @namespace)
        {
            this.@namespace = @namespace;
            return this;
        }

        public ClassGen Static()
        {
            isStatic = true;
            return this;
        }

        public ClassGen WithMethod(MethodGen method)
        {
            methods.Add(method);
            return this;
        }

        public ClassGen Constructor(Action<ConstructorGen> ctor)
        {
            constructor = new ConstructorGen(className: ClassName);
            ctor.Invoke(constructor);
            return this;
        }

        public ClassGen WithField(FieldGen fieldGen)
        {
            fields.Add(fieldGen);
            return this;
        }

        public override string GenerateCode()
        {
            foreach (string @namespace in namespaces)
            {
                indentedTextWriter.WriteLine($"using {@namespace};");
            }

            if(namespaces.Any())
            {
                indentedTextWriter.WriteLine();
            }

            if (!string.IsNullOrEmpty(@namespace))
            {
                indentedTextWriter.WriteLine($"namespace {@namespace}");
                indentedTextWriter.WriteLine("{");
                indentedTextWriter.Indent++;
                GenerateClassBody();
                indentedTextWriter.Indent--;
                indentedTextWriter.Write("}");
            }
            else
            {
                GenerateClassBody();
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

            if(implements.Count > 0 || !string.IsNullOrEmpty(extends))
            {
                classDeclaration.Append(" : ");
                if (!string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(extends);
                }
                
                if(implements.Count > 0 && !string.IsNullOrEmpty(extends))
                {
                    classDeclaration.Append(", ");
                }

                if(implements.Count > 0)
                {
                    var implemented = string.Join(", ", implements.ToArray());
                    classDeclaration.Append(implemented);
                }
            }

            indentedTextWriter.WriteLine(classDeclaration.ToString());
            indentedTextWriter.WriteLine("{");
            indentedTextWriter.Indent++;

            foreach(var field in fields)
            {
                indentedTextWriter.WriteLine(field.GenerateCode());
            }

            if(fields.Any())
            {
                var indent = indentedTextWriter.Indent;
                indentedTextWriter.Indent = 0;
                indentedTextWriter.WriteLine();
                indentedTextWriter.Indent = indent;
            }

            if(constructor is not null)
            {
                indentedTextWriter.WriteLine(constructor.GenerateCode(indentedTextWriter.Indent));
            }

            var last = methods.LastOrDefault();
            methods.ForEach(method => 
            {
                indentedTextWriter.WriteLine(method.GenerateCode(indentedTextWriter.Indent));
                if(!last.Equals(method))
                {
                    var indentation = indentedTextWriter.Indent;
                    indentedTextWriter.Indent = 0;
                    indentedTextWriter.WriteLine();
                    indentedTextWriter.Indent = indentation;
                }
            });

            indentedTextWriter.Indent--;
            if(!string.IsNullOrEmpty(@namespace))
            {
                indentedTextWriter.WriteLine("}");
            }
            else
            {
                indentedTextWriter.Write("}");
            }
        }
    }
}
