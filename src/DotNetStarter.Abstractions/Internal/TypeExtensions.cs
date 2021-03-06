﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type || member

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
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().Assembly;
#else
            return type.Assembly;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static IEnumerable<ConstructorInfo> Constructors(this Type t)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().DeclaredConstructors;
#else
            return t.GetConstructors();
#endif
        }

        public static IEnumerable<Attribute> CustomAttribute(this Type type, bool inherit)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().GetCustomAttributes(inherit);
#else
            return type.GetCustomAttributes(inherit).OfType<Attribute>();
#endif
        }

        public static IEnumerable<Attribute> CustomAttribute(this Type t, Type attrType, bool inherit)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().GetCustomAttributes(attrType, inherit).OfType<Attribute>();
#else
            return t.GetCustomAttributes(attrType, inherit).OfType<Attribute>();
#endif
        }

        public static IEnumerable<Attribute> CustomAttribute(this Assembly assembly, Type attrType, bool inherit)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return assembly.GetCustomAttributes(attrType).OfType<Attribute>();
#else
            return assembly.GetCustomAttributes(attrType, inherit).OfType<Attribute>();
#endif
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
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return assembly.GetCustomAttributes();
#else
            return assembly.GetCustomAttributes(inherit).OfType<Attribute>();
#endif
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
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredEvents;
#else
            return type.GetEvents();
#endif
        }

        public static IEnumerable<FieldInfo> GetFieldsCheck(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredFields;
#else
            return type.GetFields();
#endif
        }

        public static IEnumerable<MemberInfo> GetMembersCheck(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredMembers;
#else
            return type.GetMembers();
#endif
        }

        public static IEnumerable<MethodInfo> GetMethodsCheck(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredMethods;
#else
            return type.GetMethods();
#endif
        }

        public static IEnumerable<PropertyInfo> GetPropertiesCheck(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredProperties;
#else
            return type.GetProperties();
#endif
        }

        public static Type GetTrueType(this Type type) => type.IsNullableType() ? Nullable.GetUnderlyingType(type) : type;

        public static IEnumerable<Type> GetTypesCheck(this Assembly assembly, bool exportedOnly = false)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            if (exportedOnly)
                return assembly.ExportedTypes;

            return assembly.DefinedTypes.Select(x => x.AsType());
#else
            if (exportedOnly)
                return assembly.GetExportedTypes();

            return assembly.GetTypes();
#endif
        }

        public static bool HasInterface(this Type t, Type interfaceType)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().ImplementedInterfaces.Any(i => i == interfaceType);
#else
            return t.GetInterfaces().Any(i => i == interfaceType);
#endif
        }

        public static IEnumerable<Type> Interfaces(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().ImplementedInterfaces;
#else
            return type.GetInterfaces();
#endif
        }

        public static bool IsAbstract(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsAbstract;
#else
            return type.IsAbstract;
#endif
        }

        public static bool IsAssignableFromCheck(this Type type, Type check)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsAssignableFrom(check.GetTypeInfo());
#else
            return type.IsAssignableFrom(check);
#endif
        }

        public static bool IsEnumCheck(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsGenericInterface(this Type t, Type checkType)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().ImplementedInterfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == checkType);
#else
            return t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == checkType);
#endif
        }

        public static bool IsGenericType(this Type t)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().IsGenericType;
#else
            return t.IsGenericType;
#endif
        }

        public static bool IsGenericTypeDefinitionCheck(this Type t)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return t.GetTypeInfo().IsGenericTypeDefinition;
#else
            return t.IsGenericTypeDefinition;
#endif
        }

        public static bool IsInterface(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static bool IsNullableType(this Type type)
        {
            if (type is null)
                return false;

#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
#else
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }
    }
}