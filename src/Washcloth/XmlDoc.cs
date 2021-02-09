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

        public static string? GetSummary(Type type) => GetMemberSummary(GetMember(type));
        public static string? GetSummary(ConstructorInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(PropertyInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(FieldInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(EventInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(MemberInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(MethodInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(ParameterInfo info) => Meta.GetDocumentation(info).xml;

        public static string GetSignature(Type type) => Signature.GetTypeSignature(type);
        public static string GetSignature(MethodInfo info) => Signature.GetMethodSignature(info);

        private static XElement BuildMemberXml((string key, string? xml) docs)
        {
            string xml = docs.xml ?? "";
            string memberXml = $"<member name='{docs.key}'>{xml}</member>";
            return XElement.Parse(memberXml);
        }

        private static string? GetMemberSummary(XElement memberElement)
        {
            var summaryElement = memberElement.Element("summary");
            if (summaryElement is null)
                return null;

            string summary = summaryElement.Value;
            summary = summary.Replace('\n', ' ');
            summary = summary.Replace('\r', ' ');
            while (summary.Contains("  "))
                summary = summary.Replace("  ", " ");
            return summary.Trim();
        }
    }
}
