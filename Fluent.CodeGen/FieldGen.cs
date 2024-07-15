using System.Collections.Generic;

namespace Fluent.CodeGen
{
    public class FieldGen : ClassMemberGen<FieldGen>
    {
        private readonly List<string> attributesList = new List<string>();

        public FieldGen(string type, string name) : base(type, name)
        {
        }

        public void AddAttribute(string attribute)
        {
            attributesList.Add(attribute);
        }

        public override string GenerateCode()
        {

            Flush();
            if (!string.IsNullOrEmpty(AccessModifier))
            {
                indentedTextWriter.Write($"{AccessModifier} ");
            }

            if (IsStatic)
            {
                indentedTextWriter.Write("static ");
            }

            if (IsReadonly)
            {
                indentedTextWriter.Write("readonly ");
            }

            indentedTextWriter.Write($"{Type} {Name}");

            if (!string.IsNullOrWhiteSpace(AssignedValue))
            {
                indentedTextWriter.Write($" = {AssignedValue}");
            }

            foreach (var attribute in attributesList)
            {
                indentedTextWriter.Write($" {attribute}");
            }

            indentedTextWriter.Write($";");

            return stringWriter.ToString();
        }
    }
}
