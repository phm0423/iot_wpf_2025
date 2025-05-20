using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;

namespace WpfMqttSubApp.VieModels
{
    public partial class MainViewModel: ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string _brokerHost;
        private string _databaseHost;

        // 속성 BrokerHost, DataHost
        // 메서드 ConnectBoroker Command, ConnectDatabaseCommand

        public MainViewModel(IDialogCoordinator coordinator) 
        {
            this.dialogCoordinator = coordinator;

            BrokerHost = "211.119.12.55";
            DatabaseHost = "211.199.12.55";
        }

        public string BrokerHost
        {
            get => _brokerHost;
            set => SetProperty(ref _brokerHost, value);
        }

        public string DatabaseHost
        {
            get => _databaseHost;
            set => SetProperty(ref _databaseHost, value);
        }

        [RelayCommand]
        public async Task ConnectBroker()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "브로커연결", "브로커연결합니다!");
        }

        [RelayCommand]
        public async Task ConnectDatabase()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "DB연결", "DB연결합니다!");

        }
    }
}
