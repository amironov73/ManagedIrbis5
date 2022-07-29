// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ServerResponse.cs -- ответ сервера на притязания клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server;

/// <summary>
/// Ответ сервера на притязания клиента.
/// </summary>
public sealed class ServerResponse
{
    #region Properties

    /// <summary>
    /// Memory.
    /// </summary>
    public MemoryStream Memory { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor for mocking.
    /// </summary>
    public ServerResponse()
    {
        // To make Resharper happy
        Memory = new MemoryStream();
        _prefix = new MemoryStream();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ServerResponse
        (
            ClientRequest request
        )
    {
        Memory = new MemoryStream();

        WriteAnsiString (request.CommandCode1).NewLine();
        WriteAnsiString (request.ClientId).NewLine();
        WriteAnsiString (request.CommandNumber).NewLine();

        _prefix = Memory;
        Memory = new MemoryStream();

        // Тут должен идти размер ответа в байтах,
        // мы сформируем его в последнюю очередь

        // Для команды A может быть строка с версией сервера

        // Пять пустых переводов строки
        NewLine().NewLine().NewLine().NewLine().NewLine();

        // Дальше -- код возврата
    }

    #endregion

    #region Private members

    private readonly MemoryStream _prefix;

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование ответа.
    /// </summary>
    public ReadOnlyMemory<byte>[] Encode
        (
            string? version
        )
    {
        var ansi = IrbisEncoding.Ansi;
        var result = new ReadOnlyMemory<byte>[4];
        result[0] = _prefix.ToArray();
        result[1] = ansi.GetBytes (FastNumber.Int64ToString (Memory.Length) + "\r\n");
        if (string.IsNullOrEmpty (version))
        {
            result[2] = new byte[] { 0x0D, 0x0A };
        }
        else
        {
            result[2] = ansi.GetBytes (version + "\r\n");
        }

        result[3] = Memory.ToArray();

        return result;
    } // method Encode

    /// <summary>
    /// Write line break.
    /// </summary>
    public ServerResponse NewLine()
    {
        Memory.WriteByte (0x0D);
        Memory.WriteByte (0x0A);

        return this;
    }

    /// <summary>
    /// Write ANSI string.
    /// </summary>
    public ServerResponse WriteAnsiString
        (
            string? line
        )
    {
        if (!string.IsNullOrEmpty (line))
        {
            var bytes = IrbisEncoding.Ansi.GetBytes (line);
            Memory.Write (bytes, 0, bytes.Length);
        }

        return this;
    }

    /// <summary>
    /// Write integer.
    /// </summary>
    public ServerResponse WriteInt32
        (
            int value
        )
    {
        var line = value.ToInvariantString();

        return WriteAnsiString (line);
    }

    /// <summary>
    /// Write UTF string.
    /// </summary>
    public ServerResponse WriteUtfString
        (
            string? line
        )
    {
        if (!string.IsNullOrEmpty (line))
        {
            var bytes = IrbisEncoding.Utf8.GetBytes (line);
            Memory.Write (bytes, 0, bytes.Length);
        }

        return this;
    }

    #endregion
}
