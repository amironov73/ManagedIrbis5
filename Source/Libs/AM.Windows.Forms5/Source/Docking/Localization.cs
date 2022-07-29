// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

[AttributeUsage (AttributeTargets.All)]
internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
{
    private bool m_initialized = false;

    public LocalizedDescriptionAttribute (string key) : base (key)
    {
    }

    public override string Description
    {
        get
        {
            if (!m_initialized)
            {
                string key = base.Description;
                DescriptionValue = ResourceHelper.GetString (key);
                if (DescriptionValue == null)
                {
                    DescriptionValue = String.Empty;
                }

                m_initialized = true;
            }

            return DescriptionValue;
        }
    }
}

[AttributeUsage (AttributeTargets.All)]
internal sealed class LocalizedCategoryAttribute : CategoryAttribute
{
    public LocalizedCategoryAttribute (string key) : base (key)
    {
    }

    protected override string GetLocalizedString (string value)
    {
        return ResourceHelper.GetString (value);
    }
}