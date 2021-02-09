using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Washcloth
{
    public class XmlDoc
    {
        public static string? GetXml(ConstructorInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(PropertyInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(FieldInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(EventInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(MemberInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(ParameterInfo info) => Meta.GetDocumentation(info);
        public static string? GetXml(MethodInfo info) => Meta.GetDocumentation(info);
    }
}
