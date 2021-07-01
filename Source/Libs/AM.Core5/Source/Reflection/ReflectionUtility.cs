// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ReflectionUtility.cs -- полезные методы для работы с интроспекцией
 *  Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Reflection
{
    /// <summary>
    /// Полезные методы для работы с интроспекцией.
    /// </summary>
    public static class ReflectionUtility
    {
        #region Public methods

        /// <summary>
        /// Создание типизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<TObject, TValue> CreateGetter<TObject, TValue>
            (
                string propertyName
            )
        {
            const BindingFlags bindingFlags = BindingFlags.Instance
                                              | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = typeof(TObject).GetProperty(propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException($"Can't get property: {propertyName}");
            }

            return CreateGetter<TObject, TValue>(propertyInfo);
        }

        /// <summary>
        /// Создание типизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<TObject, TValue> CreateGetter<TObject, TValue>
            (
                PropertyInfo propertyInfo
            )
        {
            var method = propertyInfo.GetMethod;
            if (method is null)
            {
                throw new ArgumentException($"No method for {propertyInfo.Name}");
            }

            return method.CreateDelegate<Func<TObject, TValue>>();
        }

        /// <summary>
        /// Создание типизированного сеттера для указанного свойства объекта.
        /// </summary>
        public static Action<TObject, TValue> CreateSetter<TObject, TValue>
            (
                string propertyName
            )
        {
            const BindingFlags bindingFlags = BindingFlags.Instance
                                              | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = typeof(TObject).GetProperty(propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException($"Can't get property: {propertyName}");
            }

            return CreateSetter<TObject, TValue>(propertyInfo);
        }

        /// <summary>
        /// Создание типизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Action<TObject, TValue> CreateSetter<TObject, TValue>
            (
                PropertyInfo propertyInfo
            )
        {
            var method = propertyInfo.SetMethod;
            if (method is null)
            {
                throw new ArgumentException($"No method for {propertyInfo.Name}");
            }

            return method.CreateDelegate<Action<TObject, TValue>>();
        }

        /// <summary>
        /// Построение нетипизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<object, object?> CreateUntypedGetter
            (
                Type type,
                string propertyName
            )
        {
            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException($"Can't get property: {propertyName}");
            }

            return CreateUntypedGetter(propertyInfo);

        } // method CreateUntypedGetter

        /// <summary>
        /// Построение нетипизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<object, object?> CreateUntypedGetter
            (
                PropertyInfo propertyInfo
            )
        {
            var method = propertyInfo.GetMethod;
            if (method is null)
            {
                throw new ArgumentException($"No method for {propertyInfo.Name}");
            }

            // Конструируем лямбду
            // (object instance) => (object) method ((T) instance)

            var declaringType = propertyInfo.DeclaringType.ThrowIfNull (nameof (propertyInfo.DeclaringType));
            var instance = Expression.Parameter (typeof(object));
            var argument = Expression.Convert (instance, declaringType);
            var call = Expression.Convert
                (
                    Expression.Call (argument, method),
                    typeof(object)
                );
            var lambda = Expression.Lambda<Func<object, object?>> (call, instance);

            return lambda.Compile();

        } // method CreateUntypedGetter

        /// <summary>
        /// Построение сеттера для указанного свойства объекта.
        /// </summary>
        public static Action<object, object?> CreateUntypedSetter
            (
                Type type,
                string propertyName
            )
        {
            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException($"Can't get property: {propertyName}");
            }

            return CreateUntypedSetter(propertyInfo);

        } // method CreateUntypedSetter

        /// <summary>
        /// Построение нетипизированного сеттера для указанного свойства объекта.
        /// </summary>
        public static Action<object, object?> CreateUntypedSetter
            (
                PropertyInfo propertyInfo
            )
        {
            var method = propertyInfo.SetMethod;
            if (method is null)
            {
                throw new ArgumentException($"No method for {propertyInfo.Name}");
            }

            // Конструируем лямбду
            // method ((T1) instance, (T2) value)

            var declaringType = propertyInfo.DeclaringType.ThrowIfNull (nameof (propertyInfo.DeclaringType));
            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var convert1 = Expression.Convert(instance, declaringType);
            var convert2 = Expression.Convert(value, propertyInfo.PropertyType);
            var call = Expression.Call(convert1, method, convert2);

            var lambda = Expression.Lambda<Action<object, object?>> (call, instance, value);

            return lambda.Compile();

        } // method CreateUntypedSetter

        ///// <summary>
        ///// Получение списка всех типов, загруженных на данный момент
        ///// в текущий домен.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>Осторожно: могут быть загружены сборки только
        ///// для рефлексии. Типы в них непригодны для использования.
        ///// </remarks>
        //public static Type[] GetAllTypes()
        //{
        //    Assembly[] assemblies = AppDomain.CurrentDomain
        //        .GetAssemblies();
        //    List<Type> result = new List<Type>();
        //    foreach (Assembly assembly in assemblies)
        //    {
        //        result.AddRange(assembly.GetTypes());
        //    }

        //    return result.ToArray();
        //}

        /// <summary>
        /// Get the custom attribute.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                Type classType
            )
            where T : Attribute
        {
            var all = classType.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T?)all.FirstOrDefault();
        }

        /// <summary>
        /// Get the custom attribute.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                Type classType,
                bool inherit
            )
            where T : Attribute
        {
            var all = classType.GetCustomAttributes
                (
                    typeof(T),
                    inherit
                );

            return (T?) all.FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                MemberInfo member
            )
            where T : Attribute
        {
            var all = member.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T?)all.FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                FieldInfo fieldInfo
            )
            where T : Attribute
        {
            var all = fieldInfo.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T?)all.FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                PropertyInfo propertyInfo
            )
            where T : Attribute
        {
            var all = propertyInfo.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T?) all.FirstOrDefault();
        }

        ///// <summary>
        ///// Gets the custom attribute.
        ///// </summary>
        //[CanBeNull]
        //public static T GetCustomAttribute<T>
        //    (
        //        [NotNull] PropertyDescriptor propertyDescriptor
        //    )
        //    where T : Attribute
        //{
        //    return (T)propertyDescriptor.Attributes[typeof(T)];
        //}

        ///// <summary>
        ///// Get default constructor for given type.
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static ConstructorInfo GetDefaultConstructor
        //    (
        //        [NotNull] Type type
        //    )
        //{
        //    ConstructorInfo result = type.GetConstructor
        //        (
        //            BindingFlags.Instance | BindingFlags.Public,
        //            null,
        //            Type.EmptyTypes,
        //            null
        //        );
        //    return result;
        //}

        /// <summary>
        /// Get field value either public or private.
        /// </summary>
        public static object? GetFieldValue<T>
            (
                T target,
                string fieldName
            )
        {
            var fieldInfo = typeof(T).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (ReferenceEquals(fieldInfo, null))
            {
                Magna.Error
                    (
                        "ReflectionUtility::GeFieldValue: "
                        + "can't find field="
                        + fieldName
                    );

                throw new ArgumentException("fieldName");
            }

            return fieldInfo.GetValue(target);
        }

        /// <summary>
        /// Determines whether the specified type has the attribute.
        /// </summary>
        public static bool HasAttribute<T>
            (
                Type type,
                bool inherit
            )
            where T : Attribute
        {
            return !ReferenceEquals
                (
                    GetCustomAttribute<T>(type, inherit),
                    null
                );
        }

        /// <summary>
        /// Determines whether the specified member has the attribute.
        /// </summary>
        public static bool HasAttribute<T>
            (
                MemberInfo member
            )
            where T : Attribute
        {
            return !ReferenceEquals
                (
                    GetCustomAttribute<T>(member),
                    null
                );
        }

        /// <summary>
        /// Set field value either public or private.
        /// </summary>
        public static void SetFieldValue<TTarget, TValue>
            (
                TTarget target,
                string fieldName,
                TValue value
            )
            where TTarget : class
        {
            var fieldInfo = typeof(TTarget).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (ReferenceEquals(fieldInfo, null))
            {
                Magna.Error
                    (
                        "ReflectionUtility::SetFieldValue: "
                        + "can't find field="
                        + fieldName
                    );

                throw new ArgumentException("fieldName");
            }

            fieldInfo.SetValue(target, value);
        }

        /// <summary>
        /// Get property value either public or private.
        /// </summary>
        public static object? GetPropertyValue<T>
            (
                T target,
                string propertyName
            )
        {
            var propertyInfo = typeof(T).GetProperty
                (
                    propertyName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (ReferenceEquals(propertyInfo, null))
            {
                Magna.Error
                    (
                        "ReflectionUtility::GetPropertyValue: "
                        + "can't find property="
                        + propertyName
                    );

                throw new ArgumentException("propertyName");
            }

            return propertyInfo.GetValue(target, null);
        }

        /// <summary>
        /// Gets the properties and fields.
        /// </summary>
        public static PropertyOrField[] GetPropertiesAndFields
            (
                Type type,
                BindingFlags bindingFlags
            )
        {
            var result = new List<PropertyOrField>();
            foreach (PropertyInfo property in type.GetProperties(bindingFlags))
            {
                result.Add(new PropertyOrField(property));
            }

            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                result.Add(new PropertyOrField(field));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Set property value either public or private.
        /// </summary>
        public static void SetPropertyValue<TTarget, TValue>
            (
                TTarget target,
                string propertyName,
                TValue value
            )
        {
            var propertyInfo = typeof(TTarget).GetProperty
                (
                    propertyName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (ReferenceEquals(propertyInfo, null))
            {
                Magna.Error
                    (
                        "ReflectionUtility::SetPropertyValue: "
                        + "can't find property="
                        + propertyName
                    );

                throw new ArgumentException("propertyName");
            }

            propertyInfo.SetValue(target, value, null);
        }

        #endregion

    } // class ReflectionUtility

} // namespace AM.Reflection

