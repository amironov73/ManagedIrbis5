// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable RedundantAssignment
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#pragma warning disable 649

/* BatchFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Batch
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BatchFactory
    {
        #region Constants

        /// <summary>
        /// Simple formatter/reader.
        /// </summary>
        public const string Simple = "simple";

        /// <summary>
        /// Parallel formatter/reader.
        /// </summary>
        public const string Parallel = "parallel";

        #endregion

        #region Public methods

        /// <summary>
        /// Get batch reader.
        /// </summary>
        public IEnumerable<Record> GetBatchReader
            (
                string kind,
                string connectionString,
                string database,
                IEnumerable<int> range
            )
        {
            IEnumerable<Record> result;

            switch (kind)
            {
                case Parallel:
                    result = new ParallelRecordReader
                        (
                            -1,
                            connectionString,
                            range.ToArray()
                        );
                    break;

                default:
                    // TODO: implement
                    throw new NotImplementedException();

                    // result = new BatchRecordReader
                    //     (
                    //         connectionString,
                    //         database,
                    //         1000,
                    //         true,
                    //         range
                    //     );
                    // break;
            }

            return result;
        }

        /// <summary>
        /// Get batch formatter.
        /// </summary>
        public IEnumerable<string> GetFormatter
            (
                string kind,
                string connectionString,
                string database,
                string format,
                IEnumerable<int> range
            )
        {
            IEnumerable<string> result;

            switch (kind)
            {
                case "parallel":
                    result = new ParallelRecordFormatter
                        (
                            -1,
                            connectionString,
                            range.ToArray(),
                            format
                        );
                    break;

                default:
                    result = new BatchRecordFormatter
                        (
                            connectionString,
                            database,
                            format,
                            1000,
                            range
                        );
                    break;
            }

            return result;
        }

        #endregion

    } // class BatchFactory

} // namespace ManagedIrbis.Batch
