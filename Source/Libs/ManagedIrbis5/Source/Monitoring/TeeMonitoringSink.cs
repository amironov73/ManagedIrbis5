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

/* TeeMonitoringSink.cs -- sink delegating to collection of sink
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Sink delegating to collection of sink.
/// </summary>
public sealed class TeeMonitoringSink
    : MonitoringSink
{
    #region Properties

    /// <summary>
    /// Collection of sinks.
    /// </summary>
    public NonNullCollection<MonitoringSink> Sinks { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public TeeMonitoringSink()
    {
        Sinks = new NonNullCollection<MonitoringSink>();
    }

    #endregion

    #region MonitoringSink members

    /// <inheritdoc cref="MonitoringSink.WriteData" />
    public override bool WriteData
        (
            MonitoringData data
        )
    {
        var result = true;

        foreach (var sink in Sinks)
        {
            try
            {
                if (!sink.WriteData (data))
                {
                    result = false;
                    break;
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof (TeeMonitoringSink) + "::" + nameof (WriteData),
                        exception
                    );
            }
        }

        return result;
    }

    #endregion
}
