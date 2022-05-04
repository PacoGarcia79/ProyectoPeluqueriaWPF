using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Clase usada para poder enlazar mediante binding el Calendar contenido en UserControlCitas.Xaml con una ObservableCollection de DateTime que contiene las fechas
    /// ocupadas o no disponibles para realizar las citas
    /// 
    /// <href>https://stackoverflow.com/questions/1638128/how-to-bind-blackoutdates-in-wpf-toolkit-calendar-control</href>
    /// </summary>
    public class CalendarAttachedProperties : DependencyObject
    {

        private static readonly List<Calendar> _calendars = new List<Calendar>();
        private static readonly List<DatePicker> _datePickers = new List<DatePicker>();


        public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached("RegisterBlackoutDates", typeof(ObservableCollection<DateTime>), typeof(CalendarAttachedProperties), new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        public static void SetRegisterBlackoutDates(DependencyObject d, ObservableCollection<DateTime> value)
        {
            d.SetValue(RegisterBlackoutDatesProperty, value);
        }

        public static ObservableCollection<DateTime> GetRegisterBlackoutDates(DependencyObject d)
        {
            return (ObservableCollection<DateTime>)d.GetValue(RegisterBlackoutDatesProperty);
        }

      

        private static void CalendarBindings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<DateTime> blackoutDates = sender as ObservableCollection<DateTime>;

            Calendar calendar = _calendars.First(c => c.Tag == blackoutDates);

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DateTime date in e.NewItems)
                {
                    calendar.BlackoutDates.Add(new CalendarDateRange(date));
                }
            }
        }

        private static void DatePickerBindings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<DateTime> blackoutDates = sender as ObservableCollection<DateTime>;

            DatePicker datePicker = _datePickers.First(c => c.Tag == blackoutDates);

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DateTime date in e.NewItems)
                {
                    datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                }
            }
        }

        private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            if (calendar != null)
            {
                ObservableCollection<DateTime> bindings = e.NewValue as ObservableCollection<DateTime>;
                if (bindings != null)
                {
                    if (!_calendars.Contains(calendar))
                    {
                        calendar.Tag = bindings;
                        _calendars.Add(calendar);
                    }

                    calendar.BlackoutDates.Clear();
                    foreach (DateTime date in bindings)
                    {
                        calendar.BlackoutDates.Add(new CalendarDateRange(date));
                    }
                    bindings.CollectionChanged += CalendarBindings_CollectionChanged;
                }
            }
            else
            {
                DatePicker datePicker = sender as DatePicker;
                if (datePicker != null)
                {
                    ObservableCollection<DateTime> bindings = e.NewValue as ObservableCollection<DateTime>;
                    if (bindings != null)
                    {
                        if (!_datePickers.Contains(datePicker))
                        {
                            datePicker.Tag = bindings;
                            _datePickers.Add(datePicker);
                        }

                        datePicker.BlackoutDates.Clear();
                        foreach (DateTime date in bindings)
                        {
                            datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                        }
                        bindings.CollectionChanged += DatePickerBindings_CollectionChanged;
                    }
                }
            }
        }

    }



    
}
