using AutoInput.services;
using SharpHook;
using SharpHook.Data;
using System.ComponentModel;
using System.Windows;

namespace AutoInput
{
    public enum DeviceType
    {
        Vacio,
        Mouse,
        Teclado
    }

    public partial class MainWindow : Window
    {
        public KeyCode[] KeyCodes { get; } = Enum.GetValues<KeyCode>();
        public DeviceType[] DeviceTypes { get; } = Enum.GetValues<DeviceType>();
        public MouseButton[] MouseButtons { get; } = Enum.GetValues<MouseButton>();

        private readonly GlobalHook _globalHook;
        private readonly InputAutomation _inputAutomation;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _inputAutomation = new InputAutomation();
            _globalHook = new GlobalHook();
            _globalHook.KeyPressed += OnGlobalKeyPressed;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _globalHook.startHook();
        }

        private void MenuShow_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            _globalHook.KeyPressed -= OnGlobalKeyPressed;
            _globalHook.stopHook();
            Application.Current.Shutdown();
        }

        private void OnGlobalKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // NOTE: Es necesario hacer que esto sea configurable y no solamente 'F8'
                if (e.Data.KeyCode == KeyCode.VcF8)
                {
                    DeviceType device = (DeviceType)DeviceBox.SelectedItem;
                    KeyCode keySelec = (KeyCode)KeyCodeBox.SelectedItem;
                    MouseButton mouseSelec = (MouseButton)MouseCodeBox.SelectedItem;
                    _inputAutomation.startAutoInput(device, keySelec, mouseSelec);
                }
            });
        }

        private void DeviceBox_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DeviceBox.SelectedItem is DeviceType selectedDevice)
            {
                // NOTE: ¿Esto se podria mejorar? Medio "hardcoded"(?
                switch (selectedDevice)
                {
                    case DeviceType.Vacio:
                    default:
                        KeyCodeBox.Visibility = Visibility.Collapsed;
                        MouseCodeBox.Visibility = Visibility.Collapsed;
                        break;
                    case DeviceType.Mouse:
                        KeyCodeBox.Visibility = Visibility.Collapsed;
                        MouseCodeBox.Visibility = Visibility.Visible;
                        break;
                    case DeviceType.Teclado:
                        KeyCodeBox.Visibility = Visibility.Visible;
                        MouseCodeBox.Visibility = Visibility.Collapsed;
                        break;

                }
            }
        }
    }
}