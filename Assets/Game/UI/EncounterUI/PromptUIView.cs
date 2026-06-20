using System;
using Game.Core.GameManager;
using Game.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.EncounterUI
{
    public class PromptUIView
    {
        private Label _historyPromptWindowLabel;
        private Label _currentPromptWindowLabel;
        
        private float _characterWidth;

        public PromptUIView(VisualElement root)
        {
            // Find the Label by name
            _historyPromptWindowLabel = root.Q<Label>("history-prompt-window-label");
            _currentPromptWindowLabel = root.Q<Label>("prompt-window-label");
        }

        public void Initialize(float characterWidth)
        {
            _characterWidth = characterWidth;
        }

        public void UpdateHistoryPromptWindow()
        {
            _historyPromptWindowLabel.text = _currentPromptWindowLabel.text;
        }
        
        public void UpdatePromptWindowText(string newPromptWindowText)
        {
            _currentPromptWindowLabel.text = RichTextUtility.WrapInMonospaceTag(newPromptWindowText, _characterWidth + "px");
        }
        
        public void ResetCurrentPrompt()
        {
            // Remove all html tags from Prompt Window Label
            _currentPromptWindowLabel.text = RichTextUtility.StripTags(_currentPromptWindowLabel.text);
            // Add back monospace tags
            _currentPromptWindowLabel.text = RichTextUtility.WrapInMonospaceTag(_currentPromptWindowLabel.text, _characterWidth + "px");
            _currentPromptWindowLabel.MarkDirtyRepaint();
        }

        public void ColourCharacterInPrompt(int characterIndex, Color newColour)
        {
            _currentPromptWindowLabel.text = RichTextUtility.SetColorOfVisibleChar(_currentPromptWindowLabel.text, characterIndex,$"#{ColorUtility.ToHtmlStringRGB(newColour)}");
        }
    }
}
