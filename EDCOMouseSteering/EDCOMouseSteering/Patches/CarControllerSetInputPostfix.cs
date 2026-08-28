using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sCarController), "SetInput", typeof(Vector2))]
public static class CarControllerSetInputPostfix
{
    public static void Postfix(ref Vector2 ___input, ref Vector2 ___targetInput)
    {
        var steeringWheel = EdcoMouseSteering.Instance;
        if (!steeringWheel || !steeringWheel.MouseSteeringEnabled) return;
        ___targetInput.x = ___input.x;
    }
}
