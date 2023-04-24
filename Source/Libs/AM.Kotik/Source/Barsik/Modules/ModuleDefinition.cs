// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ModuleDefinition.cs -- определение модуля интерпретатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Определение модуля для интерпретатора.
/// </summary>
[PublicAPI]
public sealed class ModuleDefinition
{
    #region Properties

    /// <summary>
    /// Сборки, которые необходимо загрузить перед
    /// инициализацией модуля.
    /// </summary>
    [JsonPropertyName ("assemblies")]
    public List<string>? AssembliesToLoad { get; set; }

    /// <summary>
    /// Тип модуля.
    /// </summary>
    /// <remarks>
    /// Нужно короткое (без namespace и не Assembly-qualified)
    /// имя типа.
    /// </remarks>
    [JsonPropertyName ("module-type")]
    public string? ModuleType { get; set; }

    #endregion

    #region Private members

    private void CopyFile
        (
            string fileName,
            string sourceFolder,
            string targetFolder,
            bool mandatory
        )
    {
        var targetFileName = Path.Combine (targetFolder, fileName);
        if (File.Exists (targetFileName))
        {
            return;
        }

        var sourceFileName = Path.Combine (sourceFolder);
        if (!File.Exists (sourceFileName))
        {
            if (mandatory)
            {
                throw new FileNotFoundException (sourceFileName);
            }

            return;
        }

        File.Copy (sourceFileName, targetFileName);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Копирование сборок для деплоймента.
    /// </summary>
    public void Deploy
        (
            string sourceFolder,
            string destinationFolder
        )
    {
        Sure.DirectoryExists (sourceFolder);
        Sure.NotNullNorEmpty (destinationFolder);

        if (string.CompareOrdinal (sourceFolder, destinationFolder) == 0)
        {
            return;
        }

        Directory.CreateDirectory (destinationFolder);
        if (AssembliesToLoad is not null)
        {
            foreach (var assemblyName in AssembliesToLoad)
            {
                CopyFile (assemblyName + ".dll", sourceFolder, destinationFolder, true);
                CopyFile (assemblyName + ".pdb", sourceFolder, destinationFolder, false);
            }
        }
    }

    /// <summary>
    /// Загрузка определения модуля из указанного файла.
    /// </summary>
    public static ModuleDefinition Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        return JsonUtility.ReadObjectFromFile<ModuleDefinition> (fileName);
    }

    /// <summary>
    /// Сохранение определения модуля в указанный файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        JsonUtility.SerializeIndented (this);
    }

    #endregion
}
