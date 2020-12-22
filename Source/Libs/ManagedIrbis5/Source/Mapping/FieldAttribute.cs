// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FieldAttribute.cs -- задает отображение поля записи на свойство класса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Задаёт отображение поля записи на свойство класса.
    /// </summary>
    [DebuggerDisplay("Tag: {" + nameof(Tag) + "}")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Тег.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Повторение.
        /// </summary>
        public int Occurrence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute
            (
                int tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute
            (
                int tag,
                char code
            )
        {
            Tag = tag;
            Code = code;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldAttribute
            (
                int tag,
                int occurrence
            )
        {
            Tag = tag;
            Occurrence = occurrence;
        }

        #endregion

    } // class FieldAttribute

} // namespace ManagedIrbis.Mapping
