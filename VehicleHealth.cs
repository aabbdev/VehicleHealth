using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleHealth
{
    public class VehicleHealth : BaseScript
    {
        private string messageDamage;
        private string messageCall;
        private string messageSubmerssion;
        private string messageBreakEngine;

        private bool ShowMessageDamage;
        private bool ShowMessageCall;
        private bool ShowmessageSubmerssion;
        private bool ShowmessageBreakEngine;

        private Config config;
        public VehicleHealth()
        {
            messageDamage = string.Empty;
            messageCall = string.Empty;
            messageSubmerssion = string.Empty;
            messageBreakEngine = string.Empty;

            ShowMessageDamage = false;
            ShowMessageCall = false;
            ShowmessageSubmerssion = false;
            ShowmessageBreakEngine = false;
            string configContent = Convert(Function.Call<string>(Hash.LOAD_RESOURCE_FILE, "vehiclehealth", "config.ini"));
            config = new Config(configContent);
            messageDamage = config.Get("msgdamage", "");
            messageCall = config.Get("msgcall", "");
            messageSubmerssion = config.Get("msgsubmerssion", "");
            messageBreakEngine = config.Get("msgbreakengine", "");

            ShowMessageDamage = bool.Parse(config.Get("showmmsgdamage", "true"));
            ShowMessageCall = bool.Parse(config.Get("showmmsgcall", "true"));
            ShowmessageSubmerssion = bool.Parse(config.Get("showmmsgsubmerssion", "true"));
            ShowmessageBreakEngine = bool.Parse(config.Get("showmsgbreakengine", "true"));
            Debug.WriteLine("Advanced vehicle damage created by aabbfive - https://discord.gg/x4s4xwu");
            Tick += OnTick;
        }
        public string Convert(string str)
        {
            string resulta = str;
            Dictionary<string, string> escape = new Dictionary<string, string>();
            escape.Add("&acirc;", "â");
            escape.Add("&agrave;", "à");
            escape.Add("&eacute;", "é");
            escape.Add("&ecirc;", "ê");
            escape.Add("&egrave;", "è");
            foreach(KeyValuePair<string, string> item in escape)
            {
                resulta=resulta.Replace(item.Key, item.Value);
            }
            return resulta;
        }
        public async Task OnTick()
        {
            Ped playerPed = LocalPlayer.Character;
            if (playerPed.IsInVehicle())
            {
                Vehicle vehicle = playerPed.CurrentVehicle;
                if(vehicle != null && vehicle.GetPedOnSeat(VehicleSeat.Driver) == playerPed && vehicle.IsAlive && playerPed.CurrentVehicle.Model.IsCar)
                {
                    if (GetVehEngineHealth(vehicle) < 80f)
                    {
                        if(ShowmessageBreakEngine)
                        {
                            Screen.ShowNotification(messageBreakEngine);
                        }
                        if(ShowMessageCall)
                        {
                            Screen.ShowNotification(messageCall);
                        }
                        vehicle.EngineHealth = 100;
                        vehicle.IsDriveable = false;
                        vehicle.IsEngineRunning = false;
                        Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, vehicle, 4, false, false);
                    }
                    else if (GetVehBodyHealth(vehicle) < 60f)
                    {
                        if(ShowMessageDamage)
                        {
                            Screen.ShowNotification(messageDamage);
                        }
                        if(ShowMessageCall)
                        {
                            Screen.ShowNotification(messageCall);
                        }
                        vehicle.IsDriveable = false;
                        vehicle.IsEngineRunning = false;
                    }else if(GetVehHealth(vehicle) < 55f)
                    {
                        if(ShowMessageDamage)
                        {
                            Screen.ShowNotification(messageDamage);
                        }
                        if(ShowMessageCall)
                        {
                            Screen.ShowNotification(messageCall);
                        }
                        vehicle.IsDriveable = false;
                        vehicle.IsEngineRunning = false;
                    }else if(GetVehSubmission(vehicle) > 30f)
                    {
                        if(ShowmessageSubmerssion)
                        {
                            Screen.ShowNotification(messageSubmerssion);
                        }
                        vehicle.IsEngineRunning = false;
                    }
                }
            }
            await Task.FromResult(0);
        }
        public float GetVehEngineHealth(Vehicle veh)
        {
            return (veh.EngineHealth / 1000f) * 100f;
        }
        public float GetVehBodyHealth(Vehicle veh)
        {
            return (veh.BodyHealth / 1000f) * 100f;
        }
        public float GetVehHealth(Vehicle veh)
        {
            return (veh.HealthFloat / veh.MaxHealthFloat) * 100f;
        }
        public float GetVehSubmission(Vehicle veh)
        {
            return (veh.SubmersionLevel / 1000f) * 100f;
        }
    }
}
