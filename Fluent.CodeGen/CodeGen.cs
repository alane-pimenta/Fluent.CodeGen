
using System.CodeDom.Compiler;
using System.IO;

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