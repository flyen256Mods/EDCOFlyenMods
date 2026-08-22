using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sCarController), "Move")]
public class CarController_Move_Prefix
{
    public static void Prefix(sCarController __instance, ref Vector2 ___targetInput)
    {
        if (!EdcoMouseSteering.Instance.MouseSteeringEnabled) return;
        __instance.input.x = EdcoMouseSteering.Instance.MouseSteeringValue;
    }
}