// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftFunction.cs -- делегат для функции в PFT-скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Делегат для функции в PFT-скрипте (в т. ч. встроенной).
    /// </summary>
    public delegate void PftFunction
        (
            PftContext context,
            PftNode node,
            PftNode[] arguments
        );

} // namespace ManagedIrbis.Pft.Infrastructure
