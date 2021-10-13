// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* Variable.cs -- переменная программы-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Переменная программы-скрипта.
    /// </summary>
    public sealed class Variable
    {
        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Значение переменной.
        /// </summary>
        public dynamic? Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Variable
            (
                string name
            )
        {
            Name = name;

        } // constructor

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Name}: {Value}";

        #endregion

    } // class Variable

} // namespace SimplestLanguage
