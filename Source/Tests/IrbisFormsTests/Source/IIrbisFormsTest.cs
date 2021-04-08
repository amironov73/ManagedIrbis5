using System.Windows.Forms;

#nullable enable

namespace IrbisFormsTests
{
    public interface IIrbisFormsTest
    {
        /// <summary>
        /// Запуск теста.
        /// </summary>
        public void RunTest(IWin32Window? ownerWindow);
    }
}
