// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EmbeddedField.cs -- работа со встроенными полями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Работа со встроенными полями.
    /// </summary>
    public static class EmbeddedField
    {
        #region Constants

        /// <summary>
        /// Код по умолчанию, используемый для встраивания полей.
        /// </summary>
        public const char DefaultCode = '1';

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива встроенных полей.
        /// </summary>
        public static Field[] GetEmbeddedFields
            (
                IEnumerable<SubField> subfields,
                char sign = DefaultCode
            )
        {
            var result = new List<Field>();
            Field? found = null;

            foreach (var subField in subfields)
            {
                if (subField.Code == sign)
                {
                    if (found is not null)
                    {
                        result.Add(found);
                    }

                    var value = subField.Value;
                    if (value.IsEmpty())
                    {
                        Magna.Error
                            (
                                nameof(EmbeddedField) + "::" + nameof(GetEmbeddedFields)
                                + ": bad format"
                            );

                        throw new FormatException();
                    }

                    var tag = int.Parse(value.AsSpan()[..3]);
                    found = new Field { Tag = tag };
                    if (tag < 10 && value.Length > 3)
                    {
                        found.Value = value.Substring(3);
                    }
                }
                else
                {
                    found?.Add(subField.Code, subField.Value);
                }
            }

            if (!ReferenceEquals(found, null))
            {
                result.Add(found);
            }

            return result.ToArray();

        } // method GetEmbeddedFields

        /// <summary>
        /// Получение встроенных полей с указанным тегом.
        /// </summary>
        public static Field[] GetEmbeddedField(this Field field, int tag)
            => GetEmbeddedFields(field.Subfields).GetField(tag);

        #endregion

    } // class EmbeddedField

} // namespace ManagedIrbis
