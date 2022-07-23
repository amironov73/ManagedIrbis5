// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MethodRunner.cs -- запускает методы из динамических библиотек
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AM;

using Microsoft.Extensions.Logging;

#endregion

namespace ManagedIrbis.PlatformSpecific;

/// <summary>
/// Умеет запускать методы из динамических библиотек.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class MethodRunner
{
    #region Properties

    /// <summary>
    /// Buffer size, bytes.
    /// </summary>
    public static int BufferSize = 100 * 1024;

    #endregion

    #region Public methods

    /// <summary>
    /// Run process and get its output.
    /// </summary>
    public static MethodResult RunMethod
        (
            string dllName,
            string methodName,
            bool winApi,
            string input
        )
    {
        Sure.NotNullNorEmpty (dllName);
        Sure.NotNullNorEmpty (methodName);

        var result = new MethodResult
        {
            Input = input
        };

        var buffer = new StringBuilder (BufferSize);
        buffer.Append (input); // ???

        try
        {
            if (winApi)
            {
                var winapiPlugin
                    = (WinapiPluginFunction)DllCache
                        .CreateDelegate
                            (
                                dllName,
                                methodName,
                                typeof (WinapiPluginFunction)
                            );
                result.ReturnCode = winapiPlugin
                    (
                        input,
                        buffer,
                        buffer.Capacity
                    );
            }
            else
            {
                var cdeclPlugin
                    = (CdeclPluginFunction)DllCache
                        .CreateDelegate
                            (
                                dllName,
                                methodName,
                                typeof (CdeclPluginFunction)
                            );
                result.ReturnCode = cdeclPlugin
                    (
                        input,
                        buffer,
                        buffer.Capacity
                    );
            }

            result.Output = buffer.ToString();
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (MethodRunner) + "::" + nameof (RunMethod)
                );
        }

        return result;
    }

    #endregion
}
