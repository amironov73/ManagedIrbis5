// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* TextMonitoringSink.cs -- сбор мониторинга в текстовый поток
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Json;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Сбор мониторинга в текстовый поток.
/// </summary>
public sealed class TextMonitoringSink
    : MonitoringSink
{
    #region Public members

    /// <summary>
    /// Текстовый поток.
    /// </summary>
    public TextWriter Writer { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextMonitoringSink
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        Writer = writer;
    }

    #endregion

    #region MonitoringSink members

    /// <inheritdoc cref="MonitoringSink.WriteData" />
    public override bool WriteData
        (
            MonitoringData data
        )
    {
        Sure.NotNull (data);

        try
        {
            var text = JsonUtility.SerializeShort (data);
            Writer.WriteLine (text);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (TeeMonitoringSink) + "::" + nameof (WriteData)
                );

            return false;
        }

        return true;
    }

    #endregion
}
