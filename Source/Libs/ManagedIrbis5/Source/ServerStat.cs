﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ServerStat.cs -- IRBIS server stat
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Статистика работы сервера ИРБИС64.
    /// </summary>
    [XmlRoot("stat")]
    public sealed class ServerStat
    {
        #region Properties

        /// <summary>
        /// List of running client.
        /// </summary>
        [XmlElement("client")]
        [JsonPropertyName("clients")]
        public ClientInfo[]? RunningClients { get; set; }

        /// <summary>
        /// Current client count.
        /// </summary>
        public int ClientCount { get; set; }

        /// <summary>
        /// Total commands executed since server start.
        /// </summary>
        public int TotalCommandCount { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
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

            for(var i = 0; i < result.ClientCount; i++)
            {
                var lines = response.GetAnsiStrings(linesPerClient + 1);
                if (ReferenceEquals(lines, null))
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

                clients.Add(client);
            }

            result.RunningClients = clients.ToArray();

            return result;

        } // method Parse

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendFormat("Command executed: {0}", TotalCommandCount);
            result.AppendLine();
            result.AppendFormat("Running clients: {0}", ClientCount);
            result.AppendLine();
            if (!ReferenceEquals(RunningClients, null))
            {
                foreach (ClientInfo client in RunningClients)
                {
                    result.AppendLine(client.ToString());
                }

            }

            return result.ToString();

        } // method ToString

        #endregion

    } // class ServerStat

} // namespace ManagedIrbis
