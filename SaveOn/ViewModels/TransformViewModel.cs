using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SaveOn.ViewModels
{
    public class TransformViewModel : CustomEventArgsViewModel
    {
        #region Properties

        protected double anchorX = 0.5;
        public double AnchorX
        {
            get { return anchorX; }
            set { SetProperty(ref anchorX, value); }
        }

        protected double anchorY = 0.5;
        public double AnchorY
        {
            get { return anchorY; }
            set { SetProperty(ref anchorY, value); }
        }

        protected double rotation = 0;
        public double Rotation
        {
            get { return rotation; }
            set { SetProperty(ref rotation, value); }
        }

        protected double scale = 1;
        public double Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value); }
        }

        protected double translationX = 0;
        public double TranslationX
        {
            get { return translationX; }
            set { SetProperty(ref translationX, value); }
        }

        protected double translationY = 0;
        public double TranslationY
        {
            get { return translationY; }
            set { SetProperty(ref translationY, value); }
        }

        public double ViewX { get; set; }
        public double ViewY { get; set; }
        public double ViewWidth { get; set; }
        public double ViewHeight { get; set; }

        #endregion

        protected override void OnPanning(MR.Gestures.PanEventArgs e)
        {
            base.OnPanning(e);
            Debug.WriteLine("in OnPanning");
            //TranslationX += e.DeltaDistance.X;
            //TranslationY += e.DeltaDistance.Y;
            //if (e.Center.X > 400)
            //{
            //    Debug.WriteLine("right swipe");
            //}
            //if (e.Center.X < 100)
            //{
            //    Debug.WriteLine("left swipe");
            //}
            //Debug.WriteLine(e.Center);

        }

        protected override void OnSwiped(MR.Gestures.SwipeEventArgs e)
        {
            base.OnSwiped(e);
            Debug.WriteLine(e.Direction);
            if (e.Direction == MR.Gestures.Direction.Right)
            {
                Debug.WriteLine("right");

                //currentImage--;
                //if (currentImage < 0)
                //    currentImage = images.Length - 1;
                //NotifyPropertyChanged(() => ImageSource);
            }
            else if (e.Direction == MR.Gestures.Direction.Left)
            {
                Debug.WriteLine("left");
                //currentImage++;
                //if (currentImage >= images.Length)
                //    currentImage = 0;
                //NotifyPropertyChanged(() => ImageSource);
            }

        }

        protected void SetAnchor(Point center)
        {
            // in AnchorX/Y 0.0 means the top left corner and 1.0 means the bottom right
            // unfortunately I don't know how to calculate this correct if Translation, Scale and Rotation is used

            // var xWithinView = center.X - ViewX;
            // var yWithinView = center.Y - ViewY;
            // AnchorX = ViewWidth / xWithinView;
            // AnchorY = ViewHeight / yWithinView;
        }
    }
}
