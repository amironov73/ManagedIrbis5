// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TimestampedOutput.cs -- output that appends timestamp
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text.Output;

/// <summary>
/// Output that appends timestamp.
/// </summary>
public sealed class TimestampedOutput
    : AbstractOutput
{
    #region Constants

    /// <summary>
    /// Default format.
    /// </summary>
    public const string DefaultFormat = "G";

    #endregion

    #region Properties

    /// <summary>
    /// Inner output.
    /// </summary>
    public AbstractOutput InnerOutput { get; private set; }

    /// <summary>
    /// Format
    /// </summary>
    public string? Format { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public TimestampedOutput
        (
            AbstractOutput innerOutput
        )
    {
        InnerOutput = innerOutput;
    }

    #endregion

    #region Private members

    private string _GetPrefix()
    {
        string result = DateTime.Now.ToString (Format) + ": ";

        return result;
    }

    #endregion

    #region Public methods

    #endregion

    #region AbstractOutput members

    /// <summary>
    /// Флаг: был ли вывод с помощью WriteError.
    /// </summary>
    public override bool HaveError { get; set; }

    /// <summary>
    /// Очищает вывод, например, окно.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput Clear()
    {
        InnerOutput.Clear();

        return this;
    }

    /// <summary>
    /// Configures the specified configuration.
    /// </summary>
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        InnerOutput.Configure (configuration);

        return this;
    }

    /// <summary>
    /// Метод, который нужно переопределить
    /// в потомке.
    /// </summary>
    public override AbstractOutput Write
        (
            string text
        )
    {
        InnerOutput.Write (_GetPrefix());
        InnerOutput.Write (text);

        return this;
    }

    /// <summary>
    /// Выводит ошибку. Например, красным цветом.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput WriteError
        (
            string text
        )
    {
        InnerOutput.WriteError (_GetPrefix());
        InnerOutput.WriteError (text);

        return this;
    }

    #endregion
}
