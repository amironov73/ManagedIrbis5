// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Resources;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking
{
    internal static class ResourceHelper
    {
        private static ResourceManager _resourceManager = null;

        private static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager = new ResourceManager("WeifenLuo.WinFormsUI.Docking.Strings", typeof(ResourceHelper).Assembly);
                return _resourceManager;
            }

        }

        public static string GetString(string name)
        {
            return ResourceManager.GetString(name);
        }
    }
}
