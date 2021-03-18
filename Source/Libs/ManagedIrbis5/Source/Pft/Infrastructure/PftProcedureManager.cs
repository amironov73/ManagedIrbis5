﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProcedureManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;
using AM.Collections;
using AM.IO;




#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure manager.
    /// </summary>

    public sealed class PftProcedureManager
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        public CaseInsensitiveDictionary<PftProcedure> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureManager()
        {
            Registry = new CaseInsensitiveDictionary<PftProcedure>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize the registry.
        /// </summary>
        public void Deserialize
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                PftProcedure procedure = new PftProcedure();
                procedure.Deserialize(reader);
                string name = procedure.Name
                    .ThrowIfNull("procedure.Name");
                Registry.Add(name, procedure);
            }
        }

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                PftContext context,
                string name,
                [CanBeNull] string argument
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(name, "name");

            PftProcedure procedure = FindProcedure(name);
            if (!ReferenceEquals(procedure, null))
            {
                procedure.Execute
                    (
                        context,
                        argument
                    );
            }
        }

        /// <summary>
        /// Find specified procedure.
        /// </summary>
        [CanBeNull]
        public PftProcedure FindProcedure
            (
                string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftProcedure result;
            Registry.TryGetValue(name, out result);

            return result;
        }

        /// <summary>
        /// Have procedure with given name?
        /// </summary>
        public bool HaveProcedure
            (
                string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftProcedure result;
            Registry.TryGetValue(name, out result);

            return !ReferenceEquals(result, null);
        }

        /// <summary>
        /// Serialize the registry.
        /// </summary>
        public void Serialize
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(Registry.Count);
            foreach (PftProcedure procedure in Registry.Values)
            {
                procedure.Serialize(writer);
            }
        }

        #endregion
    }
}
