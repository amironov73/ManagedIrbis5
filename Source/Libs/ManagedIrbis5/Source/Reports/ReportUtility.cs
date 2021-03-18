// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Local

/* ReportUtility.cs -- утилиты для работы с отчетами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

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
            // TODO сделать получение форматтера через DI
            IPftFormatter result = new PftFormatter();
            result.SetProvider(context.Provider);
            if (!string.IsNullOrEmpty(expression))
            {
                result.ParseProgram(expression);
            }

            return result;
        } // method GetFormatter

        /// <summary>
        /// Перечисление известных типов полос.
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
        } // method SetHeight

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
        } // method SetWidth

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
        } // method SetVariables

        #endregion

    } // class ReportUtility

} // namespace ManagedIrbis.Reports
