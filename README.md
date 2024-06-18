# Fluent.CodeGen

This library provides a fluent api that is responsible for generating C# human readable source code. It can generate Class, Method, Constructor, Fields, Properties, Enum and so on.

<p align="center">
  <img src="resources/header.png">
</p>
<p align="center">
    <img alt="nuget" src="https://img.shields.io/nuget/dt/Fluent.CodeGen.svg">
    <a href="https://app.codacy.com?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade">
        <img src="https://app.codacy.com/project/badge/Grade/557bd5392ade4dcc89cd810df48ba103"/>
    </a>
    <img alt="nuget version" src="https://img.shields.io/nuget/v/Fluent.CodeGen.svg">
</p>


## Installation

In order to install the library you must run:

```sh
dotnet add package Fluent.CodeGen
```

## Examples

Many examples can be found on [unit tests](https://github.com/Alanep0922/Fluent.CodeGen/tree/main/Fluent.CodeGen.Tests)

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
