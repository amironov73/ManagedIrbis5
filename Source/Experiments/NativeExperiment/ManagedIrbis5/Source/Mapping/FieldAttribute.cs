// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public sealed class FieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Тег.
        /// </summary>
        public int Tag { get; }

        /// <summary>
        /// Код подполя (опциональный).
        /// </summary>
        public char Code { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldAttribute
            (
                int tag
            )
        {
            Tag = tag;
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldAttribute
            (
                int tag,
                char code
            )
        {
            Tag = tag;
            Code = code;
        } // constructor

        #endregion

    } // class FieldAttribute

} // namespace ManagedIrbis.Mapping
