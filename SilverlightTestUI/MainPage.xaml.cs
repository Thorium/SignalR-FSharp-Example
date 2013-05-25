using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace SilverlightTestUI
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            var source = new ObservableCollection<string>();
            ResultBox.ItemsSource = source;

            //Use Hub or PersistentConnection:
            var connection = SignalRSilverlightClient.MakePersistentConnection(SignalRSilverlightClient.aspnetUrl);
            //var connection = SignalRSilverlightClient.MakeHubConnection(SignalRSilverlightClient.aspnetUrl, new object[]{"Hello from client"});
            source.Add("Signal-R Test UI:");

            connection.ObserveOnDispatcher().Subscribe(
                onNext: r => source.Add(r),
                onError: e => source.Add(e.ToString()));
        }
    }
}