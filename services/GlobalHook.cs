using SharpHook;

namespace AutoInput.services
{
    internal class GlobalHook
    {
        public event EventHandler<KeyboardHookEventArgs>? KeyPressed;
        private readonly TaskPoolGlobalHook _hook = new TaskPoolGlobalHook();

        public async void startHook()
        {
            _hook.KeyPressed += (sender, e) => KeyPressed?.Invoke(this, e);
            await _hook.RunAsync();
        }

        public void stopHook()
        {
            _hook.Dispose();
        }
    }
}
