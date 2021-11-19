// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* OnixTags.cs -- сопоставление референсных и коротких тегов типам данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Сопоставление референсных и коротких тегов типам данных.
    /// </summary>
    public sealed class OnixTags
    {
        #region Properties

        /// <summary>
        /// Тип данных.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Рефренсный тег.
        /// </summary>
        public string Reference { get; }

        /// <summary>
        /// Короткий тег.
        /// </summary>
        public string Short { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public OnixTags
            (
                Type type,
                string reference,
                string @short
            )
        {
            Type = type;
            Reference = reference;
            Short = @short;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Type}: {Reference}, {Short}";
        }

        #endregion
    }
}
