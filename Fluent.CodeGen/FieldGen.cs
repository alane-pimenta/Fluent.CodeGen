using System.Collections.Generic;
using System.Text;

namespace Fluent.CodeGen
{
    public class FieldGen
    {
        private bool @static;
        private string type;
        private string accessModifier = "";
        private string name;
        private readonly List<string> attributesList = new List<string>();


        public FieldGen()
        {
            this.@static = false;
            this.type = null;
            this.accessModifier = "";
            this.name = null;
        }

        public FieldGen(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public FieldGen WithAttribute(string attribute)
        {
            attributesList.Add(attribute);
            return this;
        }

        public string GenerateCode()
        {
            var sb = new StringBuilder();

            foreach (var attribute in attributesList)
            {
                sb.AppendLine($"[{attribute}]");
            }

            sb.AppendLine($"{accessModifier}{type} {name};");

            return sb.ToString();
        }

    }
}

