// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ProcessInfo.cs -- информация о процессе на сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Информация о процессе на сервере ИРБИС64.
    /// </summary>
    public sealed class ProcessInfo
        : IEquatable<ProcessInfo>
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Workstation { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Started { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? LastCommand { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? CommandNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? ProcessId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static ProcessInfo[] Parse
            (
                Response response
            )
        {
            var lines = response.ReadRemainingAnsiLines();
            var processCount = int.Parse(lines[0]);
            var linesPerProcess = int.Parse(lines[1]);
            var result = new List<ProcessInfo>(processCount);
            if (processCount == 0 || linesPerProcess == 0)
            {
                return result.ToArray();
            }

            for (int i = 2; i < lines.Length; i += linesPerProcess + 1)
            {
                if ((i + linesPerProcess) > lines.Length)
                {
                    break;
                }

                var process = new ProcessInfo
                {
                    Number = lines[i + 0],
                    IpAddress = lines[i + 1],
                    Name = lines[i + 2],
                    ClientId = lines[i + 3],
                    Workstation = lines[i + 4],
                    Started = lines[i + 5],
                    LastCommand = lines[i + 6],
                    CommandNumber = lines[i + 7],
                    ProcessId = lines[i + 8],
                    State = lines[i + 9]
                };
                result.Add(process);
            }

            return result.ToArray();

        } // method Parse

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(ProcessInfo? other) => Number?.Equals(other?.Number) ?? false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"{Number} {IpAddress} {Name} {Workstation}";

        #endregion

    } // class ProcessInfo

} // namespace ManagedIrbis.Infrastructure
