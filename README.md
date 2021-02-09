# Washcloth ![](dev/icon/icon.png)

[![Build Status](https://dev.azure.com/swharden/swharden/_apis/build/status/swharden.Washcloth?branchName=main)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=16&branchName=main)
[![](https://img.shields.io/nuget/v/Washcloth?label=NuGet&logo=nuget)](https://www.nuget.org/packages/Washcloth/)

**Washcloth is a .NET Standard library for reading XML code comments from inside running applications.** Comments in source code are used to display tooltip information in Visual Studio and can be automatically saved in XML files, but this documentation is not added into program assemblies so it is not available in compiled applications. Washcloth bridges this gap by reading the XML documentation file and combining its comments with program information found at runtime using reflection.

**⚠️ Warning: Washcloth is early in its development.** Its API may change as it continues to evolve.

## Quickstart

### Get XML Documentation

```cs
MethodInfo info = typeof(MathClass).GetMethod("CircleArea");
XElement xml = Washcloth.XmlDoc.GetMember(info);
Console.WriteLine(xml);
```


```
<member name="M:Washcloth_Testing.MathClass.CircleArea(System.Double)">
  <summary>
    Calculate area of a circle
  </summary>
  <param name="radius">distance from center to an edge</param>
  <returns>area in original units squared</returns>
</member>
```

### Get Signature

```cs
MethodInfo info = typeof(MathClass).GetMethod("CircleArea");
string signature = Washcloth.XmlDoc.GetSignature(info);
Console.WriteLine(signature);
```

```
public double CircleArea(double radius = 123)
```

## History

Washcloth is derived from [Towel](https://github.com/ZacharyPatten/Towel) by [Zachary Patten](https://github.com/ZacharyPatten), a .NET library intended to add core functionality and make advanced topics as clean and simple as possible. Towel has tools for working with data structures, algorithms, mathematics, metadata, extensions, console, and more. Towel favors newer coding practices over maintaining backwards compatibility, and at the time of writing Towel only supports .NET 5 and newer.

Washcloth was born from the desire to use Towel's metadata tools for reflection and XML documentation in legacy environments like .NET Framework. [Scott Harden](https://github.com/ZacharyPatten) isolated Towel's metadata tools and refactored the library to target .NET Standard 2.0, allowing it to be used in projects back to .NET Core 2.0 and .NET Framework 4.6.1. This new project was named Washcloth, because a washcloth is really just a small towel with less functionality.