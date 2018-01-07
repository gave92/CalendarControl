using CalendarControl.Interfaces;
using Windows.UI.Xaml;

namespace CalendarControl
{
    public partial class Calendar
    {
        public bool IsHourSelectionEnabled
        {
            get { return (bool)GetValue(IsHourSelectionEnabledProperty); }
            set { SetValue(IsHourSelectionEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHourSelectionEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHourSelectionEnabledProperty =
            DependencyProperty.Register("IsHourSelectionEnabled", typeof(bool), typeof(Calendar), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the template used to display the content of the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public ICalendarViewModel ViewModel
        {
            get { return (ICalendarViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ICalendarViewModel), typeof(Calendar), new PropertyMetadata(null));

        public DataTemplate DayHeaderTemplate
        {
            get { return (DataTemplate)GetValue(DayHeaderTemplateProperty); }
            set { SetValue(DayHeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DayHeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayHeaderTemplateProperty =
            DependencyProperty.Register("DayHeaderTemplate", typeof(DataTemplate), typeof(Calendar), new PropertyMetadata(null));

        public DataTemplate HourItemTemplate
        {
            get { return (DataTemplate)GetValue(HourItemTemplateProperty); }
            set { SetValue(HourItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourItemTemplateProperty =
            DependencyProperty.Register("HourItemTemplate", typeof(DataTemplate), typeof(Calendar), new PropertyMetadata(null));

        public double CanvasMinHeight
        {
            get { return (double)GetValue(CanvasMinHeightProperty); }
            set { SetValue(CanvasMinHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasMinHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasMinHeightProperty =
            DependencyProperty.Register("CanvasMinHeight", typeof(double), typeof(Calendar), new PropertyMetadata(0));

        public double CanvasMaxHeight
        {
            get { return (double)GetValue(CanvasMaxHeightProperty); }
            set { SetValue(CanvasMaxHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasMaxHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasMaxHeightProperty =
            DependencyProperty.Register("CanvasMaxHeight", typeof(double), typeof(Calendar), new PropertyMetadata(0));
    }
}
