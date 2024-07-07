using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public class MethodGen
    {
        private readonly string type;
        private readonly string name;
        private readonly List<string> parameters;
        private string modifier;
        private string body;
        private bool isStatic;
        private readonly IndentedTextWriter indentedTextWriter;
        private readonly StringWriter stringWriter;

        public MethodGen(string type, string name)
        {
            this.type = type;
            this.name = name;
            this.parameters = new List<string>();
            this.modifier = AccessModifiers.Default;
            this.stringWriter = new StringWriter();
            this.indentedTextWriter = new IndentedTextWriter(stringWriter);
        }

        public MethodGen Public()
        {
            modifier = AccessModifiers.Public;
            return this;
        }
        public MethodGen Private()
        {
            modifier = AccessModifiers.Private;
            return this;
        }

        public MethodGen Internal()
        {
            modifier = AccessModifiers.Internal;
            return this;
        }
        public MethodGen Protected()
        {
            modifier = AccessModifiers.Protected;
            return this;
        }

        public MethodGen Static()
        {
            isStatic = true;
            return this;
        }

        public MethodGen WithParameter(string parameterType, string parameterName)
        {
            parameters.Add($"{parameterType} {parameterName}");
            return this;
        }

        public MethodGen WithBody(string methodBody)
        {
            body = methodBody;
            return this;
        }

        public string GenerateCode()
        {
            indentedTextWriter.Write(modifier);
            if (modifier != AccessModifiers.Default)
            {
                indentedTextWriter.Write(" ");
            }
            if (isStatic)
            {
                indentedTextWriter.Write("static ");
            }
            indentedTextWriter.Write(type);
            indentedTextWriter.Write(" ");
            indentedTextWriter.Write(name);
            indentedTextWriter.Write("(");
            indentedTextWriter.Write(string.Join(", ", parameters));
            indentedTextWriter.WriteLine(")");
            indentedTextWriter.WriteLine("{");
            indentedTextWriter.Indent++;
            indentedTextWriter.WriteLine(body);
            indentedTextWriter.Indent--;
            indentedTextWriter.WriteLine("}");

            string generatedCode = stringWriter.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(body))
            {
                List<string> generatedCodeLines = ConvertStringToList(generatedCode);

                foreach (string lineItem in generatedCodeLines)
                {
                    WriteOnline(lineItem);
                }
            }

            return generatedCode;
        }
        private List<string> ConvertStringToList(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            return new List<string>(lines);
        }

        private void WriteOnline(string generatedCode)
        {

            Console.WriteLine(generatedCode);
        }
    }

}



