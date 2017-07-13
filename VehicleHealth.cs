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
        public VehicleHealth()
        {
            Debug.WriteLine("Advanced vehicle damage created by aabbfive - https://discord.gg/x4s4xwu");
            Tick += OnTick;
        }
        public async Task OnTick()
        {
            Ped playerPed = LocalPlayer.Character;
            if (playerPed.IsInVehicle())
            {
                Vehicle vehicle = playerPed.CurrentVehicle;
                if(vehicle != null && vehicle.GetPedOnSeat(VehicleSeat.Driver) == playerPed && vehicle.IsAlive)
                {
                    if (GetVehEngineHealth(vehicle) < 80f)
                    {
                        Screen.ShowNotification("~r~Le véhicule est trop endommagé.");
                        Screen.ShowNotification("~w~Appuyez sur ~b~X~w~ pour appeler un dépanneur.");
                        vehicle.EngineHealth = 100;
                        vehicle.IsDriveable = false;
                        vehicle.IsEngineRunning = false;
                        Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, vehicle, 4, false, false);
                    }
                    else if (GetVehBodyHealth(vehicle) < 60f)
                    {
                        Screen.ShowNotification("~r~Le véhicule est trop endommagé.");
                        Screen.ShowNotification("~w~Appuyez sur ~b~X~w~ pour appeler un dépanneur.");
                        vehicle.IsDriveable = false;
                        vehicle.IsEngineRunning = false;
                    }else if(GetVehHealth(vehicle) < 55f)
                    {
                        Screen.ShowNotification("~r~Le véhicule est trop endommagé.");
                        Screen.ShowNotification("~w~Appuyez sur ~b~X~w~ pour appeler un dépanneur.");
                        vehicle.IsDriveable = false;
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
    }
}
