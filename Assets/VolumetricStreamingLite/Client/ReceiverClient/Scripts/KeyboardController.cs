//
// This is a modified version of the KeyboardTest.cs from MixedRealityToolkit-Unity.
//
// The original source code is available in GitHub.
// https://github.com/microsoft/MixedRealityToolkit-Unity/blob/releases/2.3.0/Assets/MixedRealityToolkit.Examples/Experimental/NonNativeKeyboard/Scripts/KeyboardTest.cs
//

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    /// <summary>
    /// This component links the NonNativeKeyboard to a TMP_InputField
    /// Put it on the TMP_InputField and assign the NonNativeKeyboard.prefab
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class KeyboardController : MonoBehaviour, IPointerDownHandler
    {
        [Microsoft.MixedReality.Toolkit.Experimental]
        [SerializeField] private NonNativeKeyboard keyboard = null;

        private TMP_InputField inputField;

        public void OnPointerDown(PointerEventData eventData)
        {
            inputField = GetComponent<TMP_InputField>();

            keyboard.Close();
            keyboard.PresentKeyboard();

            keyboard.OnClosed += DisableKeyboard;
            keyboard.OnTextSubmitted += DisableKeyboard;
            keyboard.OnTextUpdated += UpdateText;
        }

        private void UpdateText(string text)
        {
            inputField.text = text;
        }

        private void DisableKeyboard(object sender, EventArgs e)
        {
            keyboard.OnTextUpdated -= UpdateText;
            keyboard.OnClosed -= DisableKeyboard;
            keyboard.OnTextSubmitted -= DisableKeyboard;

            keyboard.Close();
        }
    }
}