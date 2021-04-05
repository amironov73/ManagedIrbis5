using System.Windows.Forms;

#nullable enable

namespace FormsTests
{
    public interface IFormsTest
    {
        /// <summary>
        /// Запуск теста.
        /// </summary>
        public void RunTest(IWin32Window? ownerWindow);
    }
}
