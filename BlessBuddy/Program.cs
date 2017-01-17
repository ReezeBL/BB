using SlimDX.Direct3D9;

namespace BlessBuddy
{
    class Program
    {
        static void Main(string[] args)
        {
            var device = new Device(
                new Direct3D(),
                0,
                DeviceType.Hardware,
                System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle,
                CreateFlags.HardwareVertexProcessing, new PresentParameters());
        }
    }
}
