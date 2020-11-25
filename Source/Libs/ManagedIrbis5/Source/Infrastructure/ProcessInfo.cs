// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ProcessInfo.cs -- информация о процессе на сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Информация о процессе на сервере ИРБИС64.
    /// </summary>
    public sealed class ProcessInfo
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
        ///
        /// </summary>
        public static ProcessInfo[] Parse
            (
                Response response
            )
        {
            var lines = response.ReadRemainingAnsiLines();
            var result = new LocalList<ProcessInfo>();
            var processCount = int.Parse(lines[0]);
            var linesPerProcess = int.Parse(lines[1]);
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
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"{Number} {IpAddress} {Name} {Workstation}";
        }

        #endregion
    }
}