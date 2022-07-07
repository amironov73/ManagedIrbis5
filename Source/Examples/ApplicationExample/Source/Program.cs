// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.AppServices;

using Microsoft.Extensions.Logging;

#endregion

namespace ApplicationExample;

/// <summary>
/// Наше приложение.
/// </summary>
internal static class Program
{
    private static int ActualRun
        (
            MagnaApplication application
        )
    {
        var logger = application.Logger;

        logger.LogInformation ("Привет из приложения!");

        return 0;
    }

    public static int Main (string[] args)
    {
        return new MagnaApplication (args)
            .ConfigureCancelKey()
            .Run<MagnaApplication> (ActualRun);
    }
}
