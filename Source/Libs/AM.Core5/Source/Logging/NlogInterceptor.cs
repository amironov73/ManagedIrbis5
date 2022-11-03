// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* NLogInterceptor.cs -- перехватчик для NLog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using NLog.LayoutRenderers.Wrappers;

#endregion

#nullable enable

namespace AM.Logging;

/// <summary>
/// Перехватчик для NLog.
/// </summary>
public sealed class NlogInterceptor
    : WrapperLayoutRendererBase
{
    #region Private members

    private static bool _alreadyInstalled;
    private static EventHandler<string>? Handlers;

    #endregion

    #region Public methods

    /// <summary>
    /// Регистрация обработчика.
    /// </summary>
    public static void RegisterHandler
        (
            EventHandler<string> handler
        )
    {
        if (_alreadyInstalled)
        {
            return;
        }

        Register<NlogInterceptor> (nameof (NlogInterceptor));
        Handlers += handler;

        _alreadyInstalled = true;
    }

    /// <summary>
    /// Разрегистрация обработчика.
    /// </summary>
    public static void UnregisterHandler
        (
            EventHandler<string> handler
        )
    {
        Handlers -= handler;
    }

    #endregion

    #region WrapperLayoutRenderBase members

    /// <inheritdoc cref="WrapperLayoutRendererBase.Transform(string)"/>
    protected override string Transform
        (
            string text
        )
    {
        Handlers?.Invoke (this, text);

        return text;
    }

    #endregion
}
