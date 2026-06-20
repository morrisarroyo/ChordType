using UnityEngine;
using UnityEngine.UIElements;

public class TypeWindowUIView
{
    private Label _typeWindowLabel;
    private Image _textCursorImage;
    
    private float _characterWidth;
    private float _characterStartingOffsetLeft = 0.0f;

    public TypeWindowUIView(VisualElement root)
    {
        // Find the Label by name
        _typeWindowLabel = root.Q<Label>("type-window-label");
        _textCursorImage = root.Q<Image>("text-cursor-image");
        _textCursorImage.RegisterCallback<GeometryChangedEvent>(OnTextCursorImageFirstLayout);
    }

    public void Initialize(float characterWidth)
    {
        _characterWidth = characterWidth;
    }

    public void SetTypeWindowText(string newString)
    {
        _typeWindowLabel.text = RichTextUtility.WrapInMonospaceTag(newString, _characterWidth + "px");
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
        // Old Calculations only works for Screen space
        /*
        // local position inside parent
        Vector2 localPos = _currentPromptWindowLabel.layout.position;
        Vector2 textGlobalPos = _currentPromptWindowLabel.contentRect.position + _currentPromptWindowLabel.worldBound.position;

                                      // X location of the Type Window - the cursor's width (offset)
        _textCursorImage.style.left = textGlobalPos.x + localPos.x - _textCursorImage.style.width.value.value
                                      + (_characterWidth * typedCharactersCount);
        _textCursorImage.style.top = textGlobalPos.y + localPos.y;
        */
            
        // local position inside parent
        // Vector2 localPos = _currentPromptWindowLabel.layout.position;
        // Vector2 textGlobalPos = _currentPromptWindowLabel.contentRect.position + _currentPromptWindowLabel.worldBound.position;

        // X location of the Type Window - the cursor's width (offset)
        _textCursorImage.style.left = _characterStartingOffsetLeft
                                      + (_characterWidth * typedCharactersCount);
        // _textCursorImage.style.top = textGlobalPos.y + localPos.y;
    }
}
