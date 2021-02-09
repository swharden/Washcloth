# Washcloth ![](dev/icon/icon.png)
**Washcloth is a .NET Standard library for reading XML code comments from inside running applications.** Comments in source code are used to display tooltip information in Visual Studio and are saved as an XML file, but this documentation is not compiled into the program assembly so it cannot be read from within an application. Washcloth bridges this gap by reading the XML documentation file and combining its documentation with program information found at runtime using reflection.

**⚠️ Warning: Washcloth is early in its development.** Its API may change as it continues to evolve.

## Quickstart

### Sample Code with XML Comments
```cs
public class MathClass
{
    /// <summary>
    /// Calculate area of a circle
    /// </summary>
    /// <param name="radius">distance from center to an edge</param>
    /// <returns>area in original units squared</returns>
    public double CircleArea(double radius = 123)
    {
        return Math.PI * radius * radius;
    }
}
```

### Read XML Comments with Washcloth

```cs
MethodInfo mi = typeof(MathClass).GetMethod("CircleArea");
foreach (MethodInfo info in typeof(MathClass).GetMethods())
    Console.WriteLine(docs.GetXml(info));
```

```
<summary>
Calculate area of a circle
</summary>
<param name="radius">distance from center to an edge</param>
<returns>area in original units squared</returns>
```

## History

Washcloth is derived from [Towel](https://github.com/ZacharyPatten/Towel) by [Zachary Patten](https://github.com/ZacharyPatten), .NET library intended to add core functionality and make advanced topics as clean and simple as possible. Towel has tools for working with data structures, algorithms, mathematics, metadata, extensions, console, and more. Towel states it favors newer coding practices over maintaining backwards compatibility. At the time of writing Towel can only be used in .NET 5 and newer environments. 

Washcloth was born from the desire to use Towel's metadata tools for reflection and XML documentation in older environments. [Scott Harden](https://github.com/ZacharyPatten) isolated Towel's metadata tools and refactored the library to target .NET Standard 2.0, allowing it to be used in projects back to .NET Core 2.0 and .NET Framework 4.6.1. This new project was named Washcloth, because a washcloth is really just a small towel with less functionality.