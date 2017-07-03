using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SaveOn.ViewModels
{
    public class TextOnlyViewModel : ObservableObject
    {
        public ICommand PanningCommand { get; protected set; }
        public ICommand PannedCommand { get; protected set; }
        public ICommand SwipedCommand { get; protected set; }
    }
}
