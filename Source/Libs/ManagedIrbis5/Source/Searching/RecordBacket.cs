// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RecordBacket.cs -- список ссылок на найденные записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Список ссылок на найденные записи.
/// </summary>
public class RecordBacket
    : NonNullCollection<RecordReference>
{
    #region Properties

    /// <summary>
    /// Было что-нибудь найдено?
    /// </summary>
    public bool Success => Count != 0;

    #endregion
}
