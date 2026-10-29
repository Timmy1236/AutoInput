using AutoInput.services;
using SharpHook;
using SharpHook.Data;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Input;

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
        public SharpHook.Data.MouseButton[] MouseButtons { get; } = Enum.GetValues<SharpHook.Data.MouseButton>();

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
                    int delay = int.TryParse(DelayBox.Text, out int delayN) ? delayN : 10;
                    int delayRandom = int.TryParse(DelayRandomBox.Text, out int delayRandomN) ? delayRandomN : 0;

                    switch ((DeviceType)DeviceBox.SelectedItem)
                    {
                        case DeviceType.Vacio:
                            MessageBox.Show("Necesitas seleccionar primero un dipositivo para automatizar.", "Dipositivo vacio!", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case DeviceType.Mouse:
                            SharpHook.Data.MouseButton mouseSelec = (SharpHook.Data.MouseButton)MouseCodeBox.SelectedItem;
                            _inputAutomation.StartAutoInput(DeviceType.Mouse, mouseSelec, delay, delayRandom);
                            break;
                        case DeviceType.Teclado:
                            KeyCode keySelec = (KeyCode)KeyCodeBox.SelectedItem;
                            _inputAutomation.StartAutoInput(DeviceType.Teclado, keySelec, delay, delayRandom);
                            break;
                        default:
                            break;
                    }
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

        private void DelayBoxValidation(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(cc => Char.IsNumber(cc)))
            {
                e.Handled = false;
                SystemSounds.Exclamation.Play();

            }
            e.Handled = !e.Text.All(cc => Char.IsNumber(cc));
            base.OnPreviewTextInput(e);
        }
    }
}