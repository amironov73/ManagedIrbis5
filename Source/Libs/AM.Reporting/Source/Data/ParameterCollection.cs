// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// Represents the collection of <see cref="Parameter"/> objects.
    /// </summary>
    public class ParameterCollection : FRCollectionBase
    {
        /// <summary>
        /// Gets or sets a parameter.
        /// </summary>
        /// <param name="index">The index of a parameter in this collection.</param>
        /// <returns>The parameter with specified index.</returns>
        public Parameter this [int index]
        {
            get => List[index] as Parameter;
            set => List[index] = value;
        }

        /// <summary>
        /// Finds a parameter by its name.
        /// </summary>
        /// <param name="name">The name of a parameter.</param>
        /// <returns>The <see cref="Parameter"/> object if found; otherwise <b>null</b>.</returns>
        public Parameter FindByName (string name)
        {
            foreach (Parameter c in this)
            {
                // check complete match or match without case sensitivity
                if (c.Name == name || c.Name.ToLower() == name.ToLower())
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an unique parameter name based on given name.
        /// </summary>
        /// <param name="name">The base name.</param>
        /// <returns>The unique name.</returns>
        public string CreateUniqueName (string name)
        {
            var baseName = name;
            var i = 1;
            while (FindByName (name) != null)
            {
                name = baseName + i.ToString();
                i++;
            }

            return name;
        }

        /// <summary>
        /// Copies the parameters from other collection.
        /// </summary>
        /// <param name="source">Parameters to copy from.</param>
        public void Assign (ParameterCollection source)
        {
            Clear();
            foreach (Parameter par in source)
            {
                var thisParam = new Parameter (par.Name);
                Add (thisParam);

                thisParam.DataType = par.DataType;
                thisParam.Value = par.Value;
                thisParam.Expression = par.Expression;
                thisParam.Description = par.Description;

                thisParam.Parameters.Assign (par.Parameters);
            }
        }

        private void EnumParameters (ParameterCollection root, SortedList<string, Parameter> list)
        {
            foreach (Parameter p in root)
            {
                if (!list.ContainsKey (p.FullName))
                {
                    list.Add (p.FullName, p);
                    EnumParameters (p.Parameters, list);
                }
            }
        }

        internal void AssignValues (ParameterCollection source)
        {
            SortedList<string, Parameter> this_list = new SortedList<string, Parameter>();
            EnumParameters (this, this_list);
            SortedList<string, Parameter> source_list = new SortedList<string, Parameter>();
            EnumParameters (source, source_list);
            foreach (KeyValuePair<string, Parameter> kv in source_list)
            {
                if (this_list.ContainsKey (kv.Key))
                {
                    this_list[kv.Key].Value = kv.Value.Value;
                }
            }
        }

        internal void MoveUp (Parameter par)
        {
            var order = IndexOf (par);
            if (order > 0)
            {
                RemoveAt (order);
                Insert (order - 1, par);
            }
        }

        internal void MoveDown (Parameter par)
        {
            var order = IndexOf (par);
            if (order != -1 && order < Count - 1)
            {
                RemoveAt (order);
                Insert (order + 1, par);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class with default settings.
        /// </summary>
        /// <param name="owner">The owner of this collection.</param>
        public ParameterCollection (Base owner) : base (owner)
        {
        }
    }
}
