// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ColumnWidthAttribute.cs -- задает ширину колонки в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Задает ширину колонки в гриде.
    /// </summary>
    [Serializable]
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class ColumnWidthAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Ширина колонки.
        /// </summary>
        public int Width { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColumnWidthAttribute
            (
                int width
            )
        {
            Width = width;
        } // constructor

        #endregion

    } // class ColumnWidthAttribute

} // namespace AM.Windows.Forms
