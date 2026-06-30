using Game.Data.Save;
using UnityEngine;

namespace Game.Managers
{
    public class DisplayManager
    {
        private readonly SettingsData _settingsData;
        
        public DisplayManager(SettingsData settingsData)
        {
            _settingsData = settingsData;

            Bind();
            
            ChangeDisplayMode(_settingsData.IsFullscreen);
        }

        private void Bind()
        {
            _settingsData.OnFullscreenChanged += ChangeDisplayMode;
        }

        public void Unbind()
        {
            _settingsData.OnFullscreenChanged -= ChangeDisplayMode;
        }
        
        private void ChangeDisplayMode(bool isFullScreen)
        {
            if (isFullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(Screen.currentResolution.width,
                    Screen.currentResolution.height,
                    true);
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(1280, 720, false);
            }
        }
    }
}