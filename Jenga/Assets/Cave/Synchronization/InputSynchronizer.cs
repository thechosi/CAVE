using System.Collections.Generic;
using UnityEngine;
using AwesomeSockets.Buffers;

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
        MouseDown
    }

    public struct StoredButton
    {
        public short eventType;
        public int id;
    }

    public class InputInputMessage : ISynchroMessage
    {
        public List<StoredAxis> axes = new List<StoredAxis>();
        public List<StoredButton> buttons = new List<StoredButton>();

        public void Serialize(Buffer buffer)
        {
            Buffer.Add(buffer, axes.Count);
            foreach (StoredAxis axis in axes)
            {
                Buffer.Add(buffer, axis.id);
                Buffer.Add(buffer, axis.value);
            }

            Buffer.Add(buffer, buttons.Count);
            foreach (StoredButton button in buttons)
            {
                Buffer.Add(buffer, button.id);
                Buffer.Add(buffer, button.eventType);
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
                StoredButton button = new StoredButton();
                button.id = Buffer.Get<int>(buffer);
                button.eventType = Buffer.Get<short>(buffer);
                buttons.Add(button);
            }
        }

        public int GetLength()
        {
            return (sizeof(float) + sizeof(int)) * axes.Count + (sizeof(short) + sizeof(int)) * buttons.Count + sizeof(int) * 2;
        }
    }

    class InputSynchronizer
    {
        public static string[] relevantAxes = { "Vertical", "Horizontal" };
        public static string[] relevantButtons = { };

        private static Dictionary<string, float> axes = new Dictionary<string, float>();
        private static Dictionary<string, bool> buttonsPressed = new Dictionary<string, bool>();
        private static Dictionary<int, bool> mouseButtonsPressed = new Dictionary<int, bool>();
        
        public static void ProcessMessage(InputInputMessage message)
        {
            foreach (StoredAxis axis in message.axes)
            {
                axes[relevantAxes[axis.id]] = axis.value;
            }

            buttonsPressed.Clear();
            mouseButtonsPressed.Clear();
            foreach (StoredButton button in message.buttons)
            {
                if (button.eventType == (short)InputType.ButtonDown)
                    buttonsPressed[relevantButtons[button.id]] = true;
                else if (button.eventType == (short)InputType.MouseDown)
                    mouseButtonsPressed[button.id] = true;
            }
        }

        public static void BuildMessage(InputInputMessage message)
        {
            for (int i = 0; i < relevantAxes.Length; i++)
            {
                StoredAxis storedAxis = new StoredAxis();
                storedAxis.id = i;
                storedAxis.value = Input.GetAxis(relevantAxes[i]);
                message.axes.Add(storedAxis);
            }

            for (int i = 0; i < relevantButtons.Length; i++)
            {
                if (Input.GetButton(relevantButtons[i]))
                {
                    StoredButton storedButton = new StoredButton();
                    storedButton.id = i;
                    storedButton.eventType = (short)InputType.ButtonDown;
                    message.buttons.Add(storedButton);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButton(i))
                {
                    StoredButton storedButton = new StoredButton();
                    storedButton.id = i;
                    storedButton.eventType = (short)InputType.MouseDown;
                    message.buttons.Add(storedButton);
                }
            }
        }
        

        public static float GetAxis(string axis)
        {
            if (axes.ContainsKey(axis))
                return axes[axis];
            else
                return 0;
        }

        public static bool GetButton(string button)
        {
            if (buttonsPressed.ContainsKey(button))
                return buttonsPressed[button];
            else
                return false;
        }

        public static bool GetMouseButton(int button)
        {
            if (mouseButtonsPressed.ContainsKey(button))
                return mouseButtonsPressed[button];
            else
                return false;
        }

    }
}
