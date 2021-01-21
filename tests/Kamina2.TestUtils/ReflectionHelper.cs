using System;
using System.Reflection;

namespace Kamina2.TestUtils
{
    /// <summary>
    /// Helper class containing methods related to <see cref="System.Reflection"/>.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets a custom attribute from a method without arguments.
        /// </summary>
        /// <typeparam name="TObject">The type of object to retrieve the attributes from.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="methodName">The name of the method of <typeparamref name="TObject"/> to
        ///  retrieve the attribute for.</param>
        /// <returns>The <typeparamref name="TAttribute"/>, <c>null</c> if the attribute was not found.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodName"/> is <c>null</c>, empty
        /// or consists of whitespace.</exception>
        public static TAttribute GetCustomAttribute<TObject, TAttribute>(string methodName)
            where TObject : class
            where TAttribute : Attribute
        {
            return GetCustomAttribute<TObject, TAttribute>(methodName, Type.EmptyTypes);
        }

        /// <summary>
        /// Gets a custom attribute from a method with arguments.
        /// </summary>
        /// <typeparam name="TObject">The type of object to retrieve the attributes from.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="methodName">The name of the method of <typeparamref name="TObject"/> to
        ///  retrieve the attribute for.</param>
        /// <param name="argumentTypes">The array of argument types that are passed in the method.</param>
        /// <returns>The <typeparamref name="TAttribute"/>, <c>null</c> if the attribute was not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="argumentTypes"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodName"/> is <c>null</c>, empty
        /// or consists of whitespace.</exception>
        public static TAttribute GetCustomAttribute<TObject, TAttribute>(string methodName, Type[] argumentTypes)
            where TObject : class
            where TAttribute : Attribute
        {
            if (argumentTypes == null)
            {
                throw new ArgumentNullException(nameof(argumentTypes));
            }
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(methodName));
            }
            return typeof(TObject).GetMethod(methodName, argumentTypes)?.GetCustomAttribute<TAttribute>();
        }
    }
}
