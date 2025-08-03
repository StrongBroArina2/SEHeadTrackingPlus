using HarmonyLib;
using Sandbox.ModAPI;
using System;
using System.Reflection;
using VRage.Plugins;
using VRage.Input;

namespace HeadTrackingPlugin
{
    public class HeadTrackingPlugin : IPlugin
    {
        public void Dispose()
        {
        }

        public void Init(object gameInstance)
        {
            Log.Info($"Head Tracking Plugin Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            new Harmony("com.corben.spacedout.HeadTrackingPlugin").PatchAll(Assembly.GetExecutingAssembly());
            HeadTrackingSettings.Instance.ToString();
        }

        public void Update()
        {
            var settings = HeadTrackingSettings.Instance;
            if (settings.EnableToggleKey != MyKeys.None && MyInput.Static.IsNewKeyPressed(settings.EnableToggleKey))
            {
                settings.Enabled = !settings.Enabled;
                settings.Save();
            }
        }
    }
}