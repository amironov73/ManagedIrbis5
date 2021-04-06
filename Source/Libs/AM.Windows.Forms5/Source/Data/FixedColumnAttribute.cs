// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* FixedColumnAttribute.cs -- помечает колонку в гриде как фиксированную
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Помечает колонку в гриде как фиксированную.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class FixedColumnAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Признак фиксированной колонки.
        /// </summary>
        public bool Fixed { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public FixedColumnAttribute()
            : this(true)
        {
        } // constructor

        /// <summary>
        /// Конструктор
        /// </summary>
        public FixedColumnAttribute
            (
                bool isFixed
            )
        {
            Fixed = isFixed;
        } // constructor

        #endregion

    } // class FixedColumnAttribute

} // namespace AM.Windows.Forms
