// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AssemblyMetadata.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

namespace AM.Updating.Models;

/// <summary>
/// Contains information about an assembly.
/// </summary>
public partial class AssemblyMetadata
{
    /// <summary>
    /// Assembly name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Assembly version.
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Assembly file path.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="AssemblyMetadata"/>.
    /// </summary>
    public AssemblyMetadata (string name, Version version, string filePath)
    {
        Name = name;
        Version = version;
        FilePath = filePath;
    }
}

public partial class AssemblyMetadata
{
    /// <summary>
    /// Extracts assembly metadata from given assembly.
    /// The specified path is used to override the executable file path in case the assembly is not meant to run directly.
    /// </summary>
    public static AssemblyMetadata FromAssembly (Assembly assembly, string assemblyFilePath)
    {
        var name = assembly.GetName().Name!;
        var version = assembly.GetName().Version!;
        var filePath = assemblyFilePath;

        return new AssemblyMetadata (name, version, filePath);
    }

    /// <summary>
    /// Extracts assembly metadata from given assembly.
    /// </summary>
    public static AssemblyMetadata FromAssembly (Assembly assembly) => FromAssembly (assembly, assembly.Location);

    /// <summary>
    /// Extracts assembly metadata from entry assembly.
    /// </summary>
    public static AssemblyMetadata FromEntryAssembly()
    {
        var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException ("Can't get entry assembly.");
        return FromAssembly (assembly);
    }
}
