using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sCarController), "SetInput", typeof(Vector2))]
public static class CarController_SetInput_Postfix
{
    public static void Postfix(sCarController __instance, ref Vector2 input, ref Vector2 ___targetInput)
    {
        ___targetInput.x = input.x;
    }
}
