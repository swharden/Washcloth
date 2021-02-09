using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Washcloth
{
    public class XmlDocumentation
    {
        public XmlDocumentation(string xmlFilePath)
        {

        }

        public string? GetXml(ConstructorInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(PropertyInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(FieldInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(EventInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(MemberInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(ParameterInfo info) => Meta.GetDocumentation(info);
        public string? GetXml(MethodInfo info) => Meta.GetDocumentation(info);
    }
}
