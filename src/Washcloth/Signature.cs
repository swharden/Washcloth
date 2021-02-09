using System;
using System.Collections.Generic;

using System.Text;
using System.Linq;
using System.Reflection;

/* Adapted from https://github.com/kellyelton/System.Reflection.ExtensionMethods
 * Under the Mozilla Public License V2.0 http://mozilla.org/MPL/2.0/
 */

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Washcloth
{
    public static class Signature
    {
        public static string GetMethodSignature(MethodInfo method)
        {
            var signatureBuilder = new StringBuilder();

            signatureBuilder.Append(GetMethodModifierSignature(method));
            signatureBuilder.Append(" ");

            signatureBuilder.Append(GetTypeSignature(method.ReturnType));
            signatureBuilder.Append(" ");

            // Add method name
            signatureBuilder.Append(method.Name);

            // Add method generics
            if (method.IsGenericMethod)
            {
                signatureBuilder.Append(GetGenericSignature(method));
            }

            // Add method parameters
            signatureBuilder.Append(GetMethodArgumentsSignature(method, invokable: false));

            return signatureBuilder.ToString();
        }

        public static string GetMethodModifierSignature(MethodInfo method)
        {
            string modifier = "";
            if (method.IsAssembly)
                modifier = method.IsFamily ? "internal protected" : "internal";
            else if (method.IsPublic)
                modifier = "public";
            else if (method.IsPrivate)
                modifier = "private";
            else if (method.IsFamily)
                modifier = "protected";

            if (method.IsStatic)
                modifier += " static";

            return modifier;
        }

        public static string GetTypeModifierSignature(Type type)
        {
            string modifier = type.IsPublic ? "public" : "private";

            if (type.IsAbstract)
                modifier += " abstract";

            return modifier;
        }

        public static string GetGenericSignature(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsGenericMethod) throw new ArgumentException($"{method.Name} is not generic.");

            return BuildGenericSignature(method.GetGenericArguments());
        }

        public static string GetMethodArgumentsSignature(MethodInfo method, bool invokable)
        {
            var isExtensionMethod = method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false);
            var methodParameters = method.GetParameters().AsEnumerable();

            // If this signature is designed to be invoked and it's an extension method
            if (isExtensionMethod && invokable)
            {
                // Skip the first argument
                methodParameters = methodParameters.Skip(1);
            }

            var methodParameterSignatures = methodParameters.Select(param =>
            {
                var signature = string.Empty;

                if (param.ParameterType.IsByRef)
                    signature = "ref ";
                else if (param.IsOut)
                    signature = "out ";
                else if (isExtensionMethod && param.Position == 0)
                    signature = "this ";

                if (!invokable)
                {
                    signature += GetTypeSignature(param.ParameterType) + " ";
                }

                signature += param.Name;

                if (param.HasDefaultValue)
                    signature += " = " + param.DefaultValue.ToString();

                return signature;
            });

            var methodParameterString = "(" + string.Join(", ", methodParameterSignatures) + ")";

            return methodParameterString;
        }

        /// <summary>
        /// Get a fully qualified signature for <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type. May be generic or <see cref="Nullable{T}"/></param>
        /// <returns>Fully qualified signature</returns>
        public static string GetTypeSignature(Type type)
        {
            var isNullableType = IsNullable(type, out var underlyingNullableType);

            var signatureType = isNullableType
                ? underlyingNullableType
                : type;

            var isGenericType = IsGeneric(signatureType);

            var signature = GetQualifiedTypeName(signatureType);

            if (isGenericType)
            {
                // Add the generic arguments
                signature += BuildGenericSignature(signatureType.GetGenericArguments());
            }

            if (isNullableType)
            {
                signature += "?";
            }

            bool useKeywords = true;
            if (useKeywords)
            {
                var namedTypes = new (Type type, string prettyName)[]
                {
                    (typeof(bool), "bool"),
                    (typeof(byte), "byte"),
                    (typeof(char), "char"),
                    (typeof(double), "double"),
                    (typeof(decimal), "decimal"),
                    (typeof(float), "float"),
                    (typeof(int), "int"),
                    (typeof(long), "long"),
                    (typeof(sbyte), "sbyte"),
                    (typeof(short), "short"),
                    (typeof(string), "string"),
                    (typeof(uint), "uint"),
                    (typeof(ulong), "ulong"),
                    (typeof(ushort), "ushort"),
                };

                foreach (var namedType in namedTypes)
                {
                    if (signature == namedType.type.FullName)
                    {
                        signature = namedType.prettyName;
                        break;
                    }
                }
            }

            //string modifier = GetTypeModifierSignature(type);
            //return modifier + " " + signature;

            return signature;
        }

        /// <summary>
        /// Takes an <see cref="IEnumerable{T}"/> and creates a generic type signature (&lt;string, string&gt; for example)
        /// </summary>
        /// <param name="genericArgumentTypes"></param>
        /// <returns>Generic type signature like &lt;Type, ...&gt;</returns>
        public static string BuildGenericSignature(IEnumerable<Type> genericArgumentTypes)
        {
            var argumentSignatures = genericArgumentTypes.Select(GetTypeSignature);

            return "<" + string.Join(", ", argumentSignatures) + ">";
        }

        public static bool IsNullable(Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null;
        }

        /// <summary>
        /// Is this type a generic type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if generic, otherwise False</returns>
        public static bool IsGeneric(Type type)
        {
            return type.IsGenericType
                && type.Name.Contains("`");
            //TODO: Figure out why IsGenericType isn't good enough and document (or remove) this condition
        }

        /// <summary>
        /// Gets the fully qualified type name of <paramref name="type"/>.
        /// This will use any keywords in place of types where possible (string instead of System.String for example)
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The fully qualified name for <paramref name="type"/></returns>
        public static string GetQualifiedTypeName(Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return "string";
                case "Int32":
                    return "int";
                case "Decimal":
                    return "decimal";
                case "Object":
                    return "object";
                case "Void":
                    return "void";
                case "Boolean":
                    return "bool";
            }

            //TODO: Figure out how type.FullName could be null and document (or remove) this conditional
            var signature = string.IsNullOrWhiteSpace(type.FullName)
                ? type.Name
                : type.FullName;

            if (IsGeneric(type))
                signature = RemoveGenericTypeNameArgumentCount(signature);

            return signature;
        }


        /// <summary>
        /// This removes the `{argumentcount} from a the signature of a generic type
        /// </summary>
        /// <param name="genericTypeSignature">Signature of a generic type</param>
        /// <returns><paramref name="genericTypeSignature"/> without any argument count</returns>
        public static string RemoveGenericTypeNameArgumentCount(string genericTypeSignature)
        {
            return genericTypeSignature.Substring(0, genericTypeSignature.IndexOf('`'));
        }
    }
}
