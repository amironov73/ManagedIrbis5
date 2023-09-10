// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* Utlility.cs -- разнообразные полезные методы расширения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Разнообразные полезные методы расширения.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Однобайтовая кодировка по умолчанию.
        /// Как правило, кодовая страница 1251.
        /// </summary>
        public static Encoding Ansi => Windows1251;

        /// <summary>
        /// Регистрация кодировок, основанных на кодовых страницах.
        /// </summary>
        public static void RegisterEncodingProviders() =>
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        private static Encoding? _windows1251;

        /// <summary>
        /// Получение кодировки Windows-1251 (cyrillic) <see cref="Encoding"/>.
        /// </summary>
        public static Encoding Windows1251
        {
            [DebuggerStepThrough]
            get
            {
                if (ReferenceEquals (_windows1251, null))
                {
                    RegisterEncodingProviders();
                    _windows1251 = Encoding.GetEncoding (1251);
                }

                return _windows1251;
            }
        }

        /// <summary>
        /// Проверка: валиден ли код подполя.
        /// </summary>
        public static bool IsValidSubFieldCode (char code) =>
            code is >= Constants.FirstCode and <= Constants.LastCode and not '^';

        /// <summary>
        /// Верификация кода подполя с выбросом исключения.
        /// </summary>
        public static bool VerifySubFieldCode (char code, bool trhowOnError = true) =>
            IsValidSubFieldCode (code) || (trhowOnError ? throw new IrbisException() : false);

        /// <summary>
        /// Нормализация кода подполя.
        /// </summary>
        public static char NormalizeCode (char code) => char.ToLowerInvariant (code);

        /// <summary>
        /// Проверка: валидно ли значение подполя.
        /// </summary>
        public static bool IsValidSubFieldValue
            (
                ReadOnlySpan<char> value
            )
        {
            foreach (var c in value)
            {
                if (c == SubField.Delimiter)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Верификация значения подполя с выбромо исключения.
        /// </summary>
        public static bool VerifySubFieldValue (ReadOnlySpan<char> value, bool throwOnError = true) =>
            IsValidSubFieldValue (value) || (throwOnError ? throw new IrbisException() : false);

        /// <summary>
        /// Нормализация значения подполя.
        /// </summary>
        public static string? NormalizeSubFieldValue
            (
                string? value
            )
        {
            if (ReferenceEquals (value, null) || value.Length == 0)
            {
                return value;
            }

            var result = value.Trim();

            return result;
        }

        /// <summary>
        /// Первое вхождение подполя с указанным кодом.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                char code
            )
        {
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar (code))
                {
                    return subField;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с одним из указанных кодов.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            foreach (var subField in subFields)
            {
                foreach (var code in codes)
                {
                    if (subField.Code.SameChar (code))
                    {
                        return subField;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с указанными кодом
        /// и значением (с учётом регистра символов).
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                char code,
                string? value
            )
        {
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar (code)
                    && subField.Value.SameString (value))
                {
                    return subField;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar (code))
                {
                    result ??= new List<SubField>();
                    result.Add (subField);
                }
            }

            return ReferenceEquals (result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar (codes))
                {
                    result ??= new ();
                    result.Add (subField);
                }
            }

            return ReferenceEquals (result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                Action<SubField>? action
            )
        {
            var result = subFields.ToArray();

            if (!ReferenceEquals (action, null))
            {
                foreach (var subField in result)
                {
                    action (subField);
                }
            }

            return result;
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                if (fieldPredicate (field))
                {
                    foreach (SubField subField in field.Subfields)
                    {
                        if (subPredicate (subField))
                        {
                            result ??= new List<SubField>();
                            result.Add (subField);
                        }
                    }
                }
            }

            return ReferenceEquals (result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение значения подполя.
        /// </summary>
        public static string[] GetSubFieldValue
            (
                this IEnumerable<SubField> subFields
            )
        {
            List<string>? result = null;
            foreach (var subField in subFields.NonNullItems())
            {
                var value = subField.Value;
                if (!ReferenceEquals (value, null) && value.Length != 0)
                {
                    result ??= new List<string>();
                    result.Add (value);
                }
            }

            return ReferenceEquals (result, null)
                ? Array.Empty<string>()
                : result.ToArray();
        }

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
            if (!string.IsNullOrEmpty (value))
            {
                field.Add (code, value);
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
                bool flag,
                string? value
            )
        {
            if (flag && !string.IsNullOrEmpty (value))
            {
                field.Add (code, value);
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
                field.Add (code, value.Value);
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
                field.Add (code, value.Value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполей.
        /// </summary>
        public static Field AddSubFields
            (
                this Field field,
                IEnumerable<SubField>? subFields
            )
        {
            if (!ReferenceEquals (subFields, null))
            {
                foreach (var subField in subFields)
                {
                    field.Subfields.Add (subField);
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
                .SelectMany (field => field.Subfields)
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
            if (code == SubField.NoCode)
                return field;

            if (ReferenceEquals (value, null))
            {
                field.RemoveSubField (code);
            }
            else
            {
                var subField = field.GetFirstSubField (code);
                if (ReferenceEquals (subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add (subField);
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
                field.RemoveSubField (code);
            }
            else
            {
                var subField = field.GetFirstSubField (code);
                if (ReferenceEquals (subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add (subField);
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

            if (string.IsNullOrEmpty (value))
            {
                field.RemoveSubField (code);
            }
            else
            {
                var subField = field.GetFirstSubField (code);
                if (ReferenceEquals (subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add (subField);
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
                        subField => subField.Code.IsOneOf (codes)
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
                .FilterSubFields (codes);
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
                .Where (field => field.Tag == tag)
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
                .Where (field => field.Tag.IsOneOf (tags))
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
                .GetField (tags)
                .GetOccurrence (occurrence);
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
                .Where (predicate)
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
            if (!ReferenceEquals (action, null))
            {
                foreach (var field in result)
                {
                    action (field);
                }
            }

            return result;
        }

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
            if (!ReferenceEquals (fieldAction, null)
                || !ReferenceEquals (subFieldAction, null))
            {
                foreach (var field in result)
                {
                    fieldAction?.Invoke (field);

                    if (!ReferenceEquals (subFieldAction, null))
                    {
                        foreach (var subField in field.Subfields)
                        {
                            subFieldAction (subField);
                        }
                    }
                }
            }

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
            if (!ReferenceEquals (action, null))
            {
                foreach (var field in result)
                {
                    foreach (var subField in field.Subfields)
                    {
                        action (subField);
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
                .Where (field => field.Subfields.Any (predicate))
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
                                    sub => sub.Code.SameChar (codes)
                                           && predicate (sub)
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
                                    sub => sub.Code.SameChar (codes)
                                           && sub.Value.SameString (values)
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
                                    sub => sub.Code.SameChar (code)
                                           && sub.Value.SameString (value)
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
                .Where (field => field.Tag.IsOneOf (tags))
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar (codes)
                                           && sub.Value.SameString (values)
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
                .Where (fieldPredicate)
                .Where (field => field.Subfields.Any (subPredicate))
                .ToArray();
        }

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
        }

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

                            return !string.IsNullOrEmpty (tag)
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
            =>
                fields
                    .GetFieldRegex (tagRegex)
                    .GetOccurrence (occurrence);

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                string textRegex
            )
            =>
                fields
                    .GetField (tags)
                    .Where
                        (
                            field =>
                            {
                                var value = field.Value;
                                return !ReferenceEquals (value, null) && value.Length != 0
                                                                      && Regex.IsMatch (value, textRegex);
                            })
                    .ToArray();

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
            =>
                fields
                    .GetFieldRegex (tags, textRegex)
                    .GetOccurrence (occurrence);

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
            var regex = new Regex (textRegex);
            return fields
                .GetField (tags)
                .Where (field => field.FilterSubFields (codes)
                    .Any (sub =>
                    {
                        var value = sub.Value;

                        return !ReferenceEquals (value, null) && value.Length != 0
                                                              && regex.IsMatch (value);
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
            =>
                fields
                    .GetFieldRegex (tags, codes, textRegex)
                    .GetOccurrence (occurrence);

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        public static string[] GetFieldValue (this IEnumerable<Field> fields)
            => fields
                .Select (field => field.Value!)
                .Where (line => !ReferenceEquals (line, null) && line.Length != 0)
                .ToArray();

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
                if (field.Tag == tag && !ReferenceEquals (field.Value, null) && field.Value.Length != 0)
                    result.Add (field.Value);

            return result.ToArray();
        }

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
        }

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
        }

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
        }

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
                if (field.Tag.IsOneOf (tags))
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
                if (subFields[i].Code.SameChar (code))
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
                    var subFields = field.Subfields;
                    var count = subFields.Count;
                    for (var i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.SameChar (code))
                        {
                            return subFields[i];
                        }
                    }
                }
            }

            return null;
        }

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
                if (subFields[i].Code.SameChar (code))
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
                        if (subField.Code.SameChar (code))
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
                if (subFields[i].Code.SameChar (code))
                {
                    result ??= new List<SubField>();
                    result.Add (subFields[i]);
                }
            }

            return ReferenceEquals (result, null)
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
                if (subFields[i].Code.SameChar (code))
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
                var subFields = field.Subfields;
                var count = subFields.Count;
                for (var i = 0; i < count; i++)
                {
                    if (subFields[i].Code.SameChar (code))
                    {
                        result ??= new List<SubField>();
                        result.Add (subFields[i]);
                    }
                }
            }

            return ReferenceEquals (result, null)
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
                    if (subFields[i].Code.IsOneOf (codes))
                    {
                        result ??= new List<SubField>();
                        result.Add (subFields[i]);
                    }
                }
            }

            return ReferenceEquals (result, null)
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
                        if (subField.Code.SameChar (code))
                        {
                            result ??= new List<SubField>();
                            result.Add (subField);
                        }
                    }
                }
            }

            return ReferenceEquals (result, null)
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
                        var subFields = field.Subfields;
                        var subCount = subFields.Count;
                        for (var j = 0; j < subCount; j++)
                        {
                            if (subFields[j].Code.SameChar (code))
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
                    var subFields = field.Subfields;
                    var subCount = subFields.Count;
                    for (var j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar (code))
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
                if (subFields[i].Code.SameChar (code))
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
                        if (subField.Code.SameChar (code)
                            && !ReferenceEquals (subField.Value, null) && subField.Value.Length != 0)
                        {
                            result.Add (subField.Value);
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
                if (subFields[i].Code.SameChar (code))
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
                if (subFields[i].Code.SameChar (code)
                    && subFields[i].Value.SameString (value))
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
                if (subFields[i].Code.SameChar (codes))
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
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar (code))
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
                if (subFields[i].Code.SameChar (codes))
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

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] NotNullValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields.Where (field => !ReferenceEquals (field.Value, null) && field.Value.Length != 0)
                .ToArray();
        }

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
                if (subField.Code.SameChar (code))
                {
                    if (subField.Value == oldValue)
                    {
                        subField.Value = newValue;
                    }
                }
            }

            return field;
        }

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
            var changed = string.CompareOrdinal (oldValue, newValue);

            if (changed != 0)
            {
                field.SetSubFieldValue (code, newValue);
            }

            return field;
        }

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
                    && !subField.Code.SameChar (knownCodes))
                {
                    result ??= new List<SubField>();
                    result.Add (subField);
                }
            }

            return ReferenceEquals (result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

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
            return fields.Where (field => ReferenceEquals (field.Value, null) || field.Value.Length == 0)
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithoutSubFields (this IEnumerable<Field> fields) =>
            fields.Where (field => field.Subfields.Count == 0).ToArray();

        /// <summary>
        /// Есть ли в поле подполя с кодами?
        /// </summary>
        public static bool HaveSubFields (this Field field)
        {
            foreach (var subfield in field.Subfields)
                if (subfield.Code != SubField.NoCode)
                    return true;

            return false;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithSubFields (this IEnumerable<Field> fields) =>
            fields.Where (field => field.HaveSubFields()).ToArray();

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

            throw new KeyNotFoundException ($"Tag={tag}");
        }

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField (this IEnumerable<Field> fields, int tag)
        {
            foreach (var field in fields)
                if (field.Tag == tag)
                    yield return field;
        }

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
                    if (field.HaveSubField (code))
                    {
                        yield return field;
                    }
                }
            }
        }

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
                    if (field.HaveSubField (code, value))
                    {
                        yield return field;
                    }
                }
            }
        }
    }
}
