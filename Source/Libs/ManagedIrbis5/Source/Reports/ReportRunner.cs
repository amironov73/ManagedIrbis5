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
            foreach (var path in assemblies)
            {
                Assembly.LoadFile(path);
            }

            var providerFullName = settings.RegisterProvider;
            if (!string.IsNullOrEmpty(providerFullName))
            {
                var providerType = Type.GetType
                    (
                        providerFullName,
                        true
                    )
                    .ThrowIfNull("providerType");
                var key = providerType.Name;
                ProviderManager.Registry[key] = providerType;
            }

            var driverFullName = settings.DriverName;
            if (!string.IsNullOrEmpty(driverFullName))
            {
                var driverType = Type.GetType
                    (
                        driverFullName,
                        true
                    )
                    .ThrowIfNull("driverType");
                var key = driverType.Name;
                DriverManager.Registry[key] = driverType;
            }

            var providerName = settings.ProviderName.ThrowIfNull("providerName not specified");
            var provider = ProviderManager.GetProvider
                (
                    providerName,
                    true
                )
                .ThrowIfNull("can't get provider");

            var driverName = settings.DriverName.ThrowIfNull("driverName not specified");
            var driver = DriverManager.GetDriver
                (
                    driverName,
                    true
                )
                .ThrowIfNull("can't get driver");

            var context = new ReportContext(provider)
            {
                Driver = driver
            };

            var filterExpression = settings.Filter;
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
