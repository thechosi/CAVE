using System;
using System.Collections.Generic;
using UnityEngine;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace Cave
{
    public struct StoredAxis
    {
        public int id;
        public float value;
    }
    
    enum InputType
    {
        ButtonDown,
        KeyDown
    }

    public struct StoredInput
    {
        public short inputType;
        public int id;
    }

    public class InputInputMessage : ISynchroMessage
    {
        public List<StoredAxis> axes = new List<StoredAxis>();
        public List<StoredInput> buttons = new List<StoredInput>();

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, axes.Count);
            foreach (StoredAxis axis in axes)
            {
                Buffer.Add(buffer, axis.id);
                Buffer.Add(buffer, axis.value);
            }

            Buffer.Add(buffer, buttons.Count);
            foreach (StoredInput button in buttons)
            {
                Buffer.Add(buffer, button.id);
                Buffer.Add(buffer, button.inputType);
            }
        }

        public void Deserialize(Buffer buffer)
        {
            int length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredAxis axis = new StoredAxis();
                axis.id = Buffer.Get<int>(buffer);
                axis.value = Buffer.Get<float>(buffer);
                axes.Add(axis);
            }

            length = Buffer.Get<int>(buffer);
            for (int i = 0; i < length; i++)
            {
                StoredInput input = new StoredInput();
                input.id = Buffer.Get<int>(buffer);
                input.inputType = Buffer.Get<short>(buffer);
                buttons.Add(input);
            }
        }

        public int GetLength()
        {
            return (sizeof(float) + sizeof(int)) * axes.Count + (sizeof(short) + sizeof(int)) * buttons.Count + sizeof(int) * 2;
        }
    }

    class InputSynchronizer
    {

        private static Dictionary<string, float> axes = new Dictionary<string, float>();
        private static Dictionary<string, bool> buttonsPressed = new Dictionary<string, bool>();
        private static Dictionary<string, bool> buttonsPressedLastFrame = new Dictionary<string, bool>();
        private static Dictionary<string, bool> keysPressed = new Dictionary<string, bool>();
        private static Dictionary<string, bool> keysPressedLastFrame = new Dictionary<string, bool>();

        public static void ProcessMessage(Synchronizer synchronizer, InputInputMessage message)
        {
            foreach (StoredAxis axis in message.axes)
            {
                axes[synchronizer.relevantAxes[axis.id]] = axis.value;
            }

            var tempButtons = buttonsPressedLastFrame;
            buttonsPressedLastFrame = buttonsPressed;
            buttonsPressed = tempButtons;
            
            var tempKeys = keysPressedLastFrame;
            keysPressedLastFrame = keysPressed;
            keysPressed = tempKeys;

            buttonsPressed.Clear();
            keysPressed.Clear();
            foreach (StoredInput button in message.buttons)
            {
                if (button.inputType == (short)InputType.ButtonDown)
                    buttonsPressed[synchronizer.relevantButtons[button.id]] = true;
                else
                    keysPressed[synchronizer.relevantKeys[button.id]] = true;
            }
        }

        private static bool isKeyPressed(Synchronizer synchronizer, string name)
        {
            if (name.StartsWith("flystick "))
            {
                int num = Int32.Parse(name.Substring("flystick ".Length));
                return synchronizer.flyStick != null && synchronizer.flyStick.GetComponent<TrackerSettings>().IsButtonPressed(num);
            }
            else
            {
                return Input.GetKey(name);
            }
        }

        public static void BuildMessage(Synchronizer synchronizer, InputInputMessage message)
        {
            for (int i = 0; i < synchronizer.relevantAxes.Length; i++)
            {
                StoredAxis storedAxis = new StoredAxis();
                storedAxis.id = i;
				if (synchronizer.relevantAxes [i] == "flystick horizontal")
					storedAxis.value = synchronizer.flyStick.GetComponent<TrackerSettings> ().getAnalog ().x;
				else if (synchronizer.relevantAxes[i] == "flystick vertical")
					storedAxis.value = synchronizer.flyStick.GetComponent<TrackerSettings> ().getAnalog ().y;
				else
                	storedAxis.value = Input.GetAxis(synchronizer.relevantAxes[i]);
                message.axes.Add(storedAxis);
            }

            for (int i = 0; i < synchronizer.relevantButtons.Length; i++)
            {
                if (Input.GetButton(synchronizer.relevantButtons[i]))
                {
                    StoredInput storedInput = new StoredInput();
                    storedInput.id = i;
                    storedInput.inputType = (short)InputType.ButtonDown;
                    message.buttons.Add(storedInput);
                }
            }

            for (int i = 0; i < synchronizer.relevantKeys.Length; i++)
            {
                if (isKeyPressed(synchronizer, synchronizer.relevantKeys[i]))
                {
                    StoredInput storedInput = new StoredInput();
                    storedInput.id = i;
                    storedInput.inputType = (short)InputType.KeyDown;
                    message.buttons.Add(storedInput);
                }
            }

            ProcessMessage(synchronizer, message);
        }
        
        public static float GetAxis(string axis)
        {
            if (axes.ContainsKey(axis))
                return axes[axis];
            else
                return 0;
        }

        private static bool GetButtonLastFrame(string button)
        {
            if (buttonsPressedLastFrame.ContainsKey(button))
                return buttonsPressedLastFrame[button];
            else
                return false;
        }

        public static bool GetButton(string button)
        {
            if (buttonsPressed.ContainsKey(button))
                return buttonsPressed[button];
            else
                return false;
        }

        public static bool GetButtonDown(string button)
        {
            return GetButton(button) && !GetButtonLastFrame(button);
        }

        public static bool GetButtonUp(string button)
        {
            return !GetButton(button) && GetButtonLastFrame(button);
        }

        
        private static bool GetKeyLastFrame(string button)
        {
            if (keysPressedLastFrame.ContainsKey(button))
                return keysPressedLastFrame[button];
            else
                return false;
        }

        public static bool GetKey(string button)
        {
            if (keysPressed.ContainsKey(button))
                return keysPressed[button];
            else
                return false;
        }

        public static bool GetKeyDown(string button)
        {
            return GetKey(button) && !GetKeyLastFrame(button);
        }

        public static bool GetKeyUp(string button)
        {
            return !GetKey(button) && GetKeyLastFrame(button);
        }

        public static bool GetMouseButton(int button)
        {
            return GetKey("mouse " + button);
        }

        public static bool GetMouseButtonDown(int button)
        {
            return GetKeyDown("mouse " + button);
        }

        public static bool GetMouseButtonUp(int button)
        {
            return GetKeyUp("mouse " + button);
        }

        public static bool GetFlyStickButton(int buttonChannel)
        {
            return GetKey("flystick " + buttonChannel);
        }

        public static bool GetFlyStickButtonDown(int buttonChannel)
        {
            return GetKeyDown("flystick " + buttonChannel);
        }

        public static bool GetFlyStickButtonUp(int buttonChannel)
        {
            return GetKeyUp("flystick " + buttonChannel);
        }
    }
}
