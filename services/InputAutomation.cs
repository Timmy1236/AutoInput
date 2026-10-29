using SharpHook.Simulation;
using SharpHook.Data;

namespace AutoInput.services
{
    internal class InputAutomation
    {
        Random random = new Random();
        private bool _activated = false;
        private readonly EventSimulator _simulator = EventSimulator.Create("AutoInput");

        // Teclado
        public async void StartAutoInput(DeviceType device, KeyCode keySelec, int delay = 10, int delayRandom = 0)
        {
            _activated = !_activated;

            while (_activated)
            {
                _simulator.SimulateKeyPress(keySelec);

                await Task.Delay(delay + random.Next(delayRandom));
            }
        }

        // Mouse
        public async void StartAutoInput(DeviceType device, MouseButton mouseSelec, int delay = 10, int delayRandom = 0)
        {
            _activated = !_activated;

            while (_activated)
            {
                _simulator.SimulateMousePress(mouseSelec);
                _simulator.SimulateMouseRelease(mouseSelec);

                await Task.Delay(delay + random.Next(delayRandom));
            }
        }
    }
}