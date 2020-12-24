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
using System.Linq.Expressions;
using System.Reflection;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Вспомогательные методы для отображения полей/подполей на поля классов.
    /// </summary>
    public static class MappingUtility
    {
        #region Private members

        private static readonly Dictionary<TypeCode, string> _toType = new ()
        {
            [TypeCode.Boolean]  = "ToBoolean",
            [TypeCode.Byte]     = "ToByte",
            [TypeCode.Char]     = "ToChar",
            [TypeCode.DateTime] = "ToDateTime",
            [TypeCode.DBNull]   = "ToError",
            [TypeCode.Decimal]  = "ToDecimal",
            [TypeCode.Double]   = "ToDouble",
            [TypeCode.Empty]    = "ToError",
            [TypeCode.Int16]    = "ToInt16",
            [TypeCode.Int32]    = "ToInt32",
            [TypeCode.Int64]    = "ToInt64",
            [TypeCode.Object]   = "ToObject",
            [TypeCode.SByte]    = "ToSByte",
            [TypeCode.Single]   = "ToSingle",
            [TypeCode.String]   = "ToString",
            [TypeCode.UInt16]   = "ToUInt16",
            [TypeCode.UInt32]   = "ToUInt32",
            [TypeCode.UInt64]   = "ToUInt64"
        }; // Dictionary _toType

        private static readonly Dictionary<TypeCode, string> _fromType = new ()
        {
            [TypeCode.Boolean]  = "FromBoolean",
            [TypeCode.Byte]     = "FromByte",
            [TypeCode.Char]     = "FromChar",
            [TypeCode.DateTime] = "FromDateTime",
            [TypeCode.DBNull]   = "FromError",
            [TypeCode.Decimal]  = "FromDecimal",
            [TypeCode.Double]   = "FromDouble",
            [TypeCode.Empty]    = "FromError",
            [TypeCode.Int16]    = "FromInt16",
            [TypeCode.Int32]    = "FromInt32",
            [TypeCode.Int64]    = "FromInt64",
            [TypeCode.Object]   = "FromObject",
            [TypeCode.SByte]    = "FromSByte",
            [TypeCode.Single]   = "FromSingle",
            [TypeCode.String]   = "FromString",
            [TypeCode.UInt16]   = "FromUInt16",
            [TypeCode.UInt32]   = "FromUInt32",
            [TypeCode.UInt64]   = "FromUInt64"
        }; // Dictionary _fromType

        private static MethodInfo ChooseForwardFieldMethod
            (
                PropertyInfo property
            )
        {
            var propertyType = property.PropertyType;
            var typeCode = Type.GetTypeCode(propertyType);
            var methodName = _toType[typeCode];

            var method = typeof(Map).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Field), typeof(char) }
                )
                .ThrowIfNull("method");

            return method;
        } // method ChooseForwardFieldMethod

        private static MethodInfo ChooseBackwardFieldMethod
            (
                PropertyInfo property
            )
        {
            var propertyType = property.PropertyType;
            var typeCode = Type.GetTypeCode(propertyType);
            var methodName = _fromType[typeCode];

            var method = typeof(Map).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Field), typeof(char), propertyType }
                )
                .ThrowIfNull("method");

            return method;
        } // method ChooseBackwardFieldMethod

        private static MethodInfo ChooseForwardRecordMethod
            (
                PropertyInfo property
            )
        {
            var propertyType = property.PropertyType;
            var typeCode = Type.GetTypeCode(propertyType);
            var methodName = _toType[typeCode];

            var method = typeof(Map).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Record), typeof(int), typeof(char) }
                )
                .ThrowIfNull("method");

            return method;
        } // method ChooseForwardRecordMethod

        private static MethodInfo ChooseBackwardRecordMethod
            (
                PropertyInfo property
            )
        {
            var propertyType = property.PropertyType;
            var typeCode = Type.GetTypeCode(propertyType);
            var methodName = _fromType[typeCode];

            var method = typeof(Map).GetMethod
                (
                    methodName.ThrowIfNull("methodName"),
                    new[] { typeof(Record), typeof(int), typeof(char), propertyType }
                )
                .ThrowIfNull("method");

            return method;
        } // method ChooseForwardRecordMethod

        #endregion

        #region Public methods

        /*

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
            var method = ChooseForwardFieldMethod(property);
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

        */

        /// <summary>
        /// Построение прямого маппера для заданного типа.
        /// </summary>
        public static Expression<Action<Field, T>> CreateForwardFieldMapper<T>()
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
                    var codeParameter = Expression.Constant(attribute.Code, typeof(char));
                    var accessor = Expression.PropertyOrField(target, property.Name);
                    var method = ChooseForwardFieldMethod(property);
                    var call = Expression.Call(method, field, codeParameter);
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

            return lambda;
        } // method CreateForwardFieldMapper

        /// <summary>
        /// Построение обратного маппера для указанного типа.
        /// </summary>
        public static Expression<Action<Field, object>> CreateBackwardFieldMapper
            (
                Type type
            )
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type.GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(type, "target");
            var field = Expression.Parameter(typeof(Field), "field");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SubFieldAttribute>();
                if (attribute is not null)
                {
                    var code = Expression.Constant(attribute.Code, typeof(char));
                    var accessor = Expression.PropertyOrField(target, property.Name);
                    var method = ChooseBackwardFieldMethod(property);
                    var call = Expression.Call(method, field, code, accessor);
                    expressions.Add(call);
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Field, object>>
            (
                body,
                field,
                target
            );

            return lambda;
        } // method CreateBackwardFieldMapper

        /// <summary>
        /// Построение обратного маппера для указанного типа.
        /// </summary>
        public static Expression<Action<Field, T>> CreateBackwardFieldMapper<T>()
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
                    var method = ChooseBackwardFieldMethod(property);
                    var call = Expression.Call(method, field, code, accessor);
                    expressions.Add(call);
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Field, T>>
                (
                    body,
                    field,
                    target
                );

            return lambda;
        } // method CreateBackwardFieldMapper

        /// <summary>
        /// Построение прямого маппера для заданного типа.
        /// </summary>
        public static Expression<Action<Record, T>> CreateForwardRecordMapper<T>()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(flags);
            var expressions = new List<Expression>();
            var target = Expression.Parameter(typeof(T), "target");
            var record = Expression.Parameter(typeof(Record), "record");

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FieldAttribute>();
                var typeCode = Type.GetTypeCode(property.PropertyType);
                if (typeCode == TypeCode.Object)
                {
                    attribute ??= property.PropertyType.GetCustomAttribute<FieldAttribute>();
                    if (attribute is not null)
                    {
                        var tag = attribute.Tag;
                    }
                }
                else
                {
                    if (attribute is not null)
                    {
                        var tag = Expression.Constant(attribute.Tag, typeof(int));
                        var code = Expression.Constant(attribute.Code, typeof(char));
                        var accessor = Expression.PropertyOrField(target, property.Name);
                        var method = ChooseForwardRecordMethod(property);
                        var call = Expression.Call(method, record, tag, code);
                        var assignment = Expression.Assign(accessor, call);
                        expressions.Add(assignment);
                    }
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Record, T>>
                (
                    body,
                    record,
                    target
                );

            return lambda;
        } // method CreateForwardRecordMapper

        /// <summary>
        /// Построение обратного маппера для заданного типа.
        /// </summary>
        public static Expression<Action<Record, T>> CreateBackwardRecordMapper<T>()
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
                    var method = ChooseBackwardRecordMethod(property);
                    var call = Expression.Call(method, record, tag, code, accessor);
                    expressions.Add(call);
                }
            }

            var body = Expression.Block(expressions.ToArray());
            var lambda = Expression.Lambda<Action<Record, T>>
                (
                    body,
                    record,
                    target
                );

            return lambda;
        } // method CreateBackwardRecordMapper

        #endregion

    } // class MappingUtility

} // namespace ManagedIrbis.Mapping
