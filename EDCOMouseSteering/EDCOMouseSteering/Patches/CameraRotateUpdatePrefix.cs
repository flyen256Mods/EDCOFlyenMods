using HarmonyLib;
using UnityEngine;

namespace EDCOMouseSteering.Patches;

[HarmonyPatch(typeof(sCameraRotate), "Update")]
public class CameraRotateUpdatePrefix
{
    public static bool Prefix(sCameraRotate __instance, ref sCarController ___car, ref Vector3 ___rotation, ref Vector3 ___targetRotation)
    {
        var steeringWheel = EdcoMouseSteering.Instance;
        if (!steeringWheel || !steeringWheel.MouseSteeringEnabled) return true;
        if (PauseSystem.paused)
        {
            Cursor.lockState = CursorLockMode.None;
            return true;
        }

        Cursor.lockState = CursorLockMode.Locked;

        var num5 = ___car.rb.linearVelocity.magnitude / 10f;
        if (___car.GuyActive)
            num5 = 0.5f;

        ___targetRotation = Vector3.Lerp(___targetRotation, Vector3.zero, Time.deltaTime * num5);
        ___rotation = Vector3.Lerp(___rotation, ___targetRotation, Time.deltaTime * 4f);
        
        __instance.target.localEulerAngles = Vector3.right * ___rotation.y;
        __instance.pivot.localEulerAngles = Vector3.up * ___rotation.x;

        return false;
    }
}
