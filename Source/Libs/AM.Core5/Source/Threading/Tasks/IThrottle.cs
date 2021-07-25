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
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local

/* IThrottle.cs -- интерфейс дросселя (ограничителя)
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
    /// Интерфейс дросселя, т. е. устройства, ограничивающего
    /// пропускание задач.
    /// </summary>
    public interface IThrottle
    {
        /// <summary>
        /// Получение следующей задачи.
        /// </summary>
        Task GetNext();

        /// <summary>
        /// Получение следующей задачи.
        /// </summary>
        Task GetNext (out TimeSpan delay);

    } // interface IThrottle

} // namespace AM.Threading.Tasks
