// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* FieldUtility.cs -- методы расширения для Field
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Методы расширения для <see cref="Field"/>.
    /// </summary>
    public static class FieldUtility
    {
        #region Public methods

        /// <summary>
        /// Добавление подполя, при условии, что оно не пустое.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                field.Add(code, value);
            }

            return field;

        } // method AddNonEmptySubField

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                bool flag,
                string? value
            )
        {
            if (flag && !string.IsNullOrEmpty(value))
            {
                field.Add(code, value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                int? value
            )
        {
            if (value.HasValue)
            {
                field.Add(code, value.Value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                long? value
            )
        {
            if (value.HasValue)
            {
                field.Add(code, value.Value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                DateTime? value
            )
        {
            if (value.HasValue)
            {
                field.Add(code, IrbisDate.ConvertDateToString(value.Value));
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Добавление подполей.
        /// </summary>
        public static Field AddSubFields
            (
                this Field field,
                IEnumerable<SubField>? subFields
            )
        {
            if (!ReferenceEquals(subFields, null))
            {
                foreach (var subField in subFields)
                {
                    field.Subfields.Add(subField);
                }
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Все подполя.
        /// </summary>
        public static SubField[] AllSubFields
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .SelectMany(field => field.Subfields)
                .NonNullItems()
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                object? value
            )
        {
            Sure.NotNull(field, nameof(field));

            if (code == SubField.NoCode)
            {
                return field;
            }

            if (ReferenceEquals(value, null))
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = value.ToString();
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                bool value,
                string text
            )
        {
            if (code == SubField.NoCode)
            {
                return field;
            }

            if (value == false)
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = text;
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                string? value
            )
        {
            if (code == SubField.NoCode)
            {
                return field;
            }

            if (string.IsNullOrEmpty(value))
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = value;
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        public static SubField[] FilterSubFields
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {

            return subFields
                .Where
                    (
                        subField => subField.Code.IsOneOf(codes)
                    )
                .ToArray();
        }

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        public static SubField[] FilterSubFields
            (
                this Field field,
                params char[] codes
            )
        {
            return field.Subfields
                .FilterSubFields(codes);
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            return fields
                .Where(field => field.Tag == tag)
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetField
            (
                this IEnumerable<Field> fields,
                int tag,
                int occurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int[] tags
            )
        {
            return fields
                .Where(field => field.Tag.IsOneOf(tags))
                .ToArray();
        }

        ///// <summary>
        ///// Фильтрация полей.
        ///// </summary>
        //[NotNull]
        //[ItemNotNull]
        //public static RecordField[] GetField
        //    (
        //        this RecordFieldCollection fields,
        //        params int[] tags
        //    )
        //{
        //    Code.NotNull(fields, "fields");

        //    List<RecordField> result = null;
        //    int count = fields.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (fields[i].Tag.OneOf(tags))
        //        {
        //            if (ReferenceEquals(result, null))
        //            {
        //                result = new List<RecordField>();
        //            }
        //            result.Add(fields[i]);
        //        }
        //    }

        //    return ReferenceEquals(result, null)
        //        ? EmptyArray
        //        : result.ToArray();
        //}

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetField
            (
                this IEnumerable<Field> fields,
                int[] tags,
                int occurrence
            )
        {
            return fields
                .GetField(tags)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where(predicate)
                .ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над полями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<Field>? action
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(action, null))
            {
                foreach (var field in result)
                {
                    action(field);
                }
            }

            return result;
        } // method GetField

        /// <summary>
        /// Выполнение неких действий над полями и подполями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<Field>? fieldAction,
                Action<SubField>? subFieldAction
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(fieldAction, null)
                || !ReferenceEquals(subFieldAction, null))
            {
                foreach (var field in result)
                {
                    fieldAction?.Invoke(field);

                    if (!ReferenceEquals(subFieldAction, null))
                    {
                        foreach (var subField in field.Subfields)
                        {
                            subFieldAction(subField);
                        }
                    }
                }
            } // method GetField

            return result;
        }

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<SubField>? action
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(action, null))
            {
                foreach (var field in result)
                {
                    foreach (var subField in field.Subfields)
                    {
                        action(subField);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .Where(field => field.Subfields.Any(predicate))
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char[] codes,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where
                    (
                        field => field.Subfields
                            .NonNullItems()
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && predicate(sub)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char[] codes,
                params string[] values
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && sub.Value.SameString(values)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char code,
                string? value
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(code)
                                        && sub.Value.SameString(value)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string[] values
            )
        {
            return fields
                .Where(field => field.Tag.IsOneOf(tags))
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && sub.Value.SameString(values)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
            )
        {
            return fields
                .Where(fieldPredicate)
                .Where(field => field.Subfields.Any(subPredicate))
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Количество повторений поля.
        /// </summary>
        public static int GetFieldCount
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            var result = 0;

            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    result++;
                }
            }

            return result;
        } // method GetFieldCount

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                string tagRegex
            )
        {
            return fields
                .Where
                    (
                        field =>
                        {
                            var tag = field.Tag.ToInvariantString();

                            return !string.IsNullOrEmpty(tag)
                                && Regex.IsMatch
                                   (
                                       tag,
                                       tagRegex
                                   );
                        }
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                string tagRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tagRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                string textRegex
            )
        {
            // TODO: реализовать эффективно

            return fields
                .GetField(tags)
                .Where
                    (
                        field =>
                        {
                            var value = field.Value;
                            return !value.IsEmpty()
                                   && Regex.IsMatch(value, textRegex);
                        })
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, textRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string textRegex
            )
        {
            // TODO: реализовать эффективно

            var regex = new Regex(textRegex);
            return fields
                .GetField(tags)
                .Where(field => field.FilterSubFields(codes)
                    .Any(sub =>
                    {
                        var value = sub.Value;

                        return !value.IsEmpty()
                            && regex.IsMatch(value);
                    }))
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, codes, textRegex)
                .GetOccurrence(occurrence);
        }

        // ==========================================================

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        public static string[] GetFieldValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Select
                    (
                        field => field.Value!
                    )
                .Where(line => !line.IsEmpty())
                .ToArray();
        }

        /// <summary>
        /// Непустые значения полей с указанным тегом.
        /// </summary>
        public static string[] GetFieldValue
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            var result = new List<string>();
            foreach (var field in fields.NonNullItems())
            {
                if (field.Tag == tag
                    && !field.Value.IsEmpty())
                {
                    result.Add(field.Value);
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag1,
                int tag2
            )
        {
            foreach (var field in fields)
            {
                var tag = field.Tag;
                if (tag == tag1 || tag == tag2)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag1,
                int tag2,
                int tag3
            )
        {
            foreach (var field in fields)
            {
                var tag = field.Tag;
                if (tag == tag1 || tag == tag2 || tag == tag3)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с любым из перечисленных тегов.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                params int[] tags
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag.IsOneOf(tags))
                {
                    return field;
                }
            }

            return null;
        }

        // ==========================================================

        ///// <summary>
        ///// Значение первого поля с указанным тегом или <c>null</c>.
        ///// </summary>
        //[CanBeNull]
        //public static string GetFirstFieldValue
        //    (
        //        this IEnumerable<RecordField> fields,
        //        int tag
        //    )
        //{
        //    Code.NotNull(fields, "fields");

        //    foreach (RecordField field in fields)
        //    {
        //        if (field.Tag == tag)
        //        {
        //            return field.Value;
        //        }
        //    }

        //    return null;
        //}

        // ==========================================================

        /// <summary>
        /// Gets the first subfield.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя, соответствующего указанным
        /// критериям.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    SubFieldCollection subFields = field.Subfields;
                    var count = subFields.Count;
                    for (var i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.SameChar(code))
                        {
                            return subFields[i];
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Получение текста указанного подполя
        /// </summary>
        public static string? GetFirstSubFieldValue
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i].Value;
                }
            }

            return default;
        }

        /// <summary>
        /// Значение первого подполя с указанными тегом и кодом
        /// или <c>null</c>.
        /// </summary>
        public static string? GetFirstSubFieldValue
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            return subField.Value;
                        }
                    }
                }
            }

            return default;
        }

        // ==========================================================

        /// <summary>
        /// Перечень подполей с указанным кодом.
        /// </summary>
        public static SubField[] GetSubField
            (
                this Field field,
                char code
            )
        {
            List<SubField>? result = null;
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    result ??= new List<SubField>();
                    result.Add(subFields[i]);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        public static SubField? GetSubField
            (
                this Field field,
                char code,
                int occurrence
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                SubFieldCollection subFields = field.Subfields;
                var count = subFields.Count;
                for (var i = 0; i < count; i++)
                {
                    if (subFields[i].Code.SameChar(code))
                    {
                        result ??= new List<SubField>();
                        result.Add(subFields[i]);
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                params char[] codes
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                var subFields = field.Subfields;
                var count = subFields.Count;
                for (var i = 0; i < count; i++)
                {
                    if (subFields[i].Code.IsOneOf(codes))
                    {
                        result ??= new List<SubField>();
                        result.Add(subFields[i]);
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            result ??= new List<SubField>();
                            result.Add(subField);
                        }
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        public static SubField? GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                int fieldOccurrence,
                char code,
                int subOccurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (fieldOccurrence == 0)
                    {
                        SubFieldCollection subFields = field.Subfields;
                        var subCount = subFields.Count;
                        for (var j = 0; j < subCount; j++)
                        {
                            if (subFields[j].Code.SameChar(code))
                            {
                                if (subOccurrence == 0)
                                {
                                    return subFields[j];
                                }
                                subOccurrence--;
                            }
                        }

                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        public static SubField? GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code,
                int occurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    SubFieldCollection subFields = field.Subfields;
                    var subCount = subFields.Count;
                    for (var j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar(code))
                        {
                            if (occurrence == 0)
                            {
                                return subFields[j];
                            }
                            occurrence--;
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        public static string? GetSubFieldValue
            (
                this Field field,
                char code,
                int occurrence
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i].Value;
                    }
                    occurrence--;
                }
            }

            return default;
        }

        // ==========================================================

        /// <summary>
        /// Непустые значения подполей с указанными тегом и кодом.
        /// </summary>
        public static string[] GetSubFieldValue
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            var result = new List<string>();
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code)
                            && !subField.Value.IsEmpty())
                        {
                            result.Add(subField.Value);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Есть хотя бы одно подполе с указанным кодом?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Есть хотя бы одно подполе с указанным кодом?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                char code,
                string value
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code)
                    && subFields[i].Value.SameString(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Есть хотя бы одно поле с любым из указанных кодов?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                params char[] codes
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(codes))
                {
                    return true;
                }
            }

            return false;
        }

        // ==========================================================

        /// <summary>
        /// Нет ни одного подполя с указанным кодом?
        /// </summary>
        public static bool HaveNotSubField
            (
                this Field field,
                char code
            )
        {
            Sure.NotNull(field, nameof(field));

            SubFieldCollection subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Нет ни одного подполя с указанными кодами?
        /// </summary>
        public static bool HaveNotSubField
            (
                this Field field,
                params char[] codes
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(codes))
                {
                    return false;
                }
            }

            return true;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] NotNullTag
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Tag != 0
                    )
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] NotNullValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields .Where(field => !field.Value.IsEmpty())
                .ToArray();
        }

        // ==========================================================

        ///// <summary>
        ///// Удаляем подполе.
        ///// </summary>
        ///// <remarks>Удаляет все повторения подполей
        ///// с указанным кодом.
        ///// </remarks>
        //[NotNull]
        //public static RecordField RemoveSubField
        //    (
        //        this RecordField field,
        //        char code
        //    )
        //{
        //    SubField[] found = field.SubFields
        //        .FindAll(_ => char.ToLowerInvariant(_.Code) == code)
        //        .ToArray();

        //    foreach (SubField subField in found)
        //    {
        //        field.SubFields.Remove(subField);
        //    }

        //    return field;
        //}

        // ==========================================================

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        public static Field ReplaceSubField
            (
                this Field field,
                char code,
                string oldValue,
                string newValue
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                var subField = subFields[i];
                if (subField.Code.SameChar(code))
                {
                    if (subField.Value.SameStringSensitive(oldValue))
                    {
                        subField.Value = newValue;
                        /* subField.SetValue(newValue); */
                    }
                }
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        public static Field ReplaceSubField
            (
                this Field field,
                char code,
                string newValue,
                bool ignoreCase
            )
        {
            var oldValue = field.GetSubFieldValue
                (
                    code
                );
            var changed = string.CompareOrdinal(oldValue, newValue);

            if (changed != 0)
            {
                field.SetSubFieldValue(code, newValue);
            }

            return field;

        }

        // ==========================================================

        /// <summary>
        /// Get unknown subfields.
        /// </summary>
        public static SubField[] GetUnknownSubFields
            (
                this IEnumerable<SubField> subFields,
                ReadOnlySpan<char> knownCodes
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code != '\0'
                    && !subField.Code.SameChar(knownCodes))
                {
                    result ??= new List<SubField>();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        // ==========================================================

        /*

        /// <summary>
        /// Convert the field to <see cref="JObject"/>.
        /// </summary>
        public static JObject ToJObject
            (
                this Field field
            )
        {
            JObject result = JObject.FromObject(field);

            return result;
        }

        /// <summary>
        /// Convert the field to JSON.
        /// </summary>
        public static string ToJson
            (
                this Field field
            )
        {
            string result = JObject.FromObject(field)
                .ToString(Formatting.None);

            return result;
        }

        /// <summary>
        /// Restore field from <see cref="JObject"/>.
        /// </summary>
        public static Field FromJObject
            (
                JObject jObject
            )
        {
            var result = jObject.ToObject<Field>();

            return result;
        }

        /// <summary>
        /// Restore subfield from JSON.
        /// </summary>
        public static Field FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            Field result = JsonConvert.DeserializeObject<Field>(text);

            return result;
        }

        */

        // ==========================================================

        /// <summary>
        /// Парсинг текстового представления поля
        /// </summary>
        public static Field Parse
            (
                ReadOnlyMemory<char> tag,
                ReadOnlyMemory<char> body
            )
        {
            var result = new Field { Tag = tag.ParseInt32() };

            var first = body.Span.IndexOf(Field.Delimiter);
            if (first != 0)
            {
                if (first < 0)
                {
                    result.Value = body.ToString();
                    body = default;
                }
                else
                {
                    result.Value = body.Slice
                        (
                            0,
                            first
                        )
                        .ToString();
                    body = body.Slice(first);
                }
            }

            // TODO: реализовать эффективно

            var code = (char)0;
            var value = new StringBuilder();
            foreach (var c in body.Span)
            {
                if (c == Field.Delimiter)
                {
                    if (code != '\0')
                    {
                        result.Add
                            (
                                code,
                                value
                            );
                    }
                    value.Length = 0;
                    code = (char)0;
                }
                else
                {
                    if (code == 0)
                    {
                        code = c;
                    }
                    else
                    {
                        value.Append(c);
                    }
                }
            }

            if (code != (char)0)
            {
                result.Add
                    (
                        code,
                        value
                    );
            }

            return result;
        }

        /// <summary>
        /// Парсинг строкового представления поля.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static Field? Parse
            (
                string line
            )
        {
            // TODO: реализовать эффективно

            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            var parts = line.Split('#', 2);
            var tag = parts[0];
            var body = parts.Length == 1 ? string.Empty : parts[1];
            return Parse
                (
                    tag.AsMemory(),
                    body.AsMemory()
                );
        }

        // ==========================================================

        /// <summary>
        /// Converts the field to XML.
        /// </summary>
        public static string ToXml
            (
                this Field field
            )
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                NewLineOnAttributes = false,
                Indent = true,
                CloseOutput = true
            };
            var writer = new StringWriter();
            var xml = XmlWriter.Create(writer, settings);
            var serializer = new XmlSerializer
                (
                    typeof(Field)
                );
            serializer.Serialize(xml, field);

            return writer.ToString();
        }

        /// <summary>
        /// Restore the field from XML.
        /// </summary>
        public static Field? FromXml
            (
                string text
            )
        {
            var serializer = new XmlSerializer(typeof(Field));
            var reader = new StringReader(text);
            var result = (Field?)serializer.Deserialize(reader);

            return result;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithNullTag
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Tag == 0
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithNullValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields .Where(field => field.Value.IsEmpty())
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithoutSubFields
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields.Count == 0
                    )
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithSubFields
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields.Count != 0
                    )
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Поиск поля, которое обязательно должно быть.
        /// </summary>
        public static Field RequireField
            (
                this IEnumerable<Field> fields,
                int tag,
                int occurrence = default
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }
                    occurrence--;
                }
            }

            throw new KeyNotFoundException($"Tag={tag}");

        } // method RequireField

        // ==========================================================

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    yield return field;
                }
            }
        } // method EnumerateField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (field.HaveSubField(code))
                    {
                        yield return field;
                    }
                }
            }
        } // method EnumerateField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code,
                string value
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (field.HaveSubField(code, value))
                    {
                        yield return field;
                    }
                }
            }
        } // method EnumerateField

        #endregion

    } // class FieldUtility

} // namespace ManagedIrbis
