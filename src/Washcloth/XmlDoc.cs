using System;
using System.Reflection;
using System.Xml.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Washcloth
{
    public class XmlDoc
    {
        public static XElement GetMember(Type type) => BuildMemberXml(Meta.GetDocumentation(type));
        public static XElement GetMember(ConstructorInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetMember(PropertyInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetMember(FieldInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetMember(EventInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetMember(MemberInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetMember(MethodInfo info) => BuildMemberXml(Meta.GetDocumentation(info));
        public static XElement GetParam(ParameterInfo info) => XElement.Parse(GetSummary(info));

        public static string? GetSummary(Type type, bool collapse = true) =>
            FormatSummary(GetMember(type).Element("summary")?.Value, collapse);

        public static string? GetSummary(ConstructorInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse);

        public static string? GetSummary(PropertyInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse);

        public static string? GetSummary(FieldInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse);

        public static string? GetSummary(EventInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse)
            ;
        public static string? GetSummary(MemberInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse);

        public static string? GetSummary(MethodInfo info, bool collapse = true) =>
            FormatSummary(GetMember(info).Element("summary")?.Value, collapse);

        public static string? GetSummary(ParameterInfo info, bool collapse = true) =>
            FormatSummary(XElement.Parse(Meta.GetDocumentation(info).xml)?.Value, collapse);

        private static string? FormatSummary(string? xml, bool collapse)
        {
            if (xml is null)
                return null;

            if (collapse == false)
                return xml;

            xml = xml.Replace('\n', ' ');
            xml = xml.Replace('\r', ' ');
            while (xml.Contains("  "))
                xml = xml.Replace("  ", " ");
            return xml.Trim();
        }

        private static XElement BuildMemberXml((string key, string? xml) docs)
        {
            string xml = docs.xml ?? "";
            string memberXml = $"<member name='{docs.key}'>{xml}</member>";
            return XElement.Parse(memberXml);
        }
    }
}
