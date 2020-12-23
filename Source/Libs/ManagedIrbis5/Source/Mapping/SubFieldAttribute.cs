// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* SubFieldAttribute.cs -- задает отображение подполя записи на свойство класса
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
    /// Задаёт отображение подполя записи на свойство класса.
    /// </summary>
    [DebuggerDisplay("Code: {" + nameof(Code) + "}")]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public sealed class SubFieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Код.
        /// </summary>
        public char Code { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SubFieldAttribute
            (
                char code
            )
        {
            Code = code;
        } // constructor

        #endregion

    } // class SubFieldAttribute

} // namespace ManagedIrbis.Mapping
