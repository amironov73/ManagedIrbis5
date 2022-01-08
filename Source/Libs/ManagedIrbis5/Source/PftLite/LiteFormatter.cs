// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LiteFormatter.cs -- облегченный форматтер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Облегченный форматтер.
/// </summary>
public sealed class LiteFormatter
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LiteFormatter()
    {
        _context = new PftContext();
        _program = Array.Empty<PftNode>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="format">Формат.</param>
    public LiteFormatter
        (
            string format
        )
        : this()
    {
        Sure.NotNull (format);

        SetFormat (format);
    }

    #endregion

    #region Private members

    private readonly PftContext _context;
    private PftNode[] _program;

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп скрипта.
    /// </summary>
    public string Dump()
    {
        var builder = new StringBuilder();
        foreach (var node in _program)
        {
            builder.AppendLine (node.ToString());
        }

        return builder.ToString();
    }

    /// <summary>
    /// Расформатирование записи.
    /// </summary>
    public string FormatRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        _context.Reset();
        _context.Record = record;
        foreach (var node in _program)
        {
            node.Execute (_context);
        }

        return _context.Output.ToString();
    }

    /// <summary>
    /// Установка формата.
    /// </summary>
    public void SetFormat
        (
            string format
        )
    {
        Sure.NotNull (format);

        _program = Grammar.Parse (format).ToArray();
    }

    #endregion
}
