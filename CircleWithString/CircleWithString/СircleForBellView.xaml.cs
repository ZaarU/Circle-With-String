using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CircleWithString
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Pass false property in IsVisible to make the circle invisible.
    /// </summary>
    public partial class СircleForBellView : SKCanvasView
    {
        private const int _textLengthMax = 3;

        public static readonly BindableProperty CircleBackgroundColorProperty =
            BindableProperty.Create(nameof(CircleBackgroundColor), typeof(SKColor),
                typeof(СircleForBellView), SKColor.Parse("#FF2C6FC8"));

        public static readonly BindableProperty InnerTextColorProperty =
            BindableProperty.Create(nameof(InnerTextColor), typeof(SKColor),
                typeof(СircleForBellView), SKColors.White);

        public static readonly BindableProperty InnerTextProperty =
            BindableProperty.Create(nameof(InnerText), typeof(string),
                typeof(СircleForBellView),
                default(string), BindingMode.OneWay, null,
                _bindingPropertyChangedDelegate, null, _сoerceValueDelegate);

        private static void _bindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            ((СircleForBellView)bindable).InvalidateSurface();
            ((СircleForBellView)bindable).DoAnimation();
        }

        private static object _сoerceValueDelegate(BindableObject bindable, object value)
        {
            string innerText = (string)value;

            {
                if (innerText != null && innerText.Length > _textLengthMax)
                    return innerText.Substring(0, _textLengthMax);
            }
            return innerText;
        }

        public SKColor CircleBackgroundColor
        {
            get
            {
                return (SKColor)GetValue(CircleBackgroundColorProperty);
            }
            set
            {
                SetValue(CircleBackgroundColorProperty, value);
            }
        }

        public SKColor InnerTextColor
        {
            get
            {
                return (SKColor)GetValue(InnerTextColorProperty);
            }
            set
            {
                SetValue(InnerTextColorProperty, value);
            }
        }

        /// <summary>
        /// Inner text.
        /// </summary>
        /// <value>Innder string. Maximum 3 symbols</value>
        public string InnerText
        {
            get => (string)GetValue(InnerTextProperty);
            set
            {
                SetValue(InnerTextProperty, value);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            DrawContent(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            base.OnPaintSurface(e);
        }

        private void DrawContent(SKCanvas canvas, int width, int height)
        {
            canvas.Clear();
            using (new SKAutoCanvasRestore(canvas))
            {
                canvas.Translate(width / 2.5f, height / 2.5f);

                var relativeSize = (width < height ? width : height) / 3.8f;

                var paintCircle = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = CircleBackgroundColor,
                    IsAntialias = true,
                };

                canvas.DrawCircle(0, 0, relativeSize, paintCircle);

                if (!string.IsNullOrEmpty(InnerText))
                {
                    var paintText = new SKPaint()
                    {
                        Style = SKPaintStyle.StrokeAndFill,
                        TextSize = relativeSize,
                        Color = InnerTextColor,
                        FakeBoldText = true
                    };

                    var textXPoint = GetTextXPoint(InnerText, relativeSize);
                    var textYPoint = relativeSize / 3f;

                    canvas.DrawText(InnerText, new SKPoint(textXPoint, textYPoint), paintText);
                }
            }
        }

        private float GetTextXPoint(string innerText, float relativeSize)
        {
            float textXPoint = -relativeSize / 1.5f;
            switch (innerText.Length)
            {
                case 1:
                    textXPoint = -relativeSize / 3.5f;
                    break;
                case 2:
                    textXPoint = -relativeSize / 1.9f;
                    break;
                case 3:
                    textXPoint = -relativeSize / 1.25f;
                    break;
            }
            return textXPoint;
        }

        public void DoAnimation()
        {
            var animation = new Animation(v => Scale = v, 0.2, 1);
            animation.Commit(this, "SimpleAnimation", 16, 300, Easing.Linear);
        }
    }
}