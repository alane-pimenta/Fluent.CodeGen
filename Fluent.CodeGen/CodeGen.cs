
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

namespace Fluent.CodeGen
{
    public abstract class CodeGen
    {
        protected readonly IndentedTextWriter indentedTextWriter;
        protected readonly StringWriter stringWriter;

        public CodeGen()
        {
            this.stringWriter = new StringWriter();
            this.indentedTextWriter = new IndentedTextWriter(stringWriter);
            this.indentedTextWriter.NewLine = "\n";
        }

        protected void WriteMultipleLines(string body)
        {
            var lines = body.Split("\n").ToList();
            var last = lines.Last();
            lines.ForEach(line => 
            {
                if(line.Equals(last) && string.IsNullOrEmpty(line))
                {
                    return;
                }
                if(string.IsNullOrWhiteSpace(line))
                {
                    var indentation = indentedTextWriter.Indent;
                    indentedTextWriter.Indent = 0;
                    indentedTextWriter.WriteLine();
                    indentedTextWriter.Indent = indentation;
                }
                else 
                {
                    indentedTextWriter.WriteLine(line);
                }
            });
        }

        public abstract string GenerateCode();

        public string GenerateCode(int indentation)
        {
            string generatedCode = string.Empty;
            int previousIndentation = this.indentedTextWriter.Indent;
            this.indentedTextWriter.Indent = indentation;
            generatedCode = GenerateCode();
            this.indentedTextWriter.Indent = previousIndentation;
            return generatedCode;
        }
    }
}