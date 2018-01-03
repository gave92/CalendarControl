using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CalendarControl
{
    public partial class DayView
    {
        public SolidColorBrush DayTodayBackground
        {
            get { return (SolidColorBrush)GetValue(DayTodayBackgroundProperty); }
            set { SetValue(DayTodayBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DayTodayBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayTodayBackgroundProperty =
            DependencyProperty.Register("DayTodayBackground", typeof(SolidColorBrush), typeof(DayView), new PropertyMetadata(null));

        public SolidColorBrush DayNormalBackground
        {
            get { return (SolidColorBrush)GetValue(DayNormalBackgroundProperty); }
            set { SetValue(DayNormalBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DayNormalBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayNormalBackgroundProperty =
            DependencyProperty.Register("DayNormalBackground", typeof(SolidColorBrush), typeof(DayView), new PropertyMetadata(null));

        public SolidColorBrush HourDisabledBackground
        {
            get { return (SolidColorBrush)GetValue(HourDisabledBackgroundProperty); }
            set { SetValue(HourDisabledBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourDisabledBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourDisabledBackgroundProperty =
            DependencyProperty.Register("HourDisabledBackground", typeof(SolidColorBrush), typeof(DayView), new PropertyMetadata(null));        

        public SolidColorBrush HourSelectedBackground
        {
            get { return (SolidColorBrush)GetValue(HourSelectedBackgroundProperty); }
            set { SetValue(HourSelectedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourSelectedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourSelectedBackgroundProperty =
            DependencyProperty.Register("HourSelectedBackground", typeof(SolidColorBrush), typeof(DayView), new PropertyMetadata(null));

        public SolidColorBrush HourPointerOverBackground
        {
            get { return (SolidColorBrush)GetValue(HourPointerOverBackgroundProperty); }
            set { SetValue(HourPointerOverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourPointerOverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourPointerOverBackgroundProperty =
            DependencyProperty.Register("HourPointerOverBackground", typeof(SolidColorBrush), typeof(DayView), new PropertyMetadata(null));

        public Day Day
        {
            get { return (Day)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Day.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register("Day", typeof(Day), typeof(DayView), new PropertyMetadata(null, OnDayChanged));

        private static void OnDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dayview = d as DayView;
            if (dayview == null) return;
            dayview.OnDayChanged(e);
        }        

        public DataTemplate DayHeaderTemplate
        {
            get { return (DataTemplate)GetValue(DayHeaderTemplateProperty); }
            set { SetValue(DayHeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DayHeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayHeaderTemplateProperty =
            DependencyProperty.Register("DayHeaderTemplate", typeof(DataTemplate), typeof(DayView), new PropertyMetadata(null));

        public DataTemplate HourItemTemplate
        {
            get { return (DataTemplate)GetValue(HourItemTemplateProperty); }
            set { SetValue(HourItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourItemTemplateProperty =
            DependencyProperty.Register("HourItemTemplate", typeof(DataTemplate), typeof(DayView), new PropertyMetadata(null));

        public bool IsHourVisible
        {
            get { return (bool)GetValue(IsHourVisibleProperty); }
            set { SetValue(IsHourVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHourVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHourVisibleProperty =
            DependencyProperty.Register("IsHourVisible", typeof(bool), typeof(DayView), new PropertyMetadata(true, OnIsHourVisibleChanged));

        public bool IsDateVisible
        {
            get { return (bool)GetValue(IsDateVisibleProperty); }
            set { SetValue(IsDateVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDateVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDateVisibleProperty =
            DependencyProperty.Register("IsDateVisible", typeof(bool), typeof(DayView), new PropertyMetadata(true, OnIsDateVisibleChanged));

        private static void OnIsHourVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dayview = d as DayView;
            if (dayview == null) return;
            dayview.ToggleHourVisibility();
        }

        private static void OnIsDateVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dayview = d as DayView;
            if (dayview == null) return;
            dayview.ToggleDateVisibility();
        }
    }
}
