using System;
using System.Collections.Generic;
using Game.Systems.Encounter;
using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    public class PromptCounterSelectorPopup
    {
        private readonly VisualElement _selectorPopup;
        
        private readonly Dictionary<EncounterPromptCount, TemplateContainer> _entries = new();
        private readonly Dictionary<EncounterPromptCount, Action> _callbacks = new();
        
        private readonly Button _cancelButton;

        public Action<EncounterPromptCount> OnPromptCountButtonClicked;

        
        public PromptCounterSelectorPopup(VisualElement root, VisualTreeAsset entryTemplate)
        {
            _selectorPopup = root.Q<VisualElement>("prompt-count-selector-popup");
            VisualElement entriesContainer = _selectorPopup.Q<VisualElement>("prompt-count-entries-container");
            _cancelButton = _selectorPopup.Q<Button>("prompt-count-cancel-button");

            entriesContainer.Clear();
            foreach (EncounterPromptCount count in Enum.GetValues(typeof(EncounterPromptCount)))
            {
                var entry = entryTemplate.Instantiate();
                Button countButton = entry.Q<Button>("prompt-count-button");
                countButton.text = $"{(int)count}";
                _entries.Add(count, entry);
                entriesContainer.Add(entry);
            }
            
            BindButtons();
        }

        private void BindButtons()
        {
           _cancelButton.clicked += OnPromptCountSelectCancel;

           foreach (KeyValuePair<EncounterPromptCount, TemplateContainer> promptSelection in _entries)
           {
               Button countButton = promptSelection.Value.Q<Button>("prompt-count-button");
               Action countButtonCallback = () => OnPromptCountButtonClick(promptSelection.Key);
               countButton.clicked += countButtonCallback;
               _callbacks.Add(promptSelection.Key, countButtonCallback);
           }
        }

        private void OnPromptCountButtonClick(EncounterPromptCount count)
        {
            OnPromptCountButtonClicked?.Invoke(count);
        }

        public void UnbindButtons()
        {
            _cancelButton.clicked -= OnPromptCountSelectCancel;
            foreach (KeyValuePair<EncounterPromptCount, TemplateContainer> promptSelection in _entries)
            {
                Button countButton = promptSelection.Value.Q<Button>("prompt-count-button");
                countButton.clicked -= _callbacks[promptSelection.Key];
                _callbacks.Remove(promptSelection.Key);
            }
        }
        
        public void ShowPromptCountSelectorPopup()
        {
            _selectorPopup.style.display = DisplayStyle.Flex;
        }
        
        public void HidePromptCountSelectorPopup()
        {
            _selectorPopup.style.display = DisplayStyle.None;
        }
        
        private void OnPromptCountSelectCancel()
        {
            HidePromptCountSelectorPopup();
        }
    }
}
