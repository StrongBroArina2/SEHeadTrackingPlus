﻿using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Game.Utils;
using VRageMath;

namespace HeadTrackingPlugin
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    internal class SessionComponent : MySessionComponentBase
    {
        public bool TestMode => Settings.TestModeEnabled;

        private static readonly string ConfigFilePath;

        public static SessionComponent Instance { get; private set; }

        public HeadTrackingSettings Settings => HeadTrackingSettings.Instance;

        static SessionComponent()
        {
            var appdata = Environment.GetEnvironmentVariable("appdata");
            ConfigFilePath = appdata + $"/SpaceEngineers/HeadTrackingPluginPlus.cfg";
        }

        public override void LoadData()
        {
            Instance = this;
            MyAPIGateway.Utilities.MessageEntered += Handle_Message;
        }

        protected override void UnloadData()
        {
            Instance = null;
            MyAPIGateway.Utilities.MessageEntered -= Handle_Message;
        }

        private HeadTrackingSettingsGui settingsGui = null;

        private void Handle_Message(string rawMessage, ref bool sendToOthers)
        {
            var message = rawMessage.ToLower();
            if (message.StartsWith("/ht_options"))
            {
                sendToOthers = false;

                if (settingsGui == null)
                {
                    settingsGui = new HeadTrackingSettingsGui();
                    settingsGui.Closed += (_, __) => { settingsGui = null; };
                    MyGuiSandbox.AddScreen(settingsGui);
                }
            }
            if (message.StartsWith("/ht_testmode"))
            {
                sendToOthers = false;

                var split = message.Split();
                if (split.Length == 2)
                {
                    var arg = split[1];
                    if (arg == "on" || arg == "true") Settings.TestModeEnabled = true;
                    if (arg == "off" || arg == "false") Settings.TestModeEnabled = false;
                    if (arg == "toggle") Settings.TestModeEnabled = !Settings.TestModeEnabled;

                }
            }

            var mode = TestMode ? "ON" : "OFF";
            //MyAPIGateway.Utilities.ShowMessage("HeadTracking", $"Test mode is {mode}"); СДОХНИИИ! УРААА!
        }

        public override void Draw()
        {
            // For a MySessionComponentBase in a plugin, Draw() is called by MySession.DrawAsync().
            // This causes flickering that is most likely due to camera view matrix being updated at the wrong time.
            // For mods, Draw() is called by MySession.DrawSync(). 
            // MySession.DrawSync() is patched to avoid screen flickering.
        }

        private void DoDraw()
        {
            var settings = HeadTrackingSettings.Instance;

            // Проверка активности трекинга лол
            bool isCharacter = MyAPIGateway.Session?.Player?.Character == MyAPIGateway.Session?.CameraController;
            bool isFps = MyAPIGateway.Session?.CameraController?.IsInFirstPersonView ?? false;
            bool active = settings.Enabled && (!isCharacter || (settings.EnabledInCharacter && (!isFps || settings.EnabledInFirstPerson)));

            if (!active) return;

            // Углы с учетом инверсии
            float pitch = (settings.InvertPitch ? -1 : 1) * FreeTrackClient.Pitch;
            float yaw = (settings.InvertYaw ? -1 : 1) * -FreeTrackClient.Yaw;
            float roll = (settings.InvertRoll ? -1 : 1) * FreeTrackClient.Roll;

            // Базовое смещение (только если включен позиционный трекинг)
            Vector3D offset = Vector3D.Zero;
            if (settings.EnablePositionalTracking)
            {
                offset = new Vector3D(
                    (settings.InvertX ? -1 : 1) * FreeTrackClient.PosX * settings.PositionScale * 0.01,
                    (settings.InvertY ? -1 : 1) * FreeTrackClient.PosY * settings.PositionScale * 0.01,
                    (settings.InvertZ ? -1 : 1) * FreeTrackClient.PosZ * settings.PositionScale * 0.01);

                // Нов метд
                if (!settings.UseNewPositionMethod) // Инвертируем 
                {
                    MatrixD rotation = MatrixD.CreateRotationY(yaw) * MatrixD.CreateRotationX(pitch) * MatrixD.CreateRotationZ(roll);
                    Vector3D transformedZ = Vector3D.TransformNormal(new Vector3D(0, 0, offset.Z), rotation);
                    offset.Z = transformedZ.Z; // Только для Z VO ГОЙДА УРААААА оси
                }
            }

            // Камере
            var camera = (MyCamera)MyAPIGateway.Session.Camera;
            if (camera != null)
            {
                MatrixD viewMatrix = camera.ViewMatrix *
                                   MatrixD.CreateRotationZ(roll) *
                                   MatrixD.CreateRotationY(yaw) *
                                   MatrixD.CreateRotationX(pitch);

                viewMatrix.Translation += offset;
                camera.SetViewMatrix(viewMatrix);
                camera.UploadViewMatrixToRender();
            }
        }



        public static void DrawSync()
        {
            Instance?.DoDraw();
        }
    }
}
