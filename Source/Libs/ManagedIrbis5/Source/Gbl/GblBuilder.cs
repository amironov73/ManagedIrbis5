// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* GblBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// <para>Инструмент для упрощённого построения заданий на
    /// глобальную корректировку.</para>
    /// <para>Пример построения и выполнения задания:</para>
    /// <code>
    /// GblResult result = new GblBuilder()
    ///        .Add("3079", "'1'")
    ///        .Delete("3011")
    ///        .Execute
    ///             (
    ///                 connection,
    ///                 "IBIS",
    ///                 new[] {30, 32, 34}
    ///             );
    /// Console.WriteLine
    ///     (
    ///         "Processed {0} records",
    ///         result.RecordsProcessed
    ///     );
    /// foreach (ProtocolLine line in result.Protocol)
    /// {
    ///     Console.WriteLine(line);
    /// }
    /// </code>
    /// </summary>
    public sealed class GblBuilder
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblBuilder()
        {
            _statements = new List<GblStatement>();
        }

        #endregion

        #region Private members

        private const string Filler = "XXXXXXXXXXXXXXXXX";
        private const string All = "*";

        private readonly List<GblStatement> _statements;

        #endregion

        #region Public methods

        /// <summary>
        /// Add an arbitrary statement.
        /// </summary>
        public GblBuilder AddStatement
            (
                string code,
                string? parameter1,
                string? parameter2,
                string? format1,
                string? format2
            )
        {
            GblStatement item = new GblStatement
            {
                Command = VerifyCode(code),
                Parameter1 = parameter1,
                Parameter2 = parameter2,
                Format1 = format1,
                Format2 = format2
            };
            _statements.Add(item);

            return this;
        }

        /// <summary>
        /// Command "ADD".
        /// </summary>
        public GblBuilder Add
            (
                string field,
                string value
            )
        {
            return AddStatement
                (
                    GblCode.Add,
                    VerifyField(field),
                    All,
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// Command "ADD".
        /// </summary>
        public GblBuilder Add
            (
                string field,
                string repeat,
                string value
            )
        {
            return AddStatement
                (
                    GblCode.Add,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// Command "CHA".
        /// </summary>
        public GblBuilder Change
            (
                string field,
                string fromValue,
                string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Change,
                    VerifyField(field),
                    All,
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// Command "CHA".
        /// </summary>
        public GblBuilder Change
            (
                string field,
                string repeat,
                string fromValue,
                string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Change,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// Command "DEL".
        /// </summary>
        public GblBuilder Delete
            (
                string field,
                string repeat
            )
            =>
            AddStatement
            (
                GblCode.Delete,
                VerifyField(field),
                VerifyRepeat(repeat),
                Filler,
                Filler
            );

        /// <summary>
        /// Command "DEL".
        /// </summary>
        public GblBuilder Delete
            (
                string field
            )
            =>
            AddStatement
            (
                GblCode.Delete,
                VerifyField(field),
                All,
                Filler,
                Filler
            );

        /// <summary>
        /// Command "DELR".
        /// </summary>
        public GblBuilder DeleteRecord() =>
            AddStatement
            (
                GblCode.DeleteRecord,
                Filler,
                Filler,
                Filler,
                Filler
            );

        /// <summary>
        /// Execute the GBL on the given database.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                string database
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessWholeDatabase
                (
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on the search result.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessWholeDatabase
                (
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on the search result.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                string database,
                string searchExpression
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessSearchResult
                (
                    searchExpression,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given record interval.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                string database,
                int fromMfn,
                int toMfn
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given record interval.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                int fromMfn,
                int toMfn
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given recordset.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                string database,
                IEnumerable<int> recordset
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given recordset.
        /// </summary>
        public GblResult Execute
            (
                ISyncIrbisProvider connection,
                IEnumerable<int> recordset
            )
        {
            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
        }

        /// <summary>
        /// Command "FI" - closes "IF".
        /// </summary>
        public GblBuilder Fi()
        {
            return AddStatement
                (
                    GblCode.Fi,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "IF".
        /// </summary>
        public GblBuilder If
            (
                string condition
            )
        {
            return AddStatement
                (
                    GblCode.If,
                    VerifyValue(condition),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "IF".
        /// </summary>
        public GblBuilder If
            (
                string condition,
                params GblStatement[] statements
            )
        {
            If(condition);
            _statements.AddRange(statements);

            return Fi();
        }

        /// <summary>
        /// Command "IF"
        /// </summary>
        public GblBuilder If
            (
                string condition,
                GblBuilder builder
            )
        {
            return If
                (
                    condition,
                    builder.ToStatements()
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        public GblBuilder Nop ()
        {
            return AddStatement
                (
                    GblCode.Comment,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        public GblBuilder Nop
            (
                string comment
            )
        {
            return AddStatement
                (
                    GblCode.Comment,
                    VerifyValue(comment),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        public GblBuilder Nop
            (
                string comment1,
                string comment2
            )
        {
            return AddStatement
                (
                    GblCode.Comment,
                    VerifyValue(comment1),
                    VerifyValue(comment2),
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "REP".
        /// </summary>
        public GblBuilder Replace
            (
                string field,
                string repeat,
                string toValue
            )
            =>
            AddStatement
            (
                GblCode.Replace,
                VerifyField(field),
                VerifyRepeat(repeat),
                VerifyValue(toValue),
                Filler
            );

        /// <summary>
        /// Command "REP".
        /// </summary>
        public GblBuilder Replace
            (
                string field,
                string toValue
            )
            =>
            AddStatement
            (
                GblCode.Replace,
                VerifyField(field),
                All,
                VerifyValue(toValue),
                Filler
            );

        /// <summary>
        /// Build statement array.
        /// </summary>
        public GblStatement[] ToStatements()
        {
            return _statements.ToArray();
        }

        /// <summary>
        /// Command "UNDO"
        /// </summary>
        public GblBuilder Undo
            (
                string version
            )
            =>
            AddStatement
            (
                GblCode.Undo,
                VerifyParameter(version),
                Filler,
                Filler,
                Filler
            );

        /// <summary>
        /// Verify command code.
        /// </summary>
        public string VerifyCode
            (
                string code
            )
        {
            // TODO some verification?

            return code;
        }

        /// <summary>
        /// Verify field specification.
        /// </summary>
        public string VerifyField
            (
                string field
            )
        {
            // TODO some verification?

            return field;
        }

        /// <summary>
        /// Verify format specification.
        /// </summary>
        public string VerifyFormat
            (
                string format
            )
        {
            // TODO some verification?

            return format;
        }

        /// <summary>
        /// Verify command parameter.
        /// </summary>
        public string VerifyParameter
            (
                string parameter
            )
        {
            // TODO some verification?

            return parameter;
        }

        /// <summary>
        /// Verify field repeat specification.
        /// </summary>
        public string VerifyRepeat
            (
                string repeat
            )
        {
            // TODO some verification?

            return repeat;
        }

        /// <summary>
        /// Verify value for command.
        /// </summary>
        public string VerifyValue
            (
                string value
            )
        {
            // TODO some verification?

            return value;
        }

        #endregion
    }
}
