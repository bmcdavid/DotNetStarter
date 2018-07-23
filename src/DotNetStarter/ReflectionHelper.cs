namespace DotNetStarter
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Reflection tool to support both netframework and netstandard
    /// </summary>
    [Registration(typeof(IReflectionHelper), Lifecycle.Singleton)]
    public class ReflectionHelper : IReflectionHelper
    {
        /// <summary>
        /// Gets all constructors for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<ConstructorInfo> Constructors(Type type) => Abstractions.Internal.TypeExtensions.Constructors(type);

        /// <summary>
        /// Gets given types assembly
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Assembly GetAssembly(Type type) => Abstractions.Internal.TypeExtensions.Assembly(type);

        /// <summary>
        /// Gets a types base type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Type GetBaseType(Type type) => Abstractions.Internal.TypeExtensions.BaseType(type);

        /// <summary>
        /// Gets all types base implementations
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<Type> GetBaseTypes(Type type) => Abstractions.Internal.TypeExtensions.GetBaseTypes(type);

        /// <summary>
        /// Get custom attribute
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public virtual IEnumerable<Attribute> GetCustomAttribute(Assembly assembly, Type attrType, bool inherit) =>
            Abstractions.Internal.TypeExtensions.CustomAttribute(assembly, attrType, inherit);

        /// <summary>
        /// Get custom attribute if assigned
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public virtual IEnumerable<Attribute> GetCustomAttribute(Type type, Type attrType, bool inherit) =>
            Abstractions.Internal.TypeExtensions.CustomAttribute(type, attrType, inherit);

        /// <summary>
        /// Get custom attribute if assigned
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public virtual IEnumerable<Attribute> GetCustomAttributes(Assembly assembly, bool inherit) =>
            Abstractions.Internal.TypeExtensions.CustomAttributes(assembly, inherit);

        /// <summary>
        /// Gets all custom attributes
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public virtual IEnumerable<Attribute> GetCustomAttributes(Type type, bool inherit) =>
            Abstractions.Internal.TypeExtensions.CustomAttribute(type, inherit);

        /// <summary>
        /// Get types events
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<EventInfo> GetEvents(Type type) => Abstractions.Internal.TypeExtensions.GetEventsCheck(type);

        /// <summary>
        /// Get types fields
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<FieldInfo> GetFields(Type type) => Abstractions.Internal.TypeExtensions.GetFieldsCheck(type);

        /// <summary>
        /// Gets all interfaces for the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<Type> GetInterfaces(Type type) => Abstractions.Internal.TypeExtensions.Interfaces(type);

        /// <summary>
        /// Gets types members
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<MemberInfo> GetMembers(Type type) => Abstractions.Internal.TypeExtensions.GetMembersCheck(type);

        /// <summary>
        /// Get types methods
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<MethodInfo> GetMethods(Type type) => Abstractions.Internal.TypeExtensions.GetMethodsCheck(type);

        /// <summary>
        /// Get types properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<PropertyInfo> GetProperties(Type type) => Abstractions.Internal.TypeExtensions.GetPropertiesCheck(type);

        /// <summary>
        /// Gets true type, if nullable underlying type is returned
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Type GetTrueType(Type type) => Abstractions.Internal.TypeExtensions.GetTrueType(type);

        /// <summary>
        /// Gets all types for the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="exportedTypesOnly"></param>
        /// <returns></returns>
        public virtual IEnumerable<Type> GetTypes(Assembly assembly, bool exportedTypesOnly = false) =>
            Abstractions.Internal.TypeExtensions.GetTypesCheck(assembly, exportedTypesOnly);

        /// <summary>
        /// Determines if type has interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public virtual bool HasInterface(Type type, Type interfaceType) => Abstractions.Internal.TypeExtensions.HasInterface(type, interfaceType);

        /// <summary>
        /// Determines if type is abstract
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsAbstract(Type type) => Abstractions.Internal.TypeExtensions.IsAbstract(type);

        /// <summary>
        /// Determines if checkType is assignable from given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public virtual bool IsAssignableFrom(Type type, Type checkType) => Abstractions.Internal.TypeExtensions.IsAssignableFromCheck(type, checkType);

        /// <summary>
        /// Determines if type is an enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsEnum(Type type) => Abstractions.Internal.TypeExtensions.IsEnumCheck(type);

        /// <summary>
        /// Determines if type is a generic interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public virtual bool IsGenericInterface(Type type, Type checkType) => Abstractions.Internal.TypeExtensions.IsGenericInterface(type, checkType);

        /// <summary>
        /// Determines if type is generic
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsGenericType(Type type) => Abstractions.Internal.TypeExtensions.IsGenericType(type);

        /// <summary>
        /// Determines if type is interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsInterface(Type type) => Abstractions.Internal.TypeExtensions.IsInterface(type);

        /// <summary>
        /// Determines if type is nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsNullableType(Type type) => Abstractions.Internal.TypeExtensions.IsNullableType(type);

        /// <summary>
        /// Determines if type is a value type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsValueType(Type type) => Abstractions.Internal.TypeExtensions.IsValueType(type);
    }
}