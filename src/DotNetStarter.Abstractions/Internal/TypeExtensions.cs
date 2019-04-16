#pragma warning disable CS1591 // Missing XML comment for publicly visible type || member

namespace DotNetStarter.Abstractions.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static Assembly Assembly(this Type type)
        {
            return type.Assembly;
        }

        public static Type BaseType(this Type type)
        {
            return type.BaseType;
        }

        public static IEnumerable<ConstructorInfo> Constructors(this Type t)
        {
            return t.GetConstructors();
        }

        public static IEnumerable<Attribute> CustomAttribute(this Type type, bool inherit)
        {
            return type.GetCustomAttributes(inherit).OfType<Attribute>();
        }

        public static IEnumerable<Attribute> CustomAttribute(this Type t, Type attrType, bool inherit)
        {
            return t.GetCustomAttributes(attrType, inherit).OfType<Attribute>();
        }

        public static IEnumerable<Attribute> CustomAttribute(this Assembly assembly, Type attrType, bool inherit)
        {
            return assembly.GetCustomAttributes(attrType, inherit).OfType<Attribute>();
        }

        public static IEnumerable<Attribute> CustomAttribute(this object o, Type attrType, bool inherit)
        {
            if (o is Assembly)
                return CustomAttribute(o as Assembly, attrType, inherit);

            if (o is Type)
                return CustomAttribute(o as Type, attrType, inherit);

            throw new NotSupportedException("Object can only be Assembly || Type");
        }

        public static IEnumerable<Attribute> CustomAttributes(this Assembly assembly, bool inherit)
        {
            return assembly.GetCustomAttributes(inherit).OfType<Attribute>();
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type) => (BaseType(type) is null || BaseType(type) == typeof(object))
            ? Interfaces(type)
            : Enumerable
                .Repeat(BaseType(type), 1)
                .Concat(Interfaces(type))
                .Concat(BaseType(type).GetBaseTypes())
                .Distinct();

        public static IEnumerable<EventInfo> GetEventsCheck(this Type type)
        {
            return type.GetEvents();
        }

        public static IEnumerable<FieldInfo> GetFieldsCheck(this Type type)
        {
            return type.GetFields();
        }

        public static IEnumerable<MemberInfo> GetMembersCheck(this Type type)
        {
            return type.GetMembers();
        }

        public static IEnumerable<MethodInfo> GetMethodsCheck(this Type type)
        {
            return type.GetMethods();
        }

        public static IEnumerable<PropertyInfo> GetPropertiesCheck(this Type type)
        {
            return type.GetProperties();
        }

        public static Type GetTrueType(this Type type) => type.IsNullableType() ? Nullable.GetUnderlyingType(type) : type;

        public static IEnumerable<Type> GetTypesCheck(this Assembly assembly, bool exportedOnly = false)
        {
            if (exportedOnly)
                return assembly.GetExportedTypes();

            return assembly.GetTypes();
        }

        public static bool HasInterface(this Type t, Type interfaceType)
        {
            return t.GetInterfaces().Any(i => i == interfaceType);
        }

        public static IEnumerable<Type> Interfaces(this Type type)
        {
            return type.GetInterfaces();
        }

        public static bool IsAbstract(this Type type)
        {
            return type.IsAbstract;
        }

        public static bool IsAssignableFromCheck(this Type type, Type check)
        {
            return type.IsAssignableFrom(check);
        }

        public static bool IsEnumCheck(this Type type)
        {
            return type.IsEnum;
        }

        public static bool IsGenericInterface(this Type t, Type checkType)
        {
            return t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == checkType);
        }

        public static bool IsGenericType(this Type t)
        {
            return t.IsGenericType;
        }

        public static bool IsGenericTypeDefinitionCheck(this Type t)
        {
            return t.IsGenericTypeDefinition;
        }

        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        public static bool IsNullableType(this Type type)
        {
            if (type is null)
                return false;

            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }
    }
}