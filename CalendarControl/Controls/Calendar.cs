using System;
using System.Linq;
using System.Reactive.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CalendarControl
{
    public sealed partial class Calendar : Control
    {
        static Calendar()
        {
            if (!Application.Current.Resources.MergedDictionaries.Any(d => d.Source == new Uri("ms-appx:///CalendarControl/Themes/Colors.xaml")))
            {
                var res = new ResourceDictionary() { Source = new Uri("ms-appx:///CalendarControl/Themes/Colors.xaml") };
                Application.Current.Resources.MergedDictionaries.Insert(0, res);
            }

            if (!Application.Current.Resources.MergedDictionaries.Any(d => d.Source == new Uri("ms-appx:///CalendarControl/Themes/Defaults.xaml")))
            {
                var res = new ResourceDictionary() { Source = new Uri("ms-appx:///CalendarControl/Themes/Defaults.xaml") };
                Application.Current.Resources.MergedDictionaries.Insert(0, res);
            }
        }

        public Calendar()
        {
            this.DefaultStyleKey = typeof(Calendar);
            this.ViewModel = new CalendarViewModel();
            // this.SizeChanged += Calendar_SizeChanged;
            this.RegisterPropertyChangedCallback(PaddingProperty, OnPaddingChanged);

            var sizeChangedObservable = Observable.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
                handler => this.SizeChanged += handler,
                handler => this.SizeChanged -= handler);

            sizeChangedObservable.Throttle(TimeSpan.FromSeconds(0.3)).ObserveOnDispatcher(CoreDispatcherPriority.Low).Subscribe(x =>
            {
                ViewModel.SizeChanged(x.EventArgs.NewSize);
            });
        }

        private void OnPaddingChanged(DependencyObject sender, DependencyProperty dp)
        {
            var padding = (Thickness)sender.GetValue(dp);
            var header = (GetTemplateChild("CalendarHeader") as ContentPresenter);
            var scroller = (GetTemplateChild("CalendarScroller") as ScrollViewer);
            if (padding != null && header != null && scroller != null)
            {
                header.Padding = new Thickness(padding.Left, padding.Top, padding.Right, 0);
                scroller.Padding = new Thickness(padding.Left, 0, padding.Right, padding.Bottom);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.OnPaddingChanged(this, PaddingProperty);
        }
    }
}
