// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AttentiveWrite.cs -- внимательный поток, замечающий, когда в него выводят текст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Внимательный поток, замечающий, когда в него выводят текст.
/// </summary>
public sealed class AttentiveWriter
    : TextWriter
{
    #region Properties

    /// <summary>
    /// Количество выведенных символов.
    /// </summary>
    public uint Counter => _counter;

    /// <summary>
    /// Отслеживаемый поток символов.
    /// </summary>
    public TextWriter Inner { get; }

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding => Inner.Encoding;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AttentiveWriter
        (
            TextWriter inner
        )
    {
        Sure.NotNull (inner);

        Inner = inner;
    }

    #endregion

    #region Private members

    private uint _counter;

    #endregion

    #region Public methods

    /// <summary>
    /// Сброс счетчика.
    /// </summary>
    public void ResetCounter()
    {
        _counter = 0;
    }

    #endregion

    #region TextWriter members

    /// <inheritdoc cref="TextWriter.Write(char)"/>
    public override void Write
        (
            char value
        )
    {
        ++_counter;
        Inner.Write (value);
    }

    #endregion
}
