// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RemoteFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    public sealed class RemoteFormatter
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Counter.
        /// </summary>
        public static int Counter { get; private set; }

        /// <summary>
        /// Domain
        /// </summary>
        public AppDomain Domain { get; private set; }

        /// <summary>
        /// Assembly.
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Formatter type.
        /// </summary>
        public Type FormatterType { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RemoteFormatter
            (
                string assemblyFile
            )
        {
            string friendlyName = "RemoteFormatter"
                + (++Counter).ToInvariantString();

            string directory = Path.GetDirectoryName(assemblyFile);
            //AppDomainSetup setup = new AppDomainSetup
            //{
            //    ApplicationBase = directory,
            //};
            //Evidence evidence = AppDomain.CurrentDomain.Evidence;

            //Domain = AppDomain.CreateDomain(friendlyName, evidence, setup);
            Domain = AppDomain.CreateDomain(friendlyName);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Domain.Load(assembly.GetName());
            }

            Type proxyType = typeof(CompilerProxy);
            CompilerProxy proxy = (CompilerProxy) Domain.CreateInstanceAndUnwrap
                (
                    proxyType.Assembly.FullName,
                    proxyType.FullName
                );
            Assembly = proxy.LoadAssembly(assemblyFile);

            Type[] types = Assembly.GetTypes();
            if (types.Length != 1)
            {
                throw new PftCompilerException();
            }
            FormatterType  = types[0];
            if (!FormatterType.IsSubclassOf(typeof(PftPacket)))
            {
                throw new PftCompilerException();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get formatter instance.
        /// </summary>
        public PftPacket GetFormatter
            (
                PftContext context
            )
        {
            /*

// Obsolete method call
#pragma warning disable 618

            PftPacket result = (PftPacket) Domain.CreateInstanceAndUnwrap
                (
                    Assembly.FullName, // assemblyName
                    FormatterType.FullName, // typeName
                    false, // ignoreCase
                    BindingFlags.Default, // bindingAttr
                    null, // binder
                    new object[] {context}, // args
                    null, // culture
                    null, // activationAttributes
                    null // securityAttributes
                );

#pragma warning restore 618

            if (!RemotingServices.IsTransparentProxy(result))
            {
                throw new PftCompilerException();
            }

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            AppDomain.Unload(Domain);
        }

        #endregion

        #region Object members

        #endregion
    }
}
