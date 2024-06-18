# Fluent.CodeGen

This library provides a fluent api that is responsible for generating C# source code. It can generate Class, Method, Constructor, Fields, Properties, Enum and so on.

Many examples can be found on [unit tests](https://github.com/Alanep0922/Fluent.CodeGen/tree/main/Fluent.CodeGen.Tests)

Example:

```csharp
var fieldTest = new FieldGen("string", "test")
    .Public()
    .Static();

var fieldAmount = new FieldGen("int", "amount")
    .Assign("10");

var classGen = new ClassGen(name: "Program");

var generatedCode = classGen
    .Using("System")
    .Namespace("My.Test")
    .Public()
    .WithField(fieldTest)
    .WithField(fieldAmount)
    .Constructor(ctor => ctor.Public())
    .GenerateCode();
```

The generated code will be:

```csharp
using System;

namespace My.Test
{
    public class Program
    {
        public static string test;
        int amount = 10;

        public Program()
        {
        }

    }
}
```
