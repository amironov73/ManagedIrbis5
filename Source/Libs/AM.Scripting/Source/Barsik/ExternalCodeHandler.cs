// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ExternalCodeHandler.cs -- обработчик внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

using AM.Text;

using Sprache;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Обработчик внешнего кода.
    /// </summary>
    public delegate void ExternalCodeHandler
        (
            Context context,
            ExternalNode node
        );
}
