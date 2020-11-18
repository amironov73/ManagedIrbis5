// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DirectAccessMode.cs -- режимы прямого доступа к базам ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Режимы прямого доступа к базам ИРБИС64.
    /// </summary>
    public enum DirectAccessMode
    {
        /// <summary>
        /// Exclusive access mode.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Shared access mode.
        /// </summary>
        Shared,

        /// <summary>
        /// Read-only access mode.
        /// </summary>
        ReadOnly

    } // enum DirectAccessMode
}
