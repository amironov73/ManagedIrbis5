using System.Collections.Generic;
using AM.Avalonia.DTO;
using AM.Avalonia.Models;
using AM.Avalonia.Views;

namespace AM.Avalonia.ViewModels
{
    public class MsBoxCustomViewModel : AbstractMsBoxViewModel
    {
        private readonly MsBoxCustomWindow _window;


        public MsBoxCustomViewModel(MsCustomParams parameters, MsBoxCustomWindow msBoxCustomWindow) : base(parameters,
            parameters.Icon, parameters.BitmapIcon)
        {
            _window = msBoxCustomWindow;
            ButtonDefinitions = parameters.ButtonDefinitions;
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
