// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DialogInfo.cs -- содержит сведения о диалоге и результате его отображения.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Содержит сведения о диалоге и результате его отображения.
/// </summary>
public record DialogInfo<TDialog>
    (
        TDialog Dialog,
        DialogResult Result
    );
