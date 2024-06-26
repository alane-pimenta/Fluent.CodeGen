using System.Text;
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
        private readonly HashSet<string> implements;
        private readonly HashSet<string> namespaces;
        private string accessModifier = AccessModifiers.Default;

        private readonly IDictionary<string, MethodGen> methods;
        private ConstructorGen constructor;
        private readonly IDictionary<string, FieldGen> fields;
        private readonly IDictionary<string, PropertyGen> properties;

        public ClassGen(string name)
        {
            ClassName = name;
            implements = new HashSet<string>();
            namespaces = new HashSet<string>();
            methods = new Dictionary<string, MethodGen>();
            fields = new Dictionary<string, FieldGen>();
            properties = new Dictionary<string, PropertyGen>();
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
            methods[method.Name] = method;
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
            fields[fieldGen.Name] = fieldGen;
            return this;
        }

        public ClassGen WithProperty(PropertyGen propertyGen)
        {
            properties[propertyGen.Name] = propertyGen;
            return this;
        }

        public override string GenerateCode()
        {
            Flush();
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

            foreach(var field in fields.Values)
            {
                WriteMultipleLines(field.GenerateCode());
            }

            if(properties.Any() && fields.Any())
            {
                WriteNewLineNoIndentation();
            }

            foreach (var property in properties.Values)
            {
                WriteMultipleLines(property.GenerateCode());
            }

            if (constructor is not null)
            {
                if(fields.Any() || properties.Any())
                {
                    WriteNewLineNoIndentation();
                }
                WriteMultipleLines(constructor.GenerateCode());
                if (methods.Any())
                {
                    WriteNewLineNoIndentation();
                }
            }

            foreach (var method in methods.Values)
            {
                WriteMultipleLines(method.GenerateCode());
            }

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
