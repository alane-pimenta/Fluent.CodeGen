namespace Fluent.CodeGen
{
    public class FieldGen : ClassMemberGen<FieldGen>
    {
        public FieldGen(string type, string name) : base(type, name)
        {
        }

        public override string GenerateCode()
        {
            if(!string.IsNullOrEmpty(AccessModifier))
            {
                indentedTextWriter.Write($"{AccessModifier} ");
            }

            if(IsStatic)
            {
                indentedTextWriter.Write("static ");
            }

            if(IsReadonly)
            {
                indentedTextWriter.Write("readonly ");
            }

            indentedTextWriter.Write($"{Type} {Name}");

            if(!string.IsNullOrWhiteSpace(AssignedValue))
            {
                indentedTextWriter.Write($" = {AssignedValue}");
            }

            indentedTextWriter.Write($";");

            return stringWriter.ToString();
        }
    }
}
