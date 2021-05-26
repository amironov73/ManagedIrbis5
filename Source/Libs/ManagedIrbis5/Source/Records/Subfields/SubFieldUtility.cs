// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* SubFieldUtility.cs -- утилиты для работы с подполями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Утилиты для работы с подполями.
    /// </summary>
    public static class SubFieldUtility
    {
        #region Public methods

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
                if (subField.Code.SameChar(code))
                {
                    return subField;
                }
            }

            return null;
        } // method GetFirstSubField

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
                if (codes.Contains(subField.Code))
                {
                    return subField;
                }
            }

            return null;
        } // method GetFirstSubField

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
                if (subField.Code.SameChar(code)
                    && subField.Value.SameString(value))
                {
                    return subField;
                }
            }

            return null;
        } // method GetFirstSubField

        // ==========================================================

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
                if (subField.Code.SameChar(code))
                {
                    result ??= new List<SubField>();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        } // method GetSubField

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
                if (subField.Code.SameChar(codes))
                {
                    result ??= new();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        } // method GetSubField

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

            if (!ReferenceEquals(action, null))
            {
                foreach (var subField in result)
                {
                    action(subField);
                }
            }

            return result;
        } // method GetSubField

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
                if (fieldPredicate(field))
                {
                    foreach (SubField subField in field.Subfields)
                    {
                        if (subPredicate(subField))
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
        } // method GetSubField

        /*

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes
            )
        {
            Sure.NotNull(fields, nameof(fields));
            Sure.NotNull(tags, nameof(tags));
            Sure.NotNull(codes, nameof(codes));

            return fields
                .GetField(tags)
                .GetSubField(codes)
                .ToArray();
        } // method GetSubField

        */

        // ==========================================================

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
                if (!value.IsEmpty())
                {
                    result ??= new List<string>();
                    result.Add(value);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<string>()
                : result.ToArray();
        } // method GetSubFieldValue

        // ==========================================================

        /*

        /// <summary>
        /// Convert the subfield to <see cref="JObject"/>.
        /// </summary>
        public static JObject ToJObject
            (
                this SubField subField
            )
        {
            Sure.NotNull(subField, nameof(subField));

            JObject result = JObject.FromObject(subField);

            return result;
        }

        /// <summary>
        /// Convert the subfield to JSON.
        /// </summary>
        public static string ToJson
            (
                this SubField subField
            )
        {
            Sure.NotNull(subField, nameof(subField));

            string result = JsonUtility.SerializeShort(subField);
            //JObject.FromObject(subField).ToString();

            return result;
        }

        /// <summary>
        /// Restore subfield from <see cref="JObject"/>.
        /// </summary>
        public static SubField FromJObject
            (
                JObject jObject
            )
        {
            Sure.NotNull(jObject, nameof(jObject));

            SubField result = new SubField
                (
                    jObject["code"].ToString()[0]
                );
            JToken value = jObject["value"];
            if (value != null)
            {
                result.Value = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Restore subfield from JSON.
        /// </summary>
        public static SubField FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            SubField result = JsonConvert.DeserializeObject<SubField>(text);

            return result;
        }
        */

        #endregion

    } // class SubFieldUtility

} // namespace ManagedIrbis
