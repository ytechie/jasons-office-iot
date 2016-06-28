using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using GHIElectronics.UWP.Shields;
using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PiIoTHubClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FEZHAT hat;
        private DispatcherTimer timer;
        private DispatcherTimer sensorTimer;
        private bool ledOn = false;

        private DeviceClient deviceClient;

        public MainPage()
        {
            this.InitializeComponent();

            InitializeDeviceClient();
            InitializeSensors();
            InitializeLight();
        }

        private async void InitializeDeviceClient()
        {
            deviceClient = DeviceClient.CreateFromConnectionString("");
            var message = new Message(Encoding.UTF8.GetBytes("online"));
            await deviceClient.SendEventAsync(message);
        }

        private void InitializeSensors()
        {
            sensorTimer = new DispatcherTimer();
            sensorTimer.Interval = TimeSpan.FromMinutes(1);
            sensorTimer.Tick += SensorTimer_Tick;
            sensorTimer.Start();
        }

        private async void SensorTimer_Tick(object sender, object e)
        {
            var message = new SensorMessage();

            message.Temperature = hat.GetTemperature();
            message.LightLevel = hat.GetLightLevel();

            try
            {
                await deviceClient.SendEventAsync(message.ToIoTMessage());
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error sending message: " + ex.ToString());
            }
        }

        private async void InitializeLight()
        {
            this.hat = await FEZHAT.CreateAsync();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(300);
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (ledOn)
            {
                this.hat.D2.Color = FEZHAT.Color.Blue;
                this.hat.D3.Color = FEZHAT.Color.Yellow;
            }
            else
            {
                this.hat.D2.Color = FEZHAT.Color.Red;
                this.hat.D3.Color = FEZHAT.Color.Blue;
            }

            ledOn = !ledOn;
        }
    }
}
