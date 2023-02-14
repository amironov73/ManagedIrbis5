// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SharedStringPoolProvider.cs -- провайдер общего пула строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Провайдер общего пула строк <see cref="StringPool"/>.
/// </summary>
[PublicAPI]
public sealed class SharedStringPoolProvider
    : IStringPoolProvider
{
    #region IStringPoolProvider members

    /// <inheritdoc cref="IStringPoolProvider.GetStringPool"/>
    public StringPool GetStringPool() => StringPool.Shared;

    /// <inheritdoc cref="IStringPoolProvider.ReturnStringPool"/>
    public void ReturnStringPool
        (
            StringPool stringPool
        )
    {
        stringPool.NotUsed();
    }

    #endregion
}
