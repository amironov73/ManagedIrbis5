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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

using AM.Core.Properties;

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
            Sure.NotNullNorEmpty (propertyName);

            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = typeof (TObject).GetProperty (propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException (string.Format (Resources.Can_t_get_property, propertyName));
            }

            return CreateGetter<TObject, TValue> (propertyInfo);
        }

        /// <summary>
        /// Создание типизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<TObject, TValue> CreateGetter<TObject, TValue>
            (
                this PropertyInfo propertyInfo
            )
        {
            Sure.NotNull (propertyInfo);

            var method = propertyInfo.GetMethod;
            if (method is null)
            {
                throw new ArgumentException (string.Format (Resources.No_method_for, propertyInfo.Name));
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
            Sure.NotNullNorEmpty (propertyName);

            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = typeof (TObject).GetProperty (propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException (string.Format (Resources.Can_t_get_property, propertyName));
            }

            return CreateSetter<TObject, TValue> (propertyInfo);
        }

        /// <summary>
        /// Создание типизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Action<TObject, TValue> CreateSetter<TObject, TValue>
            (
                PropertyInfo propertyInfo
            )
        {
            Sure.NotNull (propertyInfo);

            var method = propertyInfo.SetMethod;
            if (method is null)
            {
                throw new ArgumentException (string.Format (Resources.No_method_for, propertyInfo.Name));
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
            Sure.NotNull (type);
            Sure.NotNullNorEmpty (propertyName);

            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty (propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException (string.Format (Resources.Can_t_get_property, propertyName));
            }

            return CreateUntypedGetter (propertyInfo);
        }

        /// <summary>
        /// Построение нетипизированного геттера для указанного свойства объекта.
        /// </summary>
        public static Func<object, object?> CreateUntypedGetter
            (
                PropertyInfo propertyInfo
            )
        {
            Sure.NotNull (propertyInfo);

            var method = propertyInfo.GetMethod;
            if (method is null)
            {
                throw new ArgumentException (string.Format (Resources.No_method_for, propertyInfo.Name));
            }

            // Конструируем лямбду
            // (object instance) => (object) method ((T) instance)

            var declaringType = propertyInfo.DeclaringType.ThrowIfNull();
            var instance = Expression.Parameter (typeof (object));
            var argument = Expression.Convert (instance, declaringType);
            var call = Expression.Convert
                (
                    Expression.Call (argument, method),
                    typeof (object)
                );
            var lambda = Expression.Lambda<Func<object, object?>> (call, instance);

            return lambda.Compile();
        }

        /// <summary>
        /// Построение сеттера для указанного свойства объекта.
        /// </summary>
        public static Action<object, object?> CreateUntypedSetter
            (
                Type type,
                string propertyName
            )
        {
            Sure.NotNull (type);
            Sure.NotNullNorEmpty (propertyName);

            const BindingFlags bindingFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty (propertyName, bindingFlags);
            if (propertyInfo is null)
            {
                throw new ArgumentException (string.Format (Resources.Can_t_get_property, propertyName));
            }

            return CreateUntypedSetter (propertyInfo);
        }

        /// <summary>
        /// Построение нетипизированного сеттера для указанного свойства объекта.
        /// </summary>
        public static Action<object, object?> CreateUntypedSetter
            (
                PropertyInfo propertyInfo
            )
        {
            Sure.NotNull (propertyInfo);

            var method = propertyInfo.SetMethod;
            if (method is null)
            {
                throw new ArgumentException (string.Format (Resources.No_method_for, propertyInfo.Name));
            }

            // Конструируем лямбду
            // method ((T1) instance, (T2) value)

            var declaringType = propertyInfo.DeclaringType.ThrowIfNull();
            var instance = Expression.Parameter (typeof (object), "instance");
            var value = Expression.Parameter (typeof (object), "value");
            var convert1 = Expression.Convert (instance, declaringType);
            var convert2 = Expression.Convert (value, propertyInfo.PropertyType);
            var call = Expression.Call (convert1, method, convert2);

            var lambda = Expression.Lambda<Action<object, object?>> (call, instance, value);

            return lambda.Compile();
        }

        /// <summary>
        /// Получение кастомного атрибута (типизированное).
        /// Простая обертка над стандартным методом.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                Type classType,
                bool inherit = false
            )
            where T : Attribute
        {
            Sure.NotNull (classType);

            var all = classType.GetCustomAttributes
                (
                    typeof (T),
                    inherit
                );

            return (T?) all.FirstOrDefault();
        }

        /// <summary>
        /// Получение кастомного атрибута (типизированное).
        /// Простая обертка над стандартным методом.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                MemberInfo member,
                bool inhertit = false
            )
            where T : Attribute
        {
            Sure.NotNull (member);

            var all = member.GetCustomAttributes
                (
                    typeof (T),
                    inhertit
                );

            return (T?) all.FirstOrDefault();
        }

        /// <summary>
        /// Получение кастомного атрибута (типизированное).
        /// Простая обертка над стандартным методом.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                FieldInfo fieldInfo,
                bool inherit = false
            )
            where T : Attribute
        {
            Sure.NotNull (fieldInfo);

            var all = fieldInfo.GetCustomAttributes
                (
                    typeof (T),
                    inherit
                );

            return (T?) all.FirstOrDefault();
        }

        /// <summary>
        /// Получение кастомного атрибута (типизированное).
        /// Простая обертка над стандартным методом.
        /// </summary>
        public static T? GetCustomAttribute<T>
            (
                PropertyInfo propertyInfo,
                bool inherit = false
            )
            where T : Attribute
        {
            Sure.NotNull (propertyInfo);

            var all = propertyInfo.GetCustomAttributes
                (
                    typeof (T),
                    inherit
                );

            return (T?) all.FirstOrDefault();
        }

        /// <summary>
        /// Получение значения поля (неважно, публичного или приватного,
        /// статического или нет).
        /// </summary>
        public static object? GetFieldValue<T>
            (
                T target,
                string fieldName
            )
        {
            Sure.NotNullNorEmpty (fieldName);

            var fieldInfo = typeof (T).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (fieldInfo is null)
            {
                Magna.Error
                    (
                        nameof (ReflectionUtility) + "::" + nameof (GetFieldValue)
                        + Resources.CantFindField
                        + fieldName
                    );

                throw new ArgumentException (nameof (fieldName));
            }

            return fieldInfo.GetValue (target);
        }

        /// <summary>
        /// Чтение строки из ресурсов манифеста сборки.
        /// </summary>
        public static string GetManifestResourceString
            (
                this Assembly assembly,
                string resourceName
            )
        {
            Sure.NotNull (assembly);
            Sure.NotNullNorEmpty (resourceName);

            var stream = assembly.GetManifestResourceStream (resourceName);
            if (stream is null)
            {
                throw new MissingManifestResourceException
                    (
                        string.Format (Resources.ResourceDoesntExist, resourceName)
                    );
            }

            using (stream)
            using (var reader = new StreamReader (stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Простая проверка, содержит ли данный тип интересующий нас атрибут.
        /// </summary>
        public static bool HasAttribute<T>
            (
                Type type,
                bool inherit = false
            )
            where T : Attribute
        {
            Sure.NotNull (type);

            return GetCustomAttribute<T> (type, inherit) is not null;
        }

        /// <summary>
        /// Простая проверка, содержит ли данный член интересующий нас атрибут.
        /// </summary>
        public static bool HasAttribute<T>
            (
                this MemberInfo member,
                bool inherit = false
            )
            where T : Attribute
        {
            return GetCustomAttribute<T> (member, inherit) is not null;
        }

        /// <summary>
        /// Установка значения поля (неважно, публичного или приватного,
        /// статического или нет).
        /// </summary>
        public static void SetFieldValue<TTarget, TValue>
            (
                TTarget target,
                string fieldName,
                TValue value
            )
            where TTarget : class
        {
            Sure.NotNullNorEmpty (fieldName);

            var fieldInfo = typeof (TTarget).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (fieldInfo is null)
            {
                Magna.Error
                    (
                        nameof (ReflectionUtility) + "::" + nameof (SetFieldValue)
                        + Resources.CantFindField
                        + fieldName
                    );

                throw new ArgumentException (nameof (fieldName));
            }

            fieldInfo.SetValue (target, value);
        }

        /// <summary>
        /// Получение значения свойства (неважно, публичного или приватного,
        /// статического или нет).
        /// Не использует кеширование!
        /// Для эффективного многоразового применения см. <see cref="CreateGetter{TObject,TValue}(string)"/>.
        /// </summary>
        public static object? GetPropertyValue<T>
            (
                T target,
                string propertyName
            )
        {
            Sure.NotNullNorEmpty (propertyName);

            var propertyInfo = typeof (T).GetProperty
                (
                    propertyName,
                    BindingFlags.Public | BindingFlags.NonPublic
                        | BindingFlags.Instance | BindingFlags.Static
                );
            if (propertyInfo is null)
            {
                Magna.Error
                    (
                        nameof (ReflectionUtility) + "::" + nameof (GetPropertyValue)
                        + Resources.CantFindProperty
                        + propertyName
                    );

                throw new ArgumentException (nameof (propertyName));
            }

            return propertyInfo.GetValue (target, null);
        }

        /// <summary>
        /// Получение массива свойств и полей для указанного типа.
        /// </summary>
        public static PropertyOrField[] GetPropertiesAndFields
            (
                Type type,
                BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance
            )
        {
            Sure.NotNull (type);

            var result = new List<PropertyOrField>();
            foreach (var property in type.GetProperties (bindingFlags))
            {
                result.Add (new PropertyOrField (property));
            }

            foreach (var field in type.GetFields (bindingFlags))
            {
                result.Add (new PropertyOrField (field));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Установка значения свойства (неважно, публичного или приватного,
        /// статического или нет).
        /// Не использует кеширование!
        /// Для эффективного многоразового применения см. <see cref="CreateSetter{TObject,TValue}(string)"/>.
        /// </summary>
        public static void SetPropertyValue<TTarget, TValue>
            (
                TTarget target,
                string propertyName,
                TValue value
            )
        {
            Sure.NotNullNorEmpty (propertyName);

            var propertyInfo = typeof (TTarget).GetProperty
                (
                    propertyName,
                    BindingFlags.Public
                        | BindingFlags.NonPublic
                        | BindingFlags.Instance
                        | BindingFlags.Static
                );
            if (propertyInfo is null)
            {
                Magna.Error
                    (
                        nameof (ReflectionUtility) + "::" + nameof (SetPropertyValue)
                        + Resources.CantFindProperty
                        + propertyName
                    );

                throw new ArgumentException (nameof (propertyName));
            }

            propertyInfo.SetValue (target, value, null);
        }

        /// <summary>
        /// Получение массива имен констант, заданных в указанном типе.
        /// </summary>
        public static string[] ListConstantNames
            (
                Type type,
                bool inherit = false
            )
        {
            Sure.NotNull (type);

            var flags = BindingFlags.Public | BindingFlags.Static;
            if (inherit)
            {
                flags |= BindingFlags.FlattenHierarchy;
            }

            return type.GetFields (flags)
                .Where (field => field.IsLiteral && !field.IsInitOnly)
                .Select (field => field.Name)
                .ToArray();
        }

        /// <summary>
        /// Получение массива значений констант, заданных в указанном типе.
        /// </summary>
        public static T[] ListConstantValues<T>
            (
                Type type,
                bool inherit = false
            )
        {
            Sure.NotNull (type);

            var flags = BindingFlags.Public | BindingFlags.Static;
            if (inherit)
            {
                flags |= BindingFlags.FlattenHierarchy;
            }

            return type.GetFields (flags)
                .Where (field => field.IsLiteral && !field.IsInitOnly)
                .Where (field => field.FieldType == typeof (T))
                .Select (field => (T) field.GetValue (null)!)
                .ToArray();
        }

        /// <summary>
        /// Получение массива с информацией о константах, заданных в указанном типе.
        /// </summary>
        public static ConstantInfo<T>[] ListConstants<T>
            (
                Type type,
                bool inherit = false
            )
        {
            Sure.NotNull (type);

            var flags = BindingFlags.Public | BindingFlags.Static;
            if (inherit)
            {
                flags |= BindingFlags.FlattenHierarchy;
            }

            return type.GetFields (flags)
                .Where (field => field.IsLiteral && !field.IsInitOnly)
                .Where (field => field.FieldType == typeof(T))
                .Select (field => new ConstantInfo<T>(field.Name, (T) field.GetValue (null)!))
                .ToArray();
        }

        #endregion
    }
}
