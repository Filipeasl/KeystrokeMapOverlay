using System;
using System.Linq;
using System.Threading;
using InputSimulatorStandard.Native;
using InputSimulatorStandard;
using SharpDX.XInput;
using WindowsInput;
using WindowsInput.Native;

namespace InputMonitor
{
    class Program
    {
        private static readonly InputSimulator inputSimulator = new InputSimulator();
        private static readonly Controller gamepad = new Controller(UserIndex.One);

        static void Main(string[] args)
        {
            Thread keyboardThread = new Thread(KeyPressListener);
            keyboardThread.Start();

            Thread gamepadThread = new Thread(GamepadListener);
            gamepadThread.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void KeyPressListener()
        {
            while (true)
            {
                foreach (var key in Enum.GetValues(typeof(VirtualKeyCode)).Cast<VirtualKeyCode>())
                {
                    if (inputSimulator.InputDeviceState.IsKeyDown(key))
                    {
                        Console.WriteLine($"Key Pressed: {key}");
                    }
                }
                Thread.Sleep(100);
            }
        }

        private static void GamepadListener()
        {
            while (true)
            {
                if (gamepad.IsConnected)
                {
                    var state = gamepad.GetState();
                    Console.WriteLine($"Gamepad: LeftThumbX={state.Gamepad.LeftThumbX}, LeftThumbY={state.Gamepad.LeftThumbY}, RightThumbX={state.Gamepad.RightThumbX}, RightThumbY={state.Gamepad.RightThumbY}");
                }
                else
                {
                    Console.WriteLine("Gamepad not connected.");
                }
                Thread.Sleep(100);
            }
        }
    }
}
