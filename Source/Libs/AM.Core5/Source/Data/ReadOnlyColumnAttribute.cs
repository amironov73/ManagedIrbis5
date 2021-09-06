// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ReadOnlyColumnAttribute.cs -- помечает колонку как не подлежащую редактированию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Помечает колонку как не подлежащую редактированию.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class ReadOnlyColumnAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Признак колонки только для чтения.
        /// </summary>
        public bool ReadOnly { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ReadOnlyColumnAttribute()
            : this (true)
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReadOnlyColumnAttribute
            (
                bool readOnly
            )
        {
            ReadOnly = readOnly;
        } // constructor

        #endregion

    } // class ReadOnlyColumnAttribute

} // namespace AM.Windows.Forms
