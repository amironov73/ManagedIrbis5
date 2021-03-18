// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* GroupBand.cs -- полоса отчета с группировкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета с группировкой.
    /// </summary>
    public class GroupBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Выражение, согласно которому осуществляется группировка.
        /// </summary>
        [XmlAttribute("group")]
        [JsonPropertyName("group")]
        public string? GroupExpression { get; set; }

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="CompositeBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            var expression = GroupExpression;
            if (string.IsNullOrEmpty(expression))
            {
                RenderOnce(context);
            }
            else
            {
                /*

                int count = context.Records.Count;

                using (PftFormatter formatter
                    = context.GetFormatter(expression))
                {
                    List<Pair<string, int>> list
                        = new List<Pair<string, int>>(count);
                    for (int i = 0; i < count; i++)
                    {
                        string formatted = formatter.FormatRecord
                        (
                            context.Records[i]
                        );
                        Pair<string, int> pair = new Pair<string, int>
                        (
                            formatted,
                            i
                        );
                        list.Add(pair);
                    }

                    var grouped = list.GroupBy
                        (
                            item => item.First
                        )
                        .OrderBy
                        (
                            group => new NumberText(group.Key)
                        );

                    foreach (var group in grouped)
                    {
                        string key = group.Key;

                        List<Record> records = group.Select
                            (
                                item => context.Records[item.Second]
                            )
                            .ToList();

                        ReportContext cloneContext = context.Clone
                        (
                            records
                        );

                        cloneContext.Variables.SetVariable
                        (
                            "group",
                            key
                        );

                        RenderOnce(cloneContext);
                    }

                    context.Variables.Registry.Remove("group");

                }

                */
            }
        } // method Render

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<ReportBand>(this, throwOnError);

            verifier.Assert(base.Verify(throwOnError));

            verifier.NotNullNorEmpty
                (
                    GroupExpression,
                    nameof(GroupExpression)
                );

            return verifier.Result;
        } // method Verify

        #endregion

    } // class GroupBand

} // namespace ManagedIrbis.Reports
