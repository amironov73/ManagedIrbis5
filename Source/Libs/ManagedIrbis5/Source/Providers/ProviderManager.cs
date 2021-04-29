// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ProviderManager.cs -- менеджер провайдеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using AM;
using AM.Configuration;
using AM.Parameters;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.InMemory;

using Microsoft.Extensions.DependencyInjection;

#endregion

#nullable enable

namespace ManagedIrbis.Providers
{
    /// <summary>
    /// Менеджер провайдеров
    /// </summary>
    public static class ProviderManager
    {
        #region Constants

        /// <summary>
        /// Connected client (<see cref="SyncConnection"/>).
        /// </summary>
        public const string Connected = "Connected";

        /// <summary>
        /// Default provider (<see cref="SyncConnection"/>).
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// Local provider.
        /// </summary>
        public const string Direct = "Direct";

        /// <summary>
        /// Null provider.
        /// </summary>
        public const string Null = "Null";

        /// <summary>
        /// Провайдер "всё в памяти".
        /// </summary>
        public const string InMemory = "InMemory";

        // /// <summary>
        // /// Connected client with some local functionality:
        // /// <see cref="SemiConnectedClient"/>.
        // /// </summary>
        // public const string SemiConnected = "SemiConnected";

        #endregion

        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        public static Dictionary<string, Type> Registry
        {
            get; private set;
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        #endregion

        #region Construction

        static ProviderManager()
        {
            var serviceCollection = new ServiceCollection();

            Registry = new Dictionary<string, Type>
            {
                { Null, typeof(NullProvider) },
                { Direct, typeof(DirectProvider) },
                { InMemory, typeof(InMemoryProvider) },
                { Connected, typeof(SyncConnection) },
                //{SemiConnected, typeof(SemiConnectedClient)},
                //{Default, typeof(ConnectedClient)}
            };


            serviceCollection.RegisterIrbisProviders();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание првайдера прямого доступа.
        /// </summary>
        public static DirectProvider CreateDirectProvider
            (
                this IServiceProvider services
            )
        {
            var rootPath = (string) Magna.GlobalOptions["Irbis64Root"].ThrowIfNull();
            var result = new DirectProvider(rootPath);

            return result;
        }

        /// <summary>
        /// Создание синхронного подключения.
        /// </summary>
        public static ISyncProvider CreateSyncConnection
            (
                this IServiceProvider services
            )
        {
            var socket = services.GetRequiredService<ISyncClientSocket>();
            return new SyncConnection(socket, services);
        }

        /// <summary>
        /// Создание асинхронного подключения.
        /// </summary>
        public static IAsyncProvider CreateAsyncConnection
            (
                this IServiceProvider services
            )
        {
            var socket = services.GetRequiredService<IAsyncClientSocket>();
            return new AsyncConnection(socket, services);
        }

        /// <summary>
        /// Регистрация стандартных провайдеров ИРБИС.
        /// </summary>
        public static void RegisterIrbisProviders
            (
                this IServiceCollection services
            )
        {
            services.AddTransient<ISyncClientSocket, SyncTcp4Socket>();
            services.AddTransient<IAsyncClientSocket, AsyncTcp4Socket>();

            services.AddTransient<NullProvider>();
            services.AddTransient(CreateDirectProvider);
            services.AddTransient<InMemoryProvider>();
            services.AddTransient<ISyncProvider, SyncConnection>();
            services.AddTransient<ISyncConnection, SyncConnection>();
            services.AddTransient<IAsyncProvider, AsyncConnection>();
            services.AddTransient<IAsyncConnection, AsyncConnection>();
        }

        /// <summary>
        /// Get <see cref="ISyncProvider" /> and configure it.
        /// </summary>
        public static ISyncProvider GetAndConfigureProvider
            (
                string configurationString
            )
        {
            var parameters = ParameterUtility.ParseString (configurationString);
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

            var assemblyParameter= parameters.GetParameter("Assembly", null)
                ?? parameters.GetParameter("Assemblies", null);
            if (!string.IsNullOrEmpty(assemblyParameter))
            {
                var assemblies = assemblyParameter.Split('|');
                foreach (var assembly in assemblies)
                {
                    Assembly.Load(assembly);
                }
            }

            var typeName = parameters.GetParameter("Register", null)
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
        /// Get <see cref="ISyncProvider"/> by name.
        /// </summary>
        public static ISyncProvider? GetProvider
            (
                string name,
                bool throwOnError
            )
        {
            if (!Registry.TryGetValue(name, out var providerType))
            {
                Magna.Error
                    (
                        nameof(ProviderManager) + "::" + nameof(GetProvider)
                        + ": provider not found: "
                        + name
                    );

                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Provider not found: " + name
                        );
                }

                return default;
            }

            // var result = (ISyncProvider?)Activator.CreateInstance(type);
            var result = (ISyncProvider?) ServiceProvider.GetService(providerType);

            return result;

        } // method GetProvider

        /// <summary>
        /// Получение преконфигурированного провайдера.
        /// </summary>
        public static ISyncProvider GetPreconfiguredProvider()
        {
            var configurationString = ConfigurationUtility.GetString ("IrbisProvider");
            if (string.IsNullOrEmpty(configurationString))
            {
                Magna.Error
                    (
                        nameof(ProviderManager) + "::" + nameof(GetPreconfiguredProvider)
                        + ": 'IrbisProvider' configuration key not specified"
                    );

                throw new IrbisException
                    (
                        "'IrbisProvider' configuration key not specified"
                    );
            }

            var result = GetAndConfigureProvider(configurationString);

            return result;

        } // method GetPreconfiguredProvider

        #endregion

    } // class ProviderManager

} // namespace ManagedIrbis.Providers
