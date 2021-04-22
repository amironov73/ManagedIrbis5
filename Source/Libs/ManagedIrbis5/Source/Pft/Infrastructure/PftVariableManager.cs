// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftVariableManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftVariableManager
    {
        #region Properties

        /// <summary>
        /// Parent variable manager.
        /// </summary>
        public PftVariableManager? Parent { get; private set; }

        /// <summary>
        /// Registry.
        /// </summary>
        public CaseInsensitiveDictionary<PftVariable> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableManager
            (
                PftVariableManager? parent
            )
        {
            Parent = parent;

            Registry = new CaseInsensitiveDictionary<PftVariable>();
        }

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
            for (
                    var manager = this;
                    !ReferenceEquals(manager, null);
                    manager = manager.Parent
                )
            {
                var keys = manager.Registry.Keys.OrderBy(key=>key);
                foreach (var key in keys)
                {
                    var variable = manager.Registry[key];
                    writer.WriteLine(variable.ToString());
                }
                writer.WriteLine(new string('=', 60));
            }
        }

        /// <summary>
        /// Get all variables.
        /// </summary>
        public PftVariable[] GetAllVariables()
        {
            var result = new List<PftVariable>();

            for (
                    var manager = this;
                    !ReferenceEquals(manager, null);
                    manager = manager.Parent
                )
            {
                var keys = manager.Registry.Keys.OrderBy(key => key);
                foreach (var key in keys)
                {
                    var variable = manager.Registry[key];
                    result.Add(variable);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get existing variable with the specified name.
        /// </summary>
        public PftVariable? GetExistingVariable
            (
                string name
            )
        {
            PftVariable? result = null;
            for (
                    var manager = this;
                    !ReferenceEquals(manager, null);
                    manager = manager.Parent
                )
            {
                if (manager.Registry.TryGetValue(name, out result))
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get existing or create new variable with given name.
        /// </summary>
        public PftVariable GetOrCreateVariable
            (
                string name,
                bool isNumeric
            )
        {
            var result = GetExistingVariable(name);
            if (ReferenceEquals(result, null))
            {
                result = new PftVariable(name, isNumeric);
                Registry.Add(name, result);
            }
            else
            {
                if (result.IsNumeric != isNumeric)
                {
                    throw new IrbisException();
                }
            }

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        public PftVariable SetVariable
            (
                string name,
                string? value
            )
        {
            var result = GetOrCreateVariable(name, false);
            result.IsNumeric = false;
            result.StringValue = value;

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        public PftVariable SetVariable
            (
                PftContext context,
                string name,
                IndexSpecification index,
                string? value
            )
        {
            var result = GetOrCreateVariable(name, false);
            result.IsNumeric = false;

            if (index.Kind == IndexKind.None)
            {
                result.StringValue = value;
            }
            else
            {
                var text = result.StringValue ?? string.Empty;
                string?[] lines = text.SplitLines();
                lines = PftUtility.SetArrayItem
                    (
                        context,
                        lines,
                        index,
                        value
                    );

                result.StringValue = String.Join
                    (
                        Environment.NewLine,
                        lines
                    );
            }

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        public PftVariable SetVariable
            (
                string name,
                double value
            )
        {
            var result = GetOrCreateVariable(name, true);
            result.IsNumeric = true;
            result.NumericValue = value;

            return result;
        }

        #endregion
    }
}
