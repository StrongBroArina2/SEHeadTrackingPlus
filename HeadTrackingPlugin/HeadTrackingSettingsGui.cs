using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace HeadTrackingPlugin
{
    public class HeadTrackingSettingsGui : MyGuiScreenBase
    {
        private static readonly float HorizontalSpace = 25.0f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
        private static readonly float VerticalDelta = MyGuiConstants.CONTROLS_DELTA.Y;
        private static readonly float LabelOffsetX = -0.2f;

        public override string GetFriendlyName()
        {
            return "HeadTrackingSettingsGui";
        }

        public HeadTrackingSettingsGui() :
            base(position: new Vector2(0.5f, 0.5f),
                backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR,
                size: new Vector2(0.9f, 0.8f),
                isTopMostScreen: false,
                backgroundTexture: null,
                backgroundTransition: MySandboxGame.Config.UIBkOpacity,
                guiTransition: MySandboxGame.Config.UIOpacity,
                gamepadSlot: null)
        {
            EnabledBackgroundFade = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);
            var settings = HeadTrackingSettings.Instance;

            MyGuiControlLabel caption = AddCaption(text: "Head Tracking Settings", captionOffset: new Vector2(0, 0.003f));

            float sepWidth = Size.Value.X * 0.8f;
            var index1 = 1;
            var index2 = 1;
            var baseY = caption.Position.Y;
            var column1X = -Size.Value.X * 0.2f;
            var column2X = Size.Value.X * 0.1f;

            // Первая колонка: Основные настройки
            var separator = new MyGuiControlSeparatorList();
            separator.AddHorizontal(new Vector2(column1X - sepWidth / 4, baseY + index1 * VerticalDelta), sepWidth / 2);
            Controls.Add(separator);
            index1++;

            var enabledCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Enable/disable head tracking",
                    isChecked: settings.Enabled,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            enabledCheckbox.IsCheckedChanged += (box) => { settings.Enabled = box.IsChecked; };
            Controls.Add(enabledCheckbox);
            AddLabel(enabledCheckbox, "Head Tracking Enabled");
            index1++;

            var inCharacterCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Enable/disable head tracking when controlling character.",
                    isChecked: settings.EnabledInCharacter,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            inCharacterCheckbox.IsCheckedChanged += (box) => { settings.EnabledInCharacter = box.IsChecked; };
            Controls.Add(inCharacterCheckbox);
            AddLabel(inCharacterCheckbox, "Head Tracking In Character");
            index1++;

            var inFpsCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Enable/disable head tracking when controlling character in first person.",
                    isChecked: settings.EnabledInFirstPerson,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            inFpsCheckbox.IsCheckedChanged += (box) => { settings.EnabledInFirstPerson = box.IsChecked; };
            Controls.Add(inFpsCheckbox);
            AddLabel(inFpsCheckbox, "Head Tracking In FPS", offsetX: HorizontalSpace);
            index1++;

            // Первая колонка: Настройки вращения
            var rotationSeparator = new MyGuiControlSeparatorList();
            rotationSeparator.AddHorizontal(new Vector2(column1X - sepWidth / 4, baseY + index1 * VerticalDelta), sepWidth / 2);
            Controls.Add(rotationSeparator);
            index1++;

            var rotationLabel = new MyGuiControlLabel(
                position: new Vector2(column1X, baseY + index1 * VerticalDelta),
                text: "Rotation Settings",
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
            Controls.Add(rotationLabel);
            index1++;

            var invertPitchCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Invert pitch axis.",
                    isChecked: settings.InvertPitch,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertPitchCheckbox.IsCheckedChanged += (box) => { settings.InvertPitch = box.IsChecked; };
            Controls.Add(invertPitchCheckbox);
            AddLabel(invertPitchCheckbox, "Invert Pitch");
            index1++;

            var invertYawCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Invert yaw axis.",
                    isChecked: settings.InvertYaw,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertYawCheckbox.IsCheckedChanged += (box) => { settings.InvertYaw = box.IsChecked; };
            Controls.Add(invertYawCheckbox);
            AddLabel(invertYawCheckbox, "Invert Yaw");
            index1++;

            var invertRollCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column1X + HorizontalSpace, baseY + index1 * VerticalDelta),
                    toolTip: "Invert roll axis.",
                    isChecked: settings.InvertRoll,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertRollCheckbox.IsCheckedChanged += (box) => { settings.InvertRoll = box.IsChecked; };
            Controls.Add(invertRollCheckbox);
            AddLabel(invertRollCheckbox, "Invert Roll");
            index1++;

            // Вторая колонка: Настройки позиционного трекинга
            var positionalSeparator = new MyGuiControlSeparatorList();
            positionalSeparator.AddHorizontal(new Vector2(column2X - sepWidth / 4, baseY + index2 * VerticalDelta), sepWidth / 2);
            Controls.Add(positionalSeparator);
            index2++;

            var positionalLabel = new MyGuiControlLabel(
                position: new Vector2(column2X, baseY + index2 * VerticalDelta),
                text: "Positional Tracking",
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
            Controls.Add(positionalLabel);
            index2++;

            var enablePositionalCheckbox = new MyGuiControlCheckbox(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                toolTip: "Enable/disable positional head tracking",
                isChecked: settings.EnablePositionalTracking,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            enablePositionalCheckbox.IsCheckedChanged += (box) => { settings.EnablePositionalTracking = box.IsChecked; };
            Controls.Add(enablePositionalCheckbox);
            AddLabel(enablePositionalCheckbox, "Positional Tracking Enabled");
            index2++;

            var positionScaleSlider = new MyGuiControlSlider(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                width: 0.2f,
                minValue: 0.001f,
                maxValue: 0.1f,
                labelText: "Position Scale:",
                labelDecimalPlaces: 3,
                labelSpaceWidth: 0.1f,
                defaultValue: settings.PositionScale,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            positionScaleSlider.ValueChanged += (slider) => { settings.PositionScale = slider.Value; };
            Controls.Add(positionScaleSlider);
            index2++;

            var invertXCheckbox = new MyGuiControlCheckbox(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                toolTip: "Invert X axis movement",
                isChecked: settings.InvertX,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertXCheckbox.IsCheckedChanged += (box) => { settings.InvertX = box.IsChecked; };
            Controls.Add(invertXCheckbox);
            AddLabel(invertXCheckbox, "Invert X Axis");
            index2++;

            var invertYCheckbox = new MyGuiControlCheckbox(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                toolTip: "Invert Y axis movement",
                isChecked: settings.InvertY,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertYCheckbox.IsCheckedChanged += (box) => { settings.InvertY = box.IsChecked; };
            Controls.Add(invertYCheckbox);
            AddLabel(invertYCheckbox, "Invert Y Axis");
            index2++;

            var invertZCheckbox = new MyGuiControlCheckbox(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                toolTip: "Invert Z axis movement",
                isChecked: settings.InvertZ,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertZCheckbox.IsCheckedChanged += (box) => { settings.InvertZ = box.IsChecked; };
            Controls.Add(invertZCheckbox);
            AddLabel(invertZCheckbox, "Invert Z Axis");
            index2++;

            // Добавляем новый чекбокс для UseNewPositionMethod
            var positionMethodCheckbox = new MyGuiControlCheckbox(
                position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                toolTip: "Use new position method (accounts for head rotation but may result in  wacky behavior)",
                isChecked: settings.UseNewPositionMethod,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            positionMethodCheckbox.IsCheckedChanged += (box) => { settings.UseNewPositionMethod = box.IsChecked; };
            Controls.Add(positionMethodCheckbox);
            AddLabel(positionMethodCheckbox, "Advanced Position Tracking");
            index2++;

            // Вторая колонка: Тестовый режим
            var testModeSeparator = new MyGuiControlSeparatorList();
            testModeSeparator.AddHorizontal(new Vector2(column2X - sepWidth / 4, baseY + index2 * VerticalDelta), sepWidth / 2);
            Controls.Add(testModeSeparator);
            index2++;

            var testModeCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(column2X + HorizontalSpace, baseY + index2 * VerticalDelta),
                    toolTip: "Test mode simulates head tracking.",
                    isChecked: settings.TestModeEnabled,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            testModeCheckbox.IsCheckedChanged += (box) => { settings.TestModeEnabled = box.IsChecked; };
            Controls.Add(testModeCheckbox);
            AddLabel(testModeCheckbox, "Test Mode Enabled");
            index2++;

            // Кнопки OK/Cancel
            var maxIndex = Math.Max(index1, index2);
            var buttonSeparator = new MyGuiControlSeparatorList();
            buttonSeparator.AddHorizontal(new Vector2(-sepWidth / 2, baseY + maxIndex * VerticalDelta), sepWidth);
            Controls.Add(buttonSeparator);
            maxIndex++;

            var okButton =
                new MyGuiControlButton(
                    position: new Vector2(-0.05f, baseY + maxIndex * VerticalDelta),
                    text: MyTexts.Get(MyCommonTexts.Ok),
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
                    onButtonClick: (_) => {
                        HeadTrackingSettings.Instance.Save();
                        CloseScreen();
                    });
            Controls.Add(okButton);

            var cancelButton =
                new MyGuiControlButton(
                    position: new Vector2(0.05f, baseY + maxIndex * VerticalDelta),
                    text: MyTexts.Get(MyCommonTexts.Cancel),
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                    onButtonClick: (_) => {
                        CloseScreen();
                    });
            Controls.Add(cancelButton);
        }

        private void AddLabel(MyGuiControlBase control, string text, float offsetX = 0.0f)
        {
            Controls.Add(new MyGuiControlLabel(
                position: control.Position + new Vector2(LabelOffsetX + offsetX, control.Size.Y / 2),
                text: text,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
        }

        protected override void OnClosed()
        {
            HeadTrackingSettings.Reload();
        }
    }
}