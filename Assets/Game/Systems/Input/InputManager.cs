using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Systems.Input
{
    public class InputManager : MonoBehaviour
    {
        public event Action<KeyUpEvent> OnKeyUpped;
        public event Action<PointerDownEvent> OnPointerDowned;
        
        public void OnKeyUp(KeyUpEvent keyUpEvent)
        {
            // ev.character is only filled when in InputText mode (UI Toolkit must expect typing)
            Debug.LogFormat("KeyUp: " + keyUpEvent.keyCode + " Character: " + keyUpEvent.character);
            OnKeyUpped?.Invoke(keyUpEvent);
        }

        public void OnUIPointerDown(PointerDownEvent pointerDownEvent)
        {
            Debug.Log("OnUIPointerDown at position " + pointerDownEvent.position);
            OnPointerDowned?.Invoke(pointerDownEvent);
        }
    }
}
