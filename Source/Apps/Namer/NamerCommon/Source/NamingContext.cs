// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NamingContext.cs -- контекст переименования файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Контекст переименования файлов.
/// </summary>
[PublicAPI]
public sealed class NamingContext
{
    #region Properties

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    public Dictionary<string, object?> UserData { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NamingContext()
    {
        UserData = new ();
    }

    #endregion
}
