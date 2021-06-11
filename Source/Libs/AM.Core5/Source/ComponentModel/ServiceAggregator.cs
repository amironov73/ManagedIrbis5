// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ServiceAggregator.cs -- позволяет объединить несколько поставщиков сервисов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.ComponentModel
{
    /// <summary>
    /// Позволяет объединить несколько поставщиков сервисов.
    /// </summary>
    public sealed class ServiceAggregator
        : IServiceProvider
    {
        #region Nested classes

        /// <summary>
        /// Простейший дескриптор сервиса.
        /// </summary>
        record ServiceDescriptor(Type ServiceType, object Instance);

        #endregion

        #region Private members

        private readonly List<IServiceProvider> _providers = new();

        private readonly List<ServiceDescriptor> _descriptors = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление провайдера сервисов в конец списка.
        /// </summary>
        public ServiceAggregator AddServiceProvider
            (
                IServiceProvider provider,
                bool atBeginning = true
            )
        {
            if (atBeginning)
            {
                _providers.Insert(0, provider);
            }
            else
            {
                _providers.Add(provider);
            }

            return this;

        } // method AddServiceProvider

        /// <summary>
        /// Добавление сервиса.
        /// </summary>
        public ServiceAggregator AddService
            (
                Type serviceType,
                object serviceInstance,
                bool atBeginning = true
            )
        {
            var descriptor = new ServiceDescriptor(serviceType, serviceInstance);
            if (atBeginning)
            {
                _descriptors.Insert(0, descriptor);
            }
            else
            {
                _descriptors.Add(descriptor);
            }

            return this;

        } // method AddService

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService
            (
                Type serviceType
            )
        {
            foreach (var descriptor in _descriptors)
            {
                if (descriptor.ServiceType == serviceType)
                {
                    return descriptor.Instance;
                }
            }

            foreach (var provider in _providers)
            {
                var result = provider.GetService(serviceType);
                if (result is not null)
                {
                    return result;
                }
            }

            return null;

        } // method GetService

        #endregion

    } // class ServiceAggregator

} // namespace AM.ComponentModel
