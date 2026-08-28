using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sInputManager), "GetInput")]
public static class InputManagerGetInputPostfix
{
    public static void Postfix(sInputManager __instance)
    {
        var steeringWheel = EdcoMouseSteering.Instance;
        if (!steeringWheel || !steeringWheel.MouseSteeringEnabled) return;
        __instance.driveInput = new Vector2(steeringWheel.MouseSteeringValue, __instance.driveInput.y);
    }
}
