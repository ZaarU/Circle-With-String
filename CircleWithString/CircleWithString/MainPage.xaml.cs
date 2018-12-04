using System;

using Xamarin.Forms;

namespace CircleWithString
{
    public partial class MainPage : ContentPage
    {
        private readonly double _canvasViewHeightMax;
        private readonly Random _rand;

        public MainPage()
        {
            InitializeComponent();
            _canvasViewHeightMax = _circleView.HeightRequest;
            _rand = new Random();
            _circleView.InnerTextColor = SkiaSharp.SKColors.Pink;
            _circleView.CircleBackgroundColor = SkiaSharp.SKColors.Olive;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _circleView.InnerText = _rand.Next(int.MinValue, int.MaxValue).ToString();
        }

        private void _slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _circleView.HeightRequest = _canvasViewHeightMax * (e.NewValue / _slider.Maximum);
        }
    }
}
