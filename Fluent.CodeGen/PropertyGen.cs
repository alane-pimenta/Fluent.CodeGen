namespace Fluent.CodeGen
{
    public class PropertyGen : ClassMemberGen<PropertyGen>
    {
        public bool HasGet { get; private set; } = true;
        public string GetAccessModifier { get; private set; } = string.Empty;
        public string GetBody {  get; private set; } = string.Empty;

        public bool HasSet { get; private set; } = true;
        public string SetAccessModifier { get; private set; } = string.Empty;
        public string SetBody { get; private set; } = string.Empty;


        public PropertyGen(string type, string name) : base(type, name)
        {
        }


        public PropertyGen Get(string accessModifier, string body)
        {
            HasGet = true;
            GetAccessModifier = accessModifier;
            GetBody = body;
            return this;
        }

        public PropertyGen Get(string body)
        {
            HasGet = true;
            GetBody = body;
            return this;
        }

        public PropertyGen NoGet()
        {
            HasGet = false;
            return this;
        }

        public PropertyGen Set(string accessModifier, string body)
        {
            HasSet = true;
            SetAccessModifier = accessModifier;
            SetBody = body;
            return this;
        }

        public PropertyGen Set(string body)
        {
            HasSet = true;
            SetBody = body;
            return this;
        }

        public PropertyGen NoSet()
        {
            HasSet = false;
            return this;
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

            var getBodyMultipleLines = GetBody.Contains("\n");
            var setBodyMultipleLines = SetBody.Contains("\n");

            if (HasGet || HasSet)
            {
                if (getBodyMultipleLines || setBodyMultipleLines)
                {
                    indentedTextWriter.WriteLine();
                    indentedTextWriter.WriteLine("{");
                    indentedTextWriter.Indent++;
                }
                else
                {
                    indentedTextWriter.Write(" {");
                }
            }

            if (HasGet)
            {
                if (!getBodyMultipleLines)
                {
                    indentedTextWriter.Write(' ');
                }

                BuildGet();

                if (!HasSet && !getBodyMultipleLines)
                {
                    indentedTextWriter.Write(' ');
                }
            }

            if (HasSet)
            {
                if (!getBodyMultipleLines && !setBodyMultipleLines)
                {
                    indentedTextWriter.Write(' ');
                }

                BuildSet();

                if (!getBodyMultipleLines && !setBodyMultipleLines)
                {
                    indentedTextWriter.Write(' ');
                }
                else if(getBodyMultipleLines && !setBodyMultipleLines)
                {
                    indentedTextWriter.WriteLine();
                }
            }

            if (HasGet || HasSet)
            {
                if (getBodyMultipleLines || setBodyMultipleLines)
                {
                    indentedTextWriter.Indent--;
                }

                indentedTextWriter.Write("}");
            }
            else
            {
                indentedTextWriter.Write(";");
            }

            if (!string.IsNullOrWhiteSpace(AssignedValue))
            {
                indentedTextWriter.Write($" = {AssignedValue};");
            }



            return stringWriter.ToString();
        }

        private void BuildGet()
        {
            BuildProperty("get", GetAccessModifier, GetBody);
        }

        private void BuildSet()
        {
            BuildProperty("set", SetAccessModifier, SetBody);
        }

        private void BuildProperty(string propertyName, string propertyAccessorModifier, string body)
        {
            if(string.IsNullOrEmpty(propertyAccessorModifier))
            {
                indentedTextWriter.Write(propertyName);
            }
            else
            {
                indentedTextWriter.Write($"{propertyAccessorModifier} {propertyName}");
            }

            if(string.IsNullOrEmpty(body))
            {
                indentedTextWriter.Write($";");
            }
            else
            {
                if (body.Contains("\n"))
                {
                    indentedTextWriter.WriteLine();
                    indentedTextWriter.WriteLine("{");
                    indentedTextWriter.Indent++;
                    WriteMultipleLines(body);
                    indentedTextWriter.Indent--;
                    indentedTextWriter.WriteLine("}");
                }
                else
                {
                    indentedTextWriter.Write($" => {body};");
                }
            }
        }
    }
}
