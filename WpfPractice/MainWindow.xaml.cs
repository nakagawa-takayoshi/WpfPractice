using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using R3;

namespace WpfPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReactiveProperty<string> _textTimeSpan = new(String.Empty);
        private readonly TimeSpan _sourceTimeSpan;
        private TimeSpan _timeSpan = TimeSpan.Zero;
        private DispatcherTimer _timer = new();
        private readonly List<TimeSpan> _timeSpanListy = new()
        {
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromSeconds(5),
        };

        private CompositeDisposable _disposables = new();

        public MainWindow()
        {
            InitializeComponent();
            _buttonStop.IsEnabled = false;
            _sourceTimeSpan = _timeSpanListy[0];
            _texrtBlockTimeSpan.Text = _sourceTimeSpan.ToString();
            _textTimeSpan.Subscribe(x => _texrtBlockTimeSpan.Text = _timeSpan.ToString()).AddTo(_disposables);
            _timer.Stop();

        }

        private void _button_Click(object sender, RoutedEventArgs e)
        {
            CompositeDisposable disposables = new();
            if (_timer.IsEnabled) return;
            _timeSpan = _sourceTimeSpan;
            _timer.TickAsObservable().Subscribe(_ =>
            {
                if (_timeSpan == TimeSpan.Zero) return;
                _timeSpan = _timeSpan.Subtract(TimeSpan.FromSeconds(1));
                _textTimeSpan.Value = _timeSpan.ToString();
            }).AddTo(disposables);
            _timer.TickAsObservable().Where(_ => _timeSpan == TimeSpan.Zero).Subscribe(_ =>
            {
                _button.IsEnabled = true;
                _timer.Stop();
                if (disposables.IsDisposed) return;
                disposables.Dispose();
                _texrtBlockTimeSpan.Text = _sourceTimeSpan.ToString();
            }).AddTo(disposables);

            _button.IsEnabled = false;
            _buttonStop.IsEnabled = true;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }


        private void _buttonStop_Click(object sender, RoutedEventArgs e)
        {
            _button.IsEnabled = true;
            _buttonStop.IsEnabled = false;
            _timeSpan = TimeSpan.Zero;
        }
    }
}