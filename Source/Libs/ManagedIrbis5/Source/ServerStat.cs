// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ServerStat.cs -- статистика работы сервера ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Статистика работы сервера ИРБИС64.
/// </summary>
[XmlRoot ("stat")]
public sealed class ServerStat
{
    #region Properties

    /// <summary>
    /// Список активных клиентов.
    /// </summary>
    [XmlElement ("client")]
    [JsonPropertyName ("clients")]
    public ClientInfo[]? RunningClients { get; set; }

    /// <summary>
    /// Количество задействованных клиентских лицензий.
    /// </summary>
    public int ClientCount { get; set; }

    /// <summary>
    /// Общее количество выполненныъ команд с момента старта сервера.
    /// </summary>
    public int TotalCommandCount { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор ответа сервера.
    /// </summary>
    public static ServerStat Parse
        (
            Response response
        )
    {
        var result = new ServerStat
        {
            TotalCommandCount = response.RequireInt32(),
            ClientCount = response.RequireInt32(),
        };

        var linesPerClient = response.RequireInt32();
        var clients = new List<ClientInfo>();

        for (var i = 0; i < result.ClientCount; i++)
        {
            var lines = response.GetAnsiStrings (linesPerClient + 1);
            if (ReferenceEquals (lines, null))
            {
                break;
            }

            var client = new ClientInfo();
            if (lines.Length != 0)
            {
                client.Number = lines[0].EmptyToNull();
            }

            if (lines.Length > 1)
            {
                client.IPAddress = lines[1].EmptyToNull();
            }

            if (lines.Length > 2)
            {
                client.Port = lines[2].EmptyToNull();
            }

            if (lines.Length > 3)
            {
                client.Name = lines[3].EmptyToNull();
            }

            if (lines.Length > 4)
            {
                client.ID = lines[4].EmptyToNull();
            }

            if (lines.Length > 5)
            {
                client.Workstation = lines[5].EmptyToNull();
            }

            if (lines.Length > 6)
            {
                client.Registered = lines[6].EmptyToNull();
            }

            if (lines.Length > 7)
            {
                client.Acknowledged = lines[7].EmptyToNull();
            }

            if (lines.Length > 8)
            {
                client.LastCommand = lines[8].EmptyToNull();
            }

            if (lines.Length > 9)
            {
                client.CommandNumber = lines[9].EmptyToNull();
            }

            clients.Add (client);
        }

        result.RunningClients = clients.ToArray();

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = StringBuilderPool.Shared.Get();

        result.Append ($"Command executed: {TotalCommandCount}");
        result.AppendLine();
        result.Append ($"Running clients: {ClientCount}");
        result.AppendLine();
        if (RunningClients is not null)
        {
            foreach (var client in RunningClients)
            {
                result.AppendLine (client.ToString());
            }
        }

        return result.ReturnShared();
    }

    #endregion
}
