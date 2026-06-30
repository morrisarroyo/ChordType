using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    public class TypeWindowUIView
    {
        private Label _typeWindowLabel;
        private Image _textCursorImage;
    
        private float _baseFontSize;
        private float _characterWidthRatio;
        private float _characterStartingOffsetLeft = 0.0f;

        public TypeWindowUIView(VisualElement root)
        {
            // Find the Label by name
            _typeWindowLabel = root.Q<Label>("type-window-label");
            _textCursorImage = root.Q<Image>("text-cursor-image");
            _textCursorImage.RegisterCallback<GeometryChangedEvent>(OnTextCursorImageFirstLayout);
        }

        public void Initialize(float baseFontSize, float characterWidthRatio)
        {
            _baseFontSize = baseFontSize;
            _characterWidthRatio = characterWidthRatio;
        }

        public void SetTypeWindowText(string newString)
        {
            _typeWindowLabel.text = RichTextUtility.WrapInMonospaceTag(newString, 
                (_typeWindowLabel.style.fontSize.value.value * _characterWidthRatio) + "px");
        }

        public void ClearTypeWindow()
        {
            _typeWindowLabel.text = "";
            _typeWindowLabel.MarkDirtyRepaint();

            SetCursorVisibility(false);
        }
    
        public void ChangeFocusToTypeWindow(bool hasFocus)
        {
            Debug.Log(hasFocus ? "FOCUSED" : "UNFOCUSED");
            _typeWindowLabel.Focus();
        }
    
        // Used to capture starting offset put in place in the UI Builder
        private void OnTextCursorImageFirstLayout(GeometryChangedEvent layoutEvent)
        {
            _textCursorImage.UnregisterCallback<GeometryChangedEvent>(OnTextCursorImageFirstLayout);
            
            _characterStartingOffsetLeft = _textCursorImage.layout.position.x;
            Debug.Log($"Prompt UI View character starting offset left: {_characterStartingOffsetLeft}");
        
            AnimateLoopBlinkingCursor();
        }

        private void AnimateLoopBlinkingCursor()
        {
            // or back to 1.0 when it's removed.
            _textCursorImage.RegisterCallback<TransitionEndEvent>(evt => _textCursorImage.ToggleInClassList("cursor-hidden"));
            // Schedule the first transition 100 milliseconds after the root.schedule.Execute method is called.
            _textCursorImage.schedule.Execute(() => _textCursorImage.ToggleInClassList("cursor-hidden")).StartingIn(100);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            _textCursorImage.visible = isVisible;
        }
        
        public void UpdateCursorPosition(int typedCharactersCount)
        {
            // X location of the Type Window - the cursor's width (offset)
            _textCursorImage.style.left = _characterStartingOffsetLeft 
                                          + ((_typeWindowLabel.style.fontSize.value.value * _characterWidthRatio) * typedCharactersCount);
        }
    
        public void UpdateTextSize(int newPercent)
        {
            _typeWindowLabel.style.fontSize = _baseFontSize * (newPercent / 100.0f);
        }
    }
}
