using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EDCOMouseSteering;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class EdcoMouseSteering : BaseUnityPlugin
{
    public static EdcoMouseSteering Instance { get; private set; }

    public bool MouseSteeringEnabled { get; private set; }
    public float MouseSteeringValue { get; private set; }

    private float _sensitivity = 1f;
    private InputActionAsset _customAsset;
    private InputActionMap _customMap;
    private InputAction _toggleMouseSteering;
    private InputAction _lookAction;
    private InputAction _decreaseSensitivity;
    private InputAction _increaseSensitivity;

    private bool _isInitialized;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            try
            {
                InitializeInputAndPatches();
                _isInitialized = true;
                Logger.LogInfo("EDCOMouseSteering: Ввод и Harmony-патчи успешно инициализированы!");
            }
            catch (System.TypeLoadException)
            {
                return;
            }
        }

        if (_lookAction == null) return;
        var mouseDelta = _lookAction.ReadValue<Vector2>();

        if (mouseDelta != Vector2.zero && MouseSteeringEnabled && !PauseSystem.paused)
            MouseSteeringValue = Mathf.Clamp(MouseSteeringValue + mouseDelta.x / 25f * _sensitivity * Time.deltaTime * 10f, -1f, 1f);
    }

    private void InitializeInputAndPatches()
    {
        _customAsset = ScriptableObject.CreateInstance<InputActionAsset>();

        _customMap = _customAsset.AddActionMap("ModInputMap");

        _toggleMouseSteering = _customMap.AddAction("ToggleMouseSteering", binding: "<Mouse>/leftButton");
        _lookAction = _customMap.AddAction("MouseLook", type: InputActionType.Value);
        _lookAction.AddBinding("<Mouse>/delta");

        _decreaseSensitivity = _customMap.AddAction("DecreaseSensitivity", binding: "<Keyboard>/minus");
        _increaseSensitivity = _customMap.AddAction("IncreaseSensitivity", binding: "<Keyboard>/equals");

        _toggleMouseSteering.performed += OnToggleMouseSteering;
        _decreaseSensitivity.performed += OnDecreaseSensitivity;
        _increaseSensitivity.performed += OnIncreaseSensitivity;

        _customAsset.Enable();

        var harmony = new Harmony("com.flyen256.mousesteering");
        harmony.PatchAll();
    }

    private void OnDestroy()
    {
        if (_toggleMouseSteering != null) _toggleMouseSteering.performed -= OnToggleMouseSteering;
        if (_decreaseSensitivity != null) _decreaseSensitivity.performed -= OnDecreaseSensitivity;
        if (_increaseSensitivity != null) _increaseSensitivity.performed -= OnIncreaseSensitivity;

        if (!_customAsset) return;
        _customAsset.Disable();
        Destroy(_customAsset);
    }

    private void OnToggleMouseSteering(InputAction.CallbackContext context)
    {
        if (PauseSystem.paused) return;
        MouseSteeringEnabled = !MouseSteeringEnabled;
        MouseSteeringValue = 0f;
        Logger.LogInfo($"Mouse steering toggled: {MouseSteeringEnabled}");
    }

    private void OnIncreaseSensitivity(InputAction.CallbackContext context)
    {
        _sensitivity += 0.05f;
        Logger.LogInfo($"Sensitivity increased: {_sensitivity:F2}");
    }

    private void OnDecreaseSensitivity(InputAction.CallbackContext context)
    {
        _sensitivity = Mathf.Max(0.01f, _sensitivity - 0.05f);
        Logger.LogInfo($"Sensitivity decreased: {_sensitivity:F2}");
    }
}
