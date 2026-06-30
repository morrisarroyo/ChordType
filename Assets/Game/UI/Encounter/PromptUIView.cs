using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    public class PromptUIView
    {
        private Label _historyPromptWindowLabel;
        private Label _currentPromptWindowLabel;
        
        private float _baseFontSize;
        private float _characterWidthRatio;

        public PromptUIView(VisualElement root)
        {
            // Find the Label by name
            _historyPromptWindowLabel = root.Q<Label>("history-prompt-window-label");
            _currentPromptWindowLabel = root.Q<Label>("prompt-window-label");
        }

        public void Initialize(float baseFontSize, float characterWidthRatio)
        {
            _baseFontSize = baseFontSize;
            _characterWidthRatio = characterWidthRatio;
        }

        public void UpdateHistoryPromptWindow()
        {
            _historyPromptWindowLabel.text = _currentPromptWindowLabel.text;
        }
        
        public void UpdatePromptWindowText(string newPromptWindowText)
        {
            _currentPromptWindowLabel.text = RichTextUtility.WrapInMonospaceTag(newPromptWindowText, 
                (_currentPromptWindowLabel.style.fontSize.value.value * _characterWidthRatio) + "px");
        }
        
        public void ResetCurrentPrompt()
        {
            // Remove all html tags from Prompt Window Label
            _currentPromptWindowLabel.text = RichTextUtility.StripTags(_currentPromptWindowLabel.text);
            // Add back monospace tags
            _currentPromptWindowLabel.text = RichTextUtility.WrapInMonospaceTag(_currentPromptWindowLabel.text, 
                (_currentPromptWindowLabel.style.fontSize.value.value * _characterWidthRatio) + "px");
            _currentPromptWindowLabel.MarkDirtyRepaint();
        }

        public void ColourCharacterInPrompt(int characterIndex, Color newColour)
        {
            _currentPromptWindowLabel.text = RichTextUtility.SetColorOfVisibleChar(_currentPromptWindowLabel.text, characterIndex,$"#{ColorUtility.ToHtmlStringRGB(newColour)}");
        }
        
        public void UpdateTextSize(int newPercent)
        {
            _currentPromptWindowLabel.style.fontSize = _baseFontSize * (newPercent / 100.0f);
            _historyPromptWindowLabel.style.fontSize = _baseFontSize * (newPercent / 100.0f);
        }
    }
}
