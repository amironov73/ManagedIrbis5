// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IThrottle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Threading.Tasks
{
    /// <summary>
    ///
    /// </summary>
    public interface IThrottle
    {
        /// <summary>
        /// Get next task.
        /// </summary>
        Task GetNext();

        /// <summary>
        /// Get next task.
        /// </summary>
        Task GetNext(out TimeSpan delay);

    } // interface IThrottle

} // namespace AM.Threading.Tasks
