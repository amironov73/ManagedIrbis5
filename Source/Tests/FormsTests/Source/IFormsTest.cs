﻿using System.Windows.Forms;

#nullable enable

namespace FormsTests
{
    public interface IFormsTest
    {
        /// <summary>
        /// Запуск теста.
        /// </summary>
        void RunTest(IWin32Window? ownerWindow);
    }
}
