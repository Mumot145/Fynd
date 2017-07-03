using MR.Gestures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SaveOn.ViewModels
{
    public class CustomEventArgsViewModel : TextOnlyViewModel
    {
        public CustomEventArgsViewModel()
        {
            PanningCommand = new Command<PanEventArgs>(OnPanning);
            PannedCommand = new Command<PanEventArgs>(OnPanned);
            SwipedCommand = new Command<SwipeEventArgs>(OnSwiped);
            // Debug.WriteLine("in CustomEventArgsViewModel");
            //Debug.WriteLine("PanningCommand" + PanningCommand);
            //Debug.WriteLine("PannedCommand" + PannedCommand);
        }
        protected virtual void OnPanning(PanEventArgs e)
        {
            // AddText(PanInfo("Panning", e));
            Debug.WriteLine("Panning");
        }

        protected virtual void OnPanned(PanEventArgs e)
        {
            // AddText(PanInfo("Panned", e));
            Debug.WriteLine("Panned");

        }
        protected virtual void OnSwiped(SwipeEventArgs e)
        {
            Debug.WriteLine("Swiped " + e.Direction);
            //AddText(PanInfo("Swiped " + e.Direction, e));

        }
    }
}
