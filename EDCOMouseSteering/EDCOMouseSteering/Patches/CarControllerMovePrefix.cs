using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sCarController), "Move")]
public class CarControllerMovePrefix
{
    public static void Prefix(sCarController __instance)
    {
        var steeringWheel = EdcoMouseSteering.Instance;
        if (!steeringWheel || !steeringWheel.MouseSteeringEnabled) return;
        __instance.input.x = EdcoMouseSteering.Instance.MouseSteeringValue;
    }
}