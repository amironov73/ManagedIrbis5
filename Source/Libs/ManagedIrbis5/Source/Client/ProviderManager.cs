// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ProviderManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using AM;
using AM.Configuration;
using AM.Parameters;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    ///
    /// </summary>
    public static class ProviderManager
    {
        #region Constants

        /// <summary>
        /// Connected client (<see cref="Connection"/>).
        /// </summary>
        public const string Connected = "Connected";

        /// <summary>
        /// Default provider (<see cref="Connection"/>).
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// Local provider.
        /// </summary>
        public const string Local = "Local";

        /// <summary>
        /// Null provider.
        /// </summary>
        public const string Null = "Null";

        /// <summary>
        /// Connected client with some local functionality:
        /// <see cref="SemiConnectedClient"/>.
        /// </summary>
        public const string SemiConnected = "SemiConnected";

        #endregion

        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        public static Dictionary<string, Type> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        static ProviderManager()
        {
            Registry = new Dictionary<string, Type>
            {
                {Null, typeof(NullProvider)},
                {Local, typeof(LocalProvider)},
                {Connected, typeof(ConnectedClient)},
                {SemiConnected, typeof(SemiConnectedClient)},
                {Default, typeof(ConnectedClient)}
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="IrbisProvider" /> and configure it.
        /// </summary>
        public static IrbisProvider GetAndConfigureProvider
            (
                string configurationString
            )
        {
            var parameters = ParameterUtility.ParseString
                (
                    configurationString
                );
            var name = parameters.GetParameter("Provider", null);
            if (string.IsNullOrEmpty(name))
            {
                Magna.Warning
                    (
                        "ProviderManager::GetAndConfigureProvider: "
                        + "provider name not specified"
                    );

                name = Default;
            }

            var assemblyParameter
                = parameters.GetParameter("Assembly", null)
                ?? parameters.GetParameter("Assemblies", null);
            if (!string.IsNullOrEmpty(assemblyParameter))
            {
                string[] assemblies = assemblyParameter.Split('|');
                foreach (string assembly in assemblies)
                {
                    Assembly.Load(assembly);
                }
            }

            var typeName
                = parameters.GetParameter("Register", null)
                  ?? parameters.GetParameter("Type", null);
            if (!string.IsNullOrEmpty(typeName))
            {
                var type = Type.GetType(typeName, true).ThrowIfNull();
                var shortName = type.Name;
                if (!Registry.ContainsKey(shortName))
                {
                    Registry.Add(shortName, type);
                }
            }

            var result = GetProvider(name, true)
                .ThrowIfNull();
            result.Configure(configurationString);

            return result;
        }

        /// <summary>
        /// Get <see cref="IrbisProvider"/> by name.
        /// </summary>
        public static IrbisProvider? GetProvider
            (
                [NotNull] string name,
                bool throwOnError
            )
        {
            if (!Registry.TryGetValue(name, out var type))
            {
                Magna.Error
                    (
                        "ProviderManager::GetProvider: "
                        + "provider not found: "
                        + name
                    );

                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Provider not found: " + name
                        );
                }

                return null;
            }

            if (ReferenceEquals(type, null))
            {
                Magna.Error
                    (
                        "ProviderManager::GetProvider: "
                        + "can't find type: "
                        + name
                    );

                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Can't find type: " + name
                        );
                }

                return null;
            }

            var result = (IrbisProvider?)Activator.CreateInstance(type);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public static IrbisProvider GetPreconfiguredProvider()
        {
            var configurationString
                = ConfigurationUtility.GetString
                (
                    "IrbisProvider"
                );
            if (string.IsNullOrEmpty(configurationString))
            {
                Magna.Error
                    (
                        "ProviderManager::GetPreconfiguredProvider: "
                        + "IrbisProvider configuration key not specified"
                    );

                throw new IrbisException
                    (
                        "IrbisProvider configuration key not specified"
                    );
            }

            IrbisProvider result
                = GetAndConfigureProvider(configurationString);

            return result;

        }

        #endregion
    }
}
