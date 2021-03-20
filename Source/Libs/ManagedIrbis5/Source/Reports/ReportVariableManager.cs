// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ReportVariableManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ReportVariableManager
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Variable registry.
        /// </summary>
        public CaseInsensitiveDictionary<ReportVariable> Registry { get; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Dump all the variables.
        /// </summary>
        public void DumpVariables
            (
                TextWriter writer
            )
        {
            var keys = Registry.Keys.OrderBy(key => key);
            foreach (string key in keys)
            {
                ReportVariable variable = Registry[key];
                writer.WriteLine(variable.ToString());
            }
            writer.WriteLine(new string('=', 60));
        }

        /// <summary>
        /// Get all variables.
        /// </summary>
        public ReportVariable[] GetAllVariables()
        {
            List<ReportVariable> result = new List<ReportVariable>();

            var keys = Registry.Keys.OrderBy(key => key);
            foreach (string key in keys)
            {
                ReportVariable variable = Registry[key];
                result.Add(variable);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get existing variable with the specified name.
        /// </summary>
        public ReportVariable? GetExistingVariable
            (
                string name
            )
        {
            Registry.TryGetValue(name, out var result);

            return result;
        }

        /// <summary>
        /// Get existing or create new variable with given name.
        /// </summary>
        public ReportVariable GetOrCreateVariable
            (
                string name
            )
        {
            var result = GetExistingVariable(name);
            if (ReferenceEquals(result, null))
            {
                result = new ReportVariable(name, null);
                Registry.Add(name, result);
            }

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        public ReportVariable SetVariable
            (
                string name,
                object? value
            )
        {
            var result = GetOrCreateVariable(name);
            result.Value = value;

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportVariableManager> verifier
                = new Verifier<ReportVariableManager>(this, throwOnError);

            foreach (ReportVariable variable in GetAllVariables())
            {
                verifier.VerifySubObject(variable, "variable");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
