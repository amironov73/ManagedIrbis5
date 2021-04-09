using System.Windows.Forms;

#nullable enable

namespace SiberianTests
{
    public interface ISiberianTest
    {
        /// <summary>
        /// Запуск теста.
        /// </summary>
        void RunTest(IWin32Window? ownerWindow);

    }
}
