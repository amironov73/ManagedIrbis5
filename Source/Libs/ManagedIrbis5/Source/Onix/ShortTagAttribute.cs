// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ShortTagAttribute.cs -- атрибут, содержащий короткий вариант идентификатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Атрибут, содержащий короткий вариант идентификатора для XML-элемента.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage (AttributeTargets.Property)]
    public sealed class ShortTagAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Идентификатор для XML-элемента.
        /// </summary>
        public string Tag { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ShortTagAttribute
            (
                string tag
            )
        {
            Tag = tag;
        }

        #endregion
    }
}
