using System;
using System.Reflection;
using System.Xml.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Washcloth
{
    public class XmlDoc
    {
        public static string? GetText(ConstructorInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(PropertyInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(FieldInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(EventInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(MemberInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(ParameterInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(MethodInfo info) => Meta.GetDocumentation(info).xml;
        public static string? GetText(Type type) => Meta.GetDocumentation(type).xml;

        public static XElement GetMember(ConstructorInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(PropertyInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(FieldInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(EventInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(MemberInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(ParameterInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(MethodInfo info) => GetMemberElement(Meta.GetDocumentation(info));
        public static XElement GetMember(Type type) => GetMemberElement(Meta.GetDocumentation(type));

        public static string? GetMemberName(ConstructorInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(PropertyInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(FieldInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(EventInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(MemberInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(ParameterInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(MethodInfo info) => Meta.GetDocumentation(info).key;
        public static string? GetMemberName(Type type) => Meta.GetDocumentation(type).key;

        public static string? GetSummary(ConstructorInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(PropertyInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(FieldInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(EventInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(MemberInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(ParameterInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(MethodInfo info) => GetMemberSummary(GetMember(info));
        public static string? GetSummary(Type type) => GetMemberSummary(GetMember(type));

        private static XElement GetMemberElement((string key, string? docXml) docs)
        {
            string xml = docs.docXml ?? "";
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

        public static string GetSignature(MethodInfo info) => Signature.GetMethodSignature(info);

        public static string GetSignature(Type type) => Signature.GetTypeSignature(type);
    }
}
