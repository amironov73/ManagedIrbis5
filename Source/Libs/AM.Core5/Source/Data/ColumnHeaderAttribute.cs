// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ColumnHeaderAttribute.cs -- задает заголовок колонки в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Задает заголовок колонки в гриде.
    /// </summary>
    [Serializable]
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class ColumnHeaderAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Заголовок колонки.
        /// </summary>
        public string Header { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColumnHeaderAttribute
            (
                string header
            )
        {
            Header = header;
        } // constructor

        #endregion

    } // class ColumnHeaderAttribute

} // namespace AM.Windows.Forms
