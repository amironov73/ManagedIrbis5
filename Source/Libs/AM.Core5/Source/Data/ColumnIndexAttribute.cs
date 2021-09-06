// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ColumnIndexAttribute.cs -- задает порядок колонок в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Задает порядок колонок в гриде.
    /// </summary>
    [Serializable]
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class ColumnIndexAttribute
        : Attribute
    {
        #region Properties

        ///<summary>
        /// Индекс колонки.
        ///</summary>
        public int Index { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColumnIndexAttribute
            (
                int index
            )
        {
            Index = index;
        } // constructor

        #endregion

    } // class ColumnIndexAttribute

} // namespace AM.Windows.Forms
