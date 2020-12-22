// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SimplifyConditionalTernaryExpression
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MappingUtility.cs -- вспомогательные методы для отображения полей/подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Вспомогательные методы для отображения полей/подполей на поля классов.
    /// </summary>
    public static class MappingUtility
    {
        #region Public methods

        /// <summary>
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                SubField subField
            )
        {
            return !string.IsNullOrEmpty(subField.Value);
        } // method ToBoolean

        /// <summary>
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                return !string.IsNullOrEmpty(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? false
                : ToBoolean(subField);
        } // method ToBoolean

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                SubField subField
            )
        {
            var text = subField.Value;

            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        } // method ToChar

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                var text = field.Value;

                return string.IsNullOrEmpty(text)
                    ? '\0'
                    : text[0];
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? '\0'
                : ToChar(subfield);
        } // method ToChar

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                SubField subField
            )
        {
            return IrbisDate.ConvertStringToDate(subField.Value);
        } // method ToDateTime

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                var text = field.Value;

                return string.IsNullOrEmpty(text)
                    ? DateTime.MinValue
                    : IrbisDate.ConvertStringToDate(text);
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? DateTime.MinValue
                : ToDateTime(subfield);
        } // method ToDateTime

        /// <summary>
        /// Преобразование в число с фиксированной точкой.
        /// </summary>
        public static decimal ToDecimal
            (
                SubField subField
            )
        {
            decimal.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDecimal

        public static decimal ToDecimal
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                decimal.TryParse
                    (
                        field.Value,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var result
                    );

                return result;
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0m
                : ToDecimal(subfield);
        }

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// двойной точностью.
        /// </summary>
        public static double ToDouble
            (
                SubField subField
            )
        {
            double.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDouble

        public static double ToDouble
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                double.TryParse
                    (
                        field.Value,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var result
                    );

                return result;
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0.0
                : ToDouble(subfield);
        }

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// одинарной точности.
        /// </summary>
        public static float ToSingle
            (
                SubField subField
            )
        {
            float.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToSingle


        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                SubField subField
            )
        {
            int.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        }

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                int.TryParse
                    (
                        field.Value,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var result
                    );

                return result;
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0
                : ToInt32(subfield);
        } // method ToInt32

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToInt32(field, code);
        } // method ToInt32

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                SubField subField
            )
        {
            long.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToInt64

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                long.TryParse
                    (
                        field.Value,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var result
                    );

                return result;
            }
            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0
                : ToInt64(subfield);
        } // method ToInt64

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToInt64(field, code);
        } // method ToInt64

        /// <summary>
        /// Преобразование в строку (тривиальное).
        /// </summary>
        public static string? ToString
            (
                SubField subField
            )
        {
            return subField.Value;
        } // method ToString

        /// <summary>
        /// Преобразование в строку (тривиальное).
        /// </summary>
        public static string? ToString
            (
                Field field,
                char code
            )
        {
            if (code == '\0')
            {
                return field.Value;
            }

            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? null
                : ToString(subfield);
        } // method ToString

        /// <summary>
        /// Преобразование в строку (тривиальное).
        /// </summary>
        public static string? ToString
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? null
                : ToString(field, code);
        } // method ToString

        private static MethodInfo ChooseFieldMethod
            (
                PropertyInfo property
            )
        {
            string? methodName;
            var propertyType = property.PropertyType;

            if (propertyType == typeof(decimal))
            {
                methodName = "ToDecimal";
            }
            else if (propertyType == typeof(double))
            {
                methodName = "ToDouble";
            }
            else if (propertyType == typeof(int))
            {
                methodName = "ToInt32";
            }
            else if (propertyType == typeof(string))
            {
                methodName = "ToString";
            }
            else
            {
                throw new ArgumentException();
            }

            var method = typeof(MappingUtility).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Field), typeof(char) }
                )
                .ThrowIfNull("method");

            return method;
        }

        private static MethodInfo ChooseRecordMethod
            (
                PropertyInfo property
            )
        {
            string? methodName;
            var propertyType = property.PropertyType;

            if (propertyType == typeof(decimal))
            {
                methodName = "ToDecimal";
            }
            else if (propertyType == typeof(double))
            {
                methodName = "ToDouble";
            }
            else if (propertyType == typeof(int))
            {
                methodName = "ToInt32";
            }
            else if (propertyType == typeof(string))
            {
                methodName = "ToString";
            }
            else
            {
                throw new ArgumentException();
            }

            var method = typeof(MappingUtility).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Record), typeof(int), typeof(char) }
                )
                .ThrowIfNull("method");

            return method;
        }

        /// <summary>
        /// Построение простого маппера для подполя.
        /// </summary>
        public static Action<Field, T> CreateSubFieldMapper<T>
            (
                SubFieldAttribute attribute,
                PropertyInfo property
            )
        {
            var target = Expression.Parameter(typeof(T), "target");
            var field = Expression.Parameter(typeof(Field), "field");
            var code = Expression.Constant(attribute.Code, typeof(char));
            var accessor = Expression.PropertyOrField(target, property.Name);
            var method = ChooseFieldMethod(property);
            var call = Expression.Call(method, field, code);
            var assignment = Expression.Assign
                (
                    accessor,
                    call
                );
            var lambda = Expression.Lambda <Action<Field, T>>
                (
                    assignment,
                    field,
                    target
                );

            return lambda.Compile();
        }

        /// <summary>
        /// Построение маппера для заданного типа.
        /// </summary>
        public static Action<Field, T> CreateForwardFieldMapper<T>()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(typeof(T), "target");
            var field = Expression.Parameter(typeof(Field), "field");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SubFieldAttribute>();
                if (attribute is not null)
                {
                    var code = Expression.Constant(attribute.Code, typeof(char));
                    var accessor = Expression.PropertyOrField(target, property.Name);
                    var method = ChooseFieldMethod(property);
                    var call = Expression.Call(method, field, code);
                    var assignment = Expression.Assign(accessor, call);
                    expressions.Add(assignment);
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Field, T>>
                (
                    body,
                    field,
                    target
                );

            return lambda.Compile();
        } // method CreateForwardFieldMapper

        public static Action<Field, T> CreateBackwardFieldMapper<T>()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(typeof(T), "target");
            var field = Expression.Parameter(typeof(Field), "field");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SubFieldAttribute>();
                if (attribute is not null)
                {
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Field, T>>
            (
                body,
                field,
                target
            );

            return lambda.Compile();
        } // method CreateBackwardFieldMapper

        /// <summary>
        /// Построение маппера для заданного типа.
        /// </summary>
        public static Action<Record, T> CreateForwardRecordMapper<T>()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(typeof(T), "target");
            var record = Expression.Parameter(typeof(Record), "record");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FieldAttribute>();
                if (attribute is not null)
                {
                    var tag = Expression.Constant(attribute.Tag, typeof(int));
                    var code = Expression.Constant(attribute.Code, typeof(char));
                    var accessor = Expression.PropertyOrField(target, property.Name);
                    var method = ChooseRecordMethod(property);
                    var call = Expression.Call(method, record, tag, code);
                    var assignment = Expression.Assign(accessor, call);
                    expressions.Add(assignment);
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Record, T>>
                (
                    body,
                    record,
                    target
                );

            return lambda.Compile();
        } // method CreateForwardRecordMapper

        /// <summary>
        /// Построение маппера для заданного типа.
        /// </summary>
        public static Action<Record, T> CreateBackwardRecordMapper<T>()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(typeof(T), "target");
            var record = Expression.Parameter(typeof(Record), "record");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FieldAttribute>();
                if (attribute is not null)
                {
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Record, T>>
                (
                    body,
                    record,
                    target
                );

            return lambda.Compile();
        } // method CreateBackwardRecordMapper

        #endregion

    } // class MappingUtility

} // namespace ManagedIrbis.Mapping
