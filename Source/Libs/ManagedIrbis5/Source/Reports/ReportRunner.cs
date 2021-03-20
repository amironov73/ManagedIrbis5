// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ReportRunner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;
using AM;

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ReportRunner
    {
        #region Public methods

        /// <summary>
        /// Run the report.
        /// </summary>
        public void RunReport
            (
                IrbisReport report,
                ReportSettings settings
            )
        {
            report.Verify(true);
            settings.Verify(true);

            var assemblies = settings.Assemblies.ToArray();
            foreach (string path in assemblies)
            {
                Assembly.LoadFile(path);
            }
            string providerFullName = settings.RegisterProvider;
            if (!string.IsNullOrEmpty(providerFullName))
            {
                var providerType = Type.GetType
                    (
                        providerFullName,
                        true
                    )
                    .ThrowIfNull("providerType");
                string key = providerType.Name;
                ProviderManager.Registry[key] = providerType;
            }

            string driverFullName = settings.DriverName;
            if (!string.IsNullOrEmpty(driverFullName))
            {
                var driverType = Type.GetType
                    (
                        driverFullName,
                        true
                    )
                    .ThrowIfNull("driverType");
                string key = driverType.Name;
                DriverManager.Registry[key] = driverType;
            }

            string providerName = settings.ProviderName
                .ThrowIfNull("providerName not specified");
            var provider = ProviderManager.GetProvider
                (
                    providerName,
                    true
                )
                .ThrowIfNull("can't get provider");

            string driverName = settings.DriverName
                .ThrowIfNull("driverName not specified");
            ReportDriver driver = DriverManager.GetDriver
                (
                    driverName,
                    true
                )
                .ThrowIfNull("can't get driver");

            ReportContext context = new ReportContext(provider);

            string filterExpression = settings.Filter;
            if (!string.IsNullOrEmpty(filterExpression))
            {
                provider.Search(filterExpression);
                // TODO set records to context
            }

            report.Render(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
