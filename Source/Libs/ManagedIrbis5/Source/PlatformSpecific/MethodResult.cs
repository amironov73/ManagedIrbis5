// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MethodResult.cs -- результат вызова внешнего метода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Результат вызова внешнего метода.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed class MethodResult
    {
        #region Properties

        /// <summary>
        /// Return code.
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        public string? Input { get; set; }

        /// <summary>
        /// Output.
        /// </summary>
        public string? Output { get; set; }

        #endregion

    } // class MethodResult

} // namespace ManagedIrbis.PlatformSpecific
