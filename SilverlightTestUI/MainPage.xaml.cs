using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace SilverlightTestUI
{
    public partial class MainPage : UserControl
    {
        private readonly SignalRSilverlightClient.Connection connection;

        public MainPage()
        {
            InitializeComponent();

            var source = new ObservableCollection<string>();
            ResultBox.ItemsSource = source;

            //Use Hub or PersistentConnection:
            //connection = SignalRSilverlightClient.MakePersistentConnection(SignalRSilverlightClient.aspnetUrl);
            connection = SignalRSilverlightClient.MakeHubConnection(SignalRSilverlightClient.aspnetUrl);
            source.Add("Signal-R Test UI:");

            connection.ResultFeed.ObserveOnDispatcher().Subscribe(
                onNext: r => source.Add(r),
                onError: e => source.Add(e.ToString()));
        }

        private void Send_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            connection.SendRequest(InputText.Text);
        }
    }
}