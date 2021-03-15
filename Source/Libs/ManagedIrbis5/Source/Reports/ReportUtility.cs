// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReportUtility.cs -- утилиты для работы с отчетами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Утилиты для работы с отчетами.
    /// </summary>
    public static class ReportUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get PFT formatter for the report context.
        /// </summary>
        public static IPftFormatter GetFormatter
            (
                this ReportContext context,
                string? expression
            )
        {

            IPftFormatter result = new PftFormatter();
            result.SetProvider(context.Provider);
            if (!string.IsNullOrEmpty(expression))
            {
                result.ParseProgram(expression);
            }

            return result;
        }

        /// <summary>
        /// List band types.
        /// </summary>
        public static Type[] ListBandTypes()
        {
            Type[] result =
            {
                typeof(CompositeBand),
                typeof(ConditionalBand),
                typeof(FilterBand),
                typeof(GroupBand),
                typeof(SectionBand),
                typeof(SortBand),
                typeof(TableBand),
                typeof(TotalBand)
            };

            return result;
        }

        /// <summary>
        /// List cell types.
        /// </summary>
        public static Type[] ListCellTypes()
        {
            Type[] result =
            {
                typeof(IndexCell),
                typeof(PftCell),
                typeof(RawPftCell),
                typeof(RawTextCell),
                typeof(TextCell),
                typeof(TotalCell)
            };

            return result;
        }

        /// <summary>
        /// Set height of the object.
        /// </summary>
        public static IAttributable SetHeight
            (
                this IAttributable reportObject,
                int height
            )
        {
            reportObject.Attributes["Height"] = height;

            return reportObject;
        }

        /// <summary>
        /// Set width of the object.
        /// </summary>
        public static IAttributable SetWidth
            (
                this IAttributable reportObject,
                int width
            )
        {
            reportObject.Attributes["Width"] = width;

            return reportObject;
        }

        /// <summary>
        /// Set variables for <see cref="PftFormatter"/>.
        /// </summary>
        public static void SetVariables
            (
                this ReportContext context,
                IPftFormatter? formatter
            )
        {
            if (!ReferenceEquals(formatter, null))
            {
                /*

                foreach (ReportVariable variable in
                    context.Variables.GetAllVariables())
                {
                    formatter.Context.Variables.SetVariable
                        (
                            variable.Name,
                            variable.Value.NullableToString()
                        );
                }

                */
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
