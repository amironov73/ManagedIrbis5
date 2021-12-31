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

/* AbstractTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text.Output;

#endregion

#nullable enable

namespace ManagedIrbis.Testing;

/// <summary>
/// Abstract test.
/// </summary>
public abstract class AbstractTest
{
    #region Properties

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider? Connection { get; set; }

    /// <summary>
    /// Output.
    /// </summary>
    public AbstractOutput? Output { get; set; }

    /// <summary>
    /// Path to test data.
    /// </summary>
    public string? DataPath { get; set; }

    /// <summary>
    /// Test execution context.
    /// </summary>
    public TestContext? Context { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Write some text.
    /// </summary>
    public void Write
        (
            string text
        )
    {
        if (Output is null)
        {
            return;
        }

        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        Output.Write (text);
    }

    /// <summary>
    /// Write some object.
    /// </summary>
    public void Write
        (
            object? obj
        )
    {
        var text = obj.ToVisibleString();

        Write (text);
    }

    /// <summary>
    /// Write some error text.
    /// </summary>
    public void WriteError
        (
            string? text
        )
    {
        if (Output is null)
        {
            return;
        }

        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        Output.WriteError (text);
    }

    #endregion
}
