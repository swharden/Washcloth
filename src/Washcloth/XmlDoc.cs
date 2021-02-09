using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

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

        // TODO: this should be wrapped in <member> with the original name included
        public static XDocument GetXDocument(ConstructorInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(PropertyInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(FieldInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(EventInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(MemberInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(ParameterInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetXDocument(MethodInfo info) => XDocument.Parse(Meta.GetDocumentation(info));


        public static string? GetSummary(ConstructorInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(PropertyInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(FieldInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(EventInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(MemberInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(ParameterInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(MethodInfo info) => GetSummary(GetXml(info));

        private static string? GetSummary(string? xml)
        {
            if (xml is null)
                return null;

            // TODO: read this from inside towel and include the name/key
            if (!xml.StartsWith("<member>"))
                xml = $"<member>{xml}</member>";

            var summaryElement = XDocument.Parse(xml).Element("member").Element("summary");
            if (summaryElement is null)
                return null;

            // collapse multi-line summaries into a single line
            string summary = summaryElement.Value;
            summary = summary.Replace('\n', ' ');
            summary = summary.Replace('\r', ' ');
            while (summary.Contains("  "))
                summary = summary.Replace("  ", " ");
            return summary.Trim();
        }
    }
}
