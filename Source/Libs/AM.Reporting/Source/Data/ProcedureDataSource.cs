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

using System.Data;

#endregion

#nullable enable

namespace FastReport.Data
{
    /// <summary>
    /// Datasource for stored procedure.
    /// </summary>
    public partial class ProcedureDataSource : TableDataSource
    {
        /// <inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                if (value)
                {
                    if (Parameters != null && Report != null)
                    {
                        if (Connection != null)
                            Connection.FillTable(this);
                        foreach (CommandParameter parameter in Parameters)
                        {
                            if (parameter.Direction == ParameterDirection.Input)
                                continue;
                            Report.SetParameterValue(Name + "_" + parameter.Name, parameter.Value);
                            ReportDesignerSetModified();
                        }
                    }
                }
                else
                {
                    if (Parameters != null && Report != null)
                        foreach (CommandParameter parameter in Parameters)
                        {
                            if (parameter.Direction == ParameterDirection.Input)
                                continue;
                            Report.Parameters.Remove(Report.GetParameter(Name + "_" + parameter.Name));
                            ReportDesignerSetModified();
                        }
                }
            }
        }

        /// <inheritdoc/>
        public override string Name
        {
            get => base.Name;
            set
            {
                if (Enabled && Parameters != null && Report != null)
                    foreach (CommandParameter parameter in Parameters)
                    {
                        Parameter param = Report.GetParameter(Name + "_" + parameter.Name);
                        if (param != null)
                            param.Name = value + "_" + parameter.Name;
                    }
                base.Name = value;
            }
        }
    }
}