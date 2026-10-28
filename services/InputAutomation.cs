using SharpHook.Simulation;
using SharpHook.Data;

namespace AutoInput.services
{
    internal class InputAutomation
    {
        private bool _activated = false;
        private readonly EventSimulator _simulator = EventSimulator.Create("AutoInput");

        public async void startAutoInput(DeviceType device, KeyCode keySelec, MouseButton mouseSelec)
        {
            _activated = !_activated;

            if (device == DeviceType.Mouse)
            {
                while (_activated)
                {
                    _simulator.SimulateMousePress(mouseSelec);
                    _simulator.SimulateMouseRelease(mouseSelec);

                    await Task.Delay(1);
                }
            }
            else if (device == DeviceType.Teclado)
            {
                while (_activated)
                {
                    _simulator.SimulateKeyPress(keySelec);

                    await Task.Delay(1);
                }
            }
        }
    }
}
