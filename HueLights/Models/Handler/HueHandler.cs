using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HueLights.Models.Handler
{
    public class HueHandler
    {

        private static string[] colors = new string[] { "D60270", "FFEE8A", "ACCB52", "0038A8", "EE1289", "FF83FA", "CD9B1D", "7EC0EE" };


        public bool ControlLight(int id, string color, string mode)
        {
            int cnt = 0;
            ILocalHueClient client = new LocalHueClient("[ip]");
            client.Initialize("[usertoken]");
            var command = new LightCommand();
            switch (mode)
            {
                case "blink":
                    command.Alert = Alert.Multiple;
                    cnt++;
                    break;
                case "disco":
                    return DiscoMode(client, id);
                default:
                    mode = "normal";
                    break;
            }
            try
            {
                command
                    .TurnOn()
                    .SetColor(new RGBColor(color));

                cnt += 2;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return false;
            }
            //command.Alert = Alert.Once;
            //Or start a colorloop
            //command.Effect = Effect.None;
            var t = client.SendCommandAsync(command, new List<string> { "" + id });
            t.Wait();
            var results = t.Result;
            return results.Where(x => x.Success != null).Count() == cnt;
        }

        private bool DiscoMode(ILocalHueClient client, int id)
        {
            Task.Run(() =>
            {
                var command = new LightCommand();
                command.TurnOn();
                command.Alert = Alert.Once;
                foreach (var item in colors)
                {
                    command.SetColor(new RGBColor(item));
                    var t = client.SendCommandAsync(command, new List<string> { "" + id });
                    t.Wait();
                    var results = t.Result;
                    Thread.Sleep(1000);
                }
            }).ConfigureAwait(false);
            return true;

        }
    }
}