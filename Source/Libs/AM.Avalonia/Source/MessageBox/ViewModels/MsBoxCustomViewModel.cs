using System.Collections.Generic;
using AM.Avalonia.DTO;
using AM.Avalonia.Models;
using AM.Avalonia.Views;

namespace AM.Avalonia.ViewModels
{
    public class MsBoxCustomViewModel : AbstractMsBoxViewModel
    {
        private readonly MsBoxCustomWindow _window;


        public MsBoxCustomViewModel(MsCustomParams @params, MsBoxCustomWindow msBoxCustomWindow) : base(@params,
            @params.Icon, @params.BitmapIcon)
        {
            _window = msBoxCustomWindow;
            ButtonDefinitions = @params.ButtonDefinitions;
        }

        public IEnumerable<ButtonDefinition> ButtonDefinitions { get; }

        public void ButtonClick(string parameter)
        {
            foreach (var bd in ButtonDefinitions)
            {
                if (!parameter.Equals(bd.Name)) continue;
                _window.ButtonResult = bd.Name;
                break;
            }

            _window.Close();
        }
    }
}
