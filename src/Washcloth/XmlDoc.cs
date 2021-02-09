﻿using System;
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
        public static string? GetXml(Type type) => Meta.GetDocumentation(type);


        /*
        public static XDocument GetMember(ConstructorInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(PropertyInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(FieldInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(EventInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(MemberInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(ParameterInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(MethodInfo info) => XDocument.Parse(Meta.GetDocumentation(info));
        public static XDocument GetMember(Type type) => XDocument.Parse(Meta.GetDocumentation(type));
        */

        public static string? GetSummary(ConstructorInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(PropertyInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(FieldInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(EventInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(MemberInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(ParameterInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(MethodInfo info) => GetSummary(GetXml(info));
        public static string? GetSummary(Type type) => GetSummary(GetXml(type));

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

        public static string GetSignature(MethodInfo info) => Signature.GetMethodSignature(info);

        public static string GetSignature(Type type) => Signature.GetTypeSignature(type);
    }
}
