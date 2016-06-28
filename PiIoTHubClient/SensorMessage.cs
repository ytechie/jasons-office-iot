using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiIoTHubClient
{
    public class SensorMessage
    {
        public double LightLevel { get; set; }
        public double Temperature { get; set; }

        public Message ToIoTMessage()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            var message = new Message(Encoding.UTF8.GetBytes(json));

            return message;
        }
    }
}
