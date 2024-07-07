using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using R3;

namespace WpfPractice
{
    public static class ControlExtensions
    {
        public static Observable<TextCompositionEventArgs> TextChangedAsObservable(this TextBlock control)
        {
            return Observable.FromEvent<TextCompositionEventHandler, TextCompositionEventArgs>(
                                      h => (s, e) => h(e),
                                      h => control.TextInput += h,
                                      h => control.TextInput -= h);
        }

        public static Observable<RoutedEventArgs> ClickAsObservable(this Button control)
        {
            return Observable.FromEvent<RoutedEventHandler, RoutedEventArgs>(
                                    h => (s, e) => h(e),
                                    h => control.Click += h,
                                    h => control.Click -= h);
        }

        public static Observable<EventArgs> TickAsObservable(this DispatcherTimer control)
        {
            return Observable.FromEvent<EventHandler, EventArgs>(
                                   h => (s, e) => h(e),
                                   h => control.Tick += h,
                                   h => control.Tick -= h);
        }
    }
}
