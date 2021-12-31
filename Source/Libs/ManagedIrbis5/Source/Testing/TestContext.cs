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

/* TestContext.cs -- context of test execution
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

using AM;
using AM.Text.Output;

#endregion

#nullable enable

namespace ManagedIrbis.Testing;

/// <summary>
/// Context of <see cref="AbstractTest"/> execution.
/// </summary>
public sealed class TestContext
{
    #region Properties

    /// <summary>
    /// Duration.
    /// </summary>
    [JsonPropertyName ("duration")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Test failed?
    /// </summary>
    [JsonPropertyName ("failed")]
    public bool Failed { get; set; }

    /// <summary>
    /// Finish time.
    /// </summary>
    [JsonPropertyName ("finish")]
    public DateTime FinishTime { get; set; }

    /// <summary>
    /// Name of the test.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Output channel.
    /// </summary>
    [JsonIgnore]
    public AbstractOutput Output { get; }

    /// <summary>
    /// Start time.
    /// </summary>
    [JsonPropertyName ("start")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Output text.
    /// </summary>
    [JsonPropertyName ("output")]
    public string Text => _text.ToString();

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public TestContext
        (
            AbstractOutput output
        )
    {
        Sure.NotNull (output);

        _text = new TextOutput();
        Output = new TeeOutput (output, _text);
    }

    #endregion

    #region Private members

    private readonly TextOutput _text;

    #endregion
}
