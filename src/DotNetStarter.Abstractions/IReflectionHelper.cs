namespace DotNetStarter.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides reflection tools across .netframework and .netcore
    /// </summary>
    public interface IReflectionHelper
    {
        /// <summary>
        /// Gets a types constructors
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ConstructorInfo> Constructors(Type type);

        /// <summary>
        /// Gets assembly from given types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Assembly GetAssembly(Type type);

        /// <summary>
        /// Gets a types custom attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        IEnumerable<Attribute> GetCustomAttribute(Type type, Type attrType, bool inherit);

        /// <summary>
        /// Gets an assemblies custom attribute
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        IEnumerable<Attribute> GetCustomAttribute(Assembly assembly, Type attrType, bool inherit);

        /// <summary>
        /// Gets all custom attributes for an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        IEnumerable<Attribute> GetCustomAttributes(Assembly assembly, bool inherit);

        /// <summary>
        /// Gets the interfaces implemented for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetInterfaces(Type type);

        /// <summary>
        /// Gets types from an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="exportedTypesOnly"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypes(Assembly assembly, bool exportedTypesOnly = false);

        /// <summary>
        /// Determines if a type implements an itnerface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        bool HasInterface(Type type, Type interfaceType);

        /// <summary>
        /// Determines if type is an abstract class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsAbstract(Type type);

        /// <summary>
        /// Determines if checkType is assignable from given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        bool IsAssignableFrom(Type type, Type checkType);

        /// <summary>
        /// Determines if type is an enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsEnum(Type type);

        /// <summary>
        /// Determines if a type is a generic interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        bool IsGenericInterface(Type type, Type checkType);

        /// <summary>
        /// Determines if type is a genreric
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsGenericType(Type type);

        /// <summary>
        /// Determines if type is an interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsInterface(Type type);

        /// <summary>
        /// Determines if type is Nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsNullableType(Type type);

        /// <summary>
        /// Gets real type, so if nullable, underlying type is returned
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetTrueType(Type type);

        /// <summary>
        /// Gets a types base type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetBaseType(Type type);

        /// <summary>
        /// Gets all types base implementations
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetBaseTypes(Type type);
    }
}