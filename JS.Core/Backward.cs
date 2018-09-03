using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace NiL.JS.Backward
{
    internal static class Backward
    {
        private static readonly Type[] Types =
        {
            null,
            typeof(object),
            Type.GetType("System.DBNull"),
            typeof(bool),
            typeof(char),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            null,
            typeof(string)
        };

        internal static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> self)
        {
            return new ReadOnlyCollection<T>(self);
        }

        internal static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> self)
        {
            return new ReadOnlyCollection<T>(self);
        }

        internal static bool IsAssignableFrom(this Type self, Type sourceType)
        {
            return self.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo());
        }

        internal static bool IsSubclassOf(this Type self, Type sourceType)
        {
            return self != sourceType && self.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo());
        }

        internal static bool IsDefined(this Type self, Type attributeType, bool inherit)
        {
            return self.GetTypeInfo().IsDefined(attributeType, inherit);
        }

        internal static MemberTypes GetMemberType(this MemberInfo self)
        {
            if (self is ConstructorInfo)
                return MemberTypes.Constructor;
            if (self is EventInfo)
                return MemberTypes.Event;
            if (self is FieldInfo)
                return MemberTypes.Field;
            if (self is MethodInfo)
                return MemberTypes.Method;
            if (self is TypeInfo)
                return MemberTypes.TypeInfo;
            if (self is PropertyInfo)
                return MemberTypes.Property;
            return MemberTypes.Custom; // чёт своё, пускай сами разбираются
        }

        internal static MethodInfo GetGetMethod(this PropertyInfo self, bool fictive)
        {
            return self.GetMethod;
        }

        internal static MethodInfo GetSetMethod(this PropertyInfo self, bool fictive)
        {
            return self.SetMethod;
        }

        internal static MethodInfo GetAddMethod(this EventInfo self, bool fictive)
        {
            return self.AddMethod;
        }

        internal static MethodInfo GetGetMethod(this PropertyInfo self)
        {
            return self.GetMethod;
        }

        internal static MethodInfo GetSetMethod(this PropertyInfo self)
        {
            return self.SetMethod;
        }

        internal static MethodInfo GetAddMethod(this EventInfo self)
        {
            return self.AddMethod;
        }

        internal static Type GetInterface(this Type type, string name)
        {
            return type.GetTypeInfo().ImplementedInterfaces.First(x => x.Name == name);
        }

        internal static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }

        internal static MethodInfo GetMethod(this Type type, string name, Type[] parameters)
        {
            return type.GetRuntimeMethod(name, parameters);
        }

        internal static TypeCode GetTypeCode(this Type type)
        {
            if (type == null)
                return TypeCode.Empty;

            if (type.GetTypeInfo().IsClass)
            {
                if (type == Types[2])
                    return TypeCode.DBNull;

                if (type == typeof(string))
                    return TypeCode.String;

                return TypeCode.Object;
            }

            for (var i = 3; i < Types.Length; i++)
                if (Types[i] == type)
                    return (TypeCode) i;

            return TypeCode.Object;
        }
    }
}