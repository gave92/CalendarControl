using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CalendarControl
{
    public sealed partial class DayView : Control
    {
        private ContainerVisual _foreground, _background;
        private Canvas _canvas;

        private bool IsPressed;
        private int PressedHour;

        public DayView()
        {
            this.DefaultStyleKey = typeof(DayView);
            this.DataContextChanged += (s,e) => { Day = e.NewValue as Day; };
        }

        private void OnDayChanged(DependencyPropertyChangedEventArgs e)
        {
            // Delete and redraw all canvas content
            if (_canvas != null)
            {
                _background.Children.RemoveAll();
                _foreground.Children.RemoveAll();
                _canvas.Children.Clear();
                UpdateCanvas(_canvas);
            }

            // Detach callbacks and attach to new objects
            if (e.OldValue != null)
            {
                foreach (var h in ((Day)e.OldValue).Hours)
                {
                    h.PropertyChanged -= OnHourChanged;
                }
            }
            if (e.NewValue != null)
            {
                foreach (var h in ((Day)e.NewValue).Hours)
                {
                    h.PropertyChanged += OnHourChanged;
                }
            }
        }

        private void OnHourChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if (_foreground == null) return;
                var hour = sender as Hour;
                var sprite = _foreground.Children.ElementAtOrDefault(Day.Hours.IndexOf(hour)) as SpriteVisual;
                SetHourBackground(sprite, hour.IsSelected);
            }
        }

        protected override void OnApplyTemplate()
        {
            this.ToggleDateVisibility();
            this.ToggleHourVisibility();

            _canvas = GetTemplateChild("DayCanvas") as Canvas;
            if (_canvas != null)
            {
                GetVisual(_canvas, ref _foreground, ref _background);
                _canvas.PointerPressed += OnCanvasPointerPressed;
                _canvas.PointerMoved += OnCanvasPointerMoved;
                _canvas.PointerReleased += OnCanvasPointerLost;
                _canvas.PointerCaptureLost += OnCanvasPointerLost;
                _canvas.PointerExited += OnCanvasPointerLost;
                _canvas.SizeChanged += (s, e) => UpdateCanvas(_canvas);
            }

            base.OnApplyTemplate();
        }

        private void SetHourBackground(SpriteVisual sprite, bool selected = false, bool over = false)
        {
            if (sprite == null) return;
            if (selected)
            {
                sprite.Brush = _foreground.Compositor.CreateColorBrush(HourSelectedBackground.Color);
                sprite.Opacity = (float)HourSelectedBackground.Opacity;
            }
            else if (over)
            {
                sprite.Brush = _foreground.Compositor.CreateColorBrush(HourPointerOverBackground.Color);
                sprite.Opacity = (float)HourPointerOverBackground.Opacity;
            }
            else
            {
                sprite.Brush = _foreground.Compositor.CreateColorBrush(Colors.Transparent);
                sprite.Opacity = 1;
            }
        }

        private void ResetHourBackground()
        {
            for (var i = 0; i < Day.Hours.Count; i++)
            {
                var sprite = _foreground.Children.ElementAtOrDefault(i) as SpriteVisual;
                this.SetHourBackground(sprite, Day.Hours[i].IsSelected);
            }
        }

        private void OnCanvasPointerLost(object sender, PointerRoutedEventArgs e)
        {
            // Reset background color for all sprites
            this.ResetHourBackground();
            this.IsPressed = false;
        }

        private void OnCanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var position = e.GetCurrentPoint(canvas).Position;
            PressedHour = (int)(position.Y / canvas.ActualHeight * Day.Hours.Count);

            // Reset selection
            for (var i = 0; i < Day.Hours.Count; i++)
            {
                if (PressedHour != i)
                    Day.Hours[i].IsSelected = false;
            }

            // (De)Select pressed hour
            if (!Day.Hours[PressedHour].IsSelected)
            {
                Day.Hours[PressedHour].IsSelected = true;
                this.IsPressed = true;
            }
            else
            {
                Day.Hours[PressedHour].IsSelected = false;
            }
        }

        private void OnCanvasPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var position = e.GetCurrentPoint(canvas).Position;
            var hour = (int)(position.Y / canvas.ActualHeight * Day.Hours.Count);

            if (IsPressed)
            {
                // Select hours
                if (hour < PressedHour) return;
                for (int i = PressedHour; i <= hour; i++)
                    Day.Hours[i].IsSelected = true;
                PressedHour = hour;
            }
            else
            {
                // Show mouse hover
                this.ResetHourBackground();
                SpriteVisual sprite = _foreground.Children.ElementAtOrDefault(hour) as SpriteVisual;
                this.SetHourBackground(sprite, over: true);
            }
        }

        private void UpdateCanvas(Canvas canvas)
        {
            if (_foreground.Compositor == null
                || canvas.ActualHeight == 0) return;            

            if (IsHourVisible)
            {
                UpdateHours(canvas);
            }
            else
            {
                UpdateBackground(canvas);
                UpdateRect(canvas);
            }
        }

        private void UpdateBackground(Canvas canvas)
        {
            if (!_background.Children.Any())
            {
                SpriteVisual tick = _background.Compositor.CreateSpriteVisual();
                tick.Size = new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight);
                tick.Brush = _background.Compositor.CreateColorBrush(Day.IsToday ? DayTodayBackground.Color : DayNormalBackground.Color);
                _background.Children.InsertAtTop(tick);
            }
            else
            {
                var visual = _background.Children.ElementAt(0);
                visual.Size = new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight);
            }
        }

        private void UpdateRect(Canvas canvas)
        {
            if (!_foreground.Children.Any())
            {
                for (var i = 0; i < Day.Hours.Count; i++)
                {
                    SpriteVisual tick = _foreground.Compositor.CreateSpriteVisual();
                    tick.Size = new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight / Day.Hours.Count);
                    tick.Offset = new Vector3(0, i * (float)canvas.ActualHeight / Day.Hours.Count, 0);
                    _foreground.Children.InsertAtTop(tick);
                }
            }
            else
            {
                for (var i = 0; i < _foreground.Children.Count; i++)
                {
                    var visual = _foreground.Children.ElementAt(i);
                    visual.Size = new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight / Day.Hours.Count);
                    visual.Offset = new Vector3(0, i * (float)canvas.ActualHeight / Day.Hours.Count, 0);
                }
            }
        }

        private void UpdateHours(Canvas canvas)
        {
            if (!canvas.Children.Any())
            {
                for (var i = 0; i < Day.Hours.Count; i++)
                {
                    var ui = new ContentPresenter();
                    ui.Content = Day.Hours[i];
                    ui.ContentTemplate = HourItemTemplate;
                    ui.Height = canvas.ActualHeight / Day.Hours.Count;
                    Canvas.SetTop(ui, i * (float)canvas.ActualHeight / Day.Hours.Count);
                    canvas.Children.Add(ui);
                }
            }
            else
            {
                for (var i = 0; i < canvas.Children.Count; i++)
                {
                    var ui = canvas.Children[i] as FrameworkElement;
                    ui.Height = canvas.ActualHeight / Day.Hours.Count;
                    Canvas.SetTop(ui, i * (float)canvas.ActualHeight / Day.Hours.Count);
                }
            }
        }

        private void GetVisual(UIElement element, ref ContainerVisual foreground, ref ContainerVisual background)
        {
            var hostVisual = ElementCompositionPreview.GetElementVisual(element);
            var root = hostVisual.Compositor.CreateContainerVisual();
            background = root.Compositor.CreateContainerVisual();
            foreground = root.Compositor.CreateContainerVisual();
            root.Children.InsertAtTop(background);
            root.Children.InsertAtTop(foreground);
            ElementCompositionPreview.SetElementChildVisual(element, root);
        }

        private void ToggleHourVisibility()
        {
            var content = GetTemplateChild("DayEvents") as EventView;
            if (content != null)
            {
                content.Visibility = IsHourVisible ? Visibility.Collapsed : Visibility.Visible;
            }

            if (_canvas != null)
            {
                _background.Children.RemoveAll();
                _foreground.Children.RemoveAll();
                _canvas.Children.Clear();
                this.UpdateCanvas(_canvas);
            }
        }

        private void ToggleDateVisibility()
        {
            var header = GetTemplateChild("DayHeader") as ContentPresenter;
            if (header != null)
            {
                header.Opacity = IsDateVisible ? 1 : 0;
            }
        }
    }
}
