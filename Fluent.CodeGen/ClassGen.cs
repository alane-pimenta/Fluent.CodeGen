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
        public List<MethodGen> Methods { get; private set; }
        private ConstructorGen constructor;
        private readonly IDictionary<string, FieldGen> fields;
        private readonly IDictionary<string, PropertyGen> properties;

        public string ClassName { get; private set; }
        public string AccessModifier { get; private set; }
        public string? Inherits { get; private set; }
        public bool IsStatic { get; private set; }
        public string? GetNamespace() => @namespace;
        public List<FieldGen> Fields => fields.Values.ToList();
        public List<PropertyGen> Properties => properties.Values.ToList();
        public ConstructorGen GetConstructor() => constructor;
        public HashSet<string> Implementations { get; private set; }
        public HashSet<string> Usings { get; private set; }
        private List<string> attributesList = new List<string>();



        public ClassGen(string name)
        {
            ClassName = name;
            AccessModifier = AccessModifiers.Default;
            Implementations = new HashSet<string>();
            Usings = new HashSet<string>();
            Methods = new List<MethodGen>();
            fields = new Dictionary<string, FieldGen>();
            properties = new Dictionary<string, PropertyGen>();
        }

        public ClassGen Using(params string[] namespaces)
        {
            foreach(var @namespace in namespaces)
            {
                this.Usings.Add(@namespace);
            }
            return this;
        }

        public ClassGen Implements(params string[] interfaceNames)
        {
            foreach (var interfaceName in interfaceNames)
            {
                Implementations.Add(interfaceName);
            }
            return this;
        }

        public ClassGen Public()
        {
            AccessModifier = AccessModifiers.Public;
            return this;
        }

        public ClassGen Internal()
        {
            AccessModifier = AccessModifiers.Internal;
            return this;
        }

        public ClassGen Extends(string className)
        {
            Inherits = className;
            return this;
        }
        
        public ClassGen Namespace(string @namespace)
        {
            this.@namespace = @namespace;
            return this;
        }

        public ClassGen Static()
        {
            IsStatic = true;
            return this;
        }

        public ClassGen WithMethod(MethodGen method)
        {
            Methods.Add(method);
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
            foreach (string @using in Usings)
            {
                indentedTextWriter.WriteLine($"using {@using};");
            }

            if(Usings.Any())
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

        public ClassGen WithAttribute(string attribute)
        {
            this.attributesList.Add(attribute);
            return this;
        }

        private void GenerateClassBody()
        {
            var classDeclaration = new StringBuilder();
            foreach (var attribute in attributesList) 
            {
                indentedTextWriter.WriteLine(attribute);
            }
            classDeclaration.Append(AccessModifier);
            if (!string.IsNullOrEmpty(AccessModifier))
            {
                classDeclaration.Append(' ');
            }
            
            if(IsStatic)
            {
                classDeclaration.Append("static ");
            }
            classDeclaration.Append($"class {ClassName}");

            if(Implementations.Count > 0 || !string.IsNullOrEmpty(Inherits))
            {
                classDeclaration.Append(" : ");
                if (!string.IsNullOrEmpty(Inherits))
                {
                    classDeclaration.Append(Inherits);
                }
                
                if(Implementations.Count > 0 && !string.IsNullOrEmpty(Inherits))
                {
                    classDeclaration.Append(", ");
                }

                if(Implementations.Count > 0)
                {
                    var implemented = string.Join(", ", Implementations.ToArray());
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
                if (Methods.Any())
                {
                    WriteNewLineNoIndentation();
                }
            }

            foreach (var method in Methods)
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
