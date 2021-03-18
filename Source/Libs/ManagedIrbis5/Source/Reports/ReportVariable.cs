// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global

/* ReportVariable.cs -- переменная, содержащее некое значение, используемое в отчете
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Переменная, содержащая некое значение, используемое в отчете.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}: {" + nameof(Value) + "}")]
    public sealed class ReportVariable
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя переменное (вообще говоря, произвольное).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение переменной (еще более произвольное).
        /// </summary>
        public object? Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportVariable
            (
                string name,
                object? value
            )
        {
            Name = name;
            Value = value;
        } // constructor

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReportVariable>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, nameof(Name));

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{Name.ToVisibleString()}: {Value.ToVisibleString()}";

        #endregion

    } // class ReportVariable

} // namespace ManagedIrbis.Reports
