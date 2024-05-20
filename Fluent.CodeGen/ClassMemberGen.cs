using Fluent.CodeGen.Consts;

namespace Fluent.CodeGen
{
    public abstract class ClassMemberGen<T> : CodeGen
        where T: ClassMemberGen<T>
    {
        public bool IsStatic { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public bool IsReadonly { get; private set; }
        public string AccessModifier { get; private set; }
        public string AssignedValue { get; private set; }

        public ClassMemberGen(string type, string name) 
        {
            Type = type;
            Name = name;
            AccessModifier = string.Empty;
        }

        public T Static()
        {
            IsStatic = true;
            return (T) this;
        }

        public T Readonly()
        {
            IsReadonly = true;
            return (T) this;
        }

        public T Public()
        {
            AccessModifier = AccessModifiers.Public;
            return (T) this;
        }

        public T Private()
        {
            AccessModifier = AccessModifiers.Private;
            return (T) this;
        }

        public T Protected()
        {
            AccessModifier = AccessModifiers.Protected;
            return (T) this;
        }

        public T Internal()
        {
            AccessModifier = AccessModifiers.Internal;
            return (T) this;
        }

        public T Assign(string value)
        {
            AssignedValue = value;
            return (T) this;
        }
    }
}
