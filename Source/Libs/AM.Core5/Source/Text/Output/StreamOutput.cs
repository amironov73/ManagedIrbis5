// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FileOutput.cs -- файловый вывод
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Text.Output;

/// <summary>
/// Output to stream.
/// </summary>
public sealed class StreamOutput
    : AbstractOutput
{
    #region Properties

    /// <summary>
    /// Inner writer.
    /// </summary>
    public TextWriter Writer => _writer;

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public StreamOutput
        (
            TextWriter writer
        )
    {
        _writer = writer;
        _ownWriter = false;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public StreamOutput
        (
            Stream stream,
            Encoding encoding
        )
    {
        _writer = new StreamWriter (stream, encoding);
        _ownWriter = true;
    }

    #endregion

    #region Private members

    private readonly TextWriter _writer;

    private readonly bool _ownWriter;

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
        HaveError = false;

        return this;
    }

    /// <summary>
    /// Конфигурирование объекта.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        Magna.Error
            (
                "StreamOutput::Configure: "
                + "not implemented"
            );

        throw new NotImplementedException();
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
        _writer.Write (text);

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
        _writer.Write (text);
        HaveError = true;

        return this;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public override void Dispose()
    {
        if (_ownWriter)
        {
            _writer.Dispose();
        }

        base.Dispose();
    }

    #endregion
}
