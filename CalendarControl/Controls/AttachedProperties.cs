﻿using Windows.UI.Xaml;

namespace CalendarControl
{
    public class AttachedProperties
    {
        public static bool GetIsHourSelectionEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHourSelectionEnabledProperty);
        }

        public static void SetIsHourSelectionEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHourSelectionEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsHourSelectionEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHourSelectionEnabledProperty =
            DependencyProperty.RegisterAttached("IsHourSelectionEnabled", typeof(bool), typeof(AttachedProperties), new PropertyMetadata(true));

        public static DataTemplate GetDayHeaderTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(DayHeaderTemplateProperty);
        }

        public static void SetDayHeaderTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(DayHeaderTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for DayHeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayHeaderTemplateProperty =
            DependencyProperty.RegisterAttached("DayHeaderTemplate", typeof(DataTemplate), typeof(AttachedProperties), new PropertyMetadata(null));

        public static DataTemplate GetHourItemTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(HourItemTemplateProperty);
        }

        public static void SetHourItemTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(HourItemTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for HourItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourItemTemplateProperty =
            DependencyProperty.RegisterAttached("HourItemTemplate", typeof(DataTemplate), typeof(AttachedProperties), new PropertyMetadata(null));
    }
}
