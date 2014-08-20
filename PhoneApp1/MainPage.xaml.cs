using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
       private SoundEffectInstance previousSoundEffect = null;
       private SoundEffectInstance sonicInstance = null;
       private PhotoCamera camera = new PhotoCamera();
       private 
       bool screwdriverActive = false;
       bool cameraReady = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            camera.AutoFocusCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_AutoFocusCompleted);
            viewfinderBrush.SetSource(camera);
            camera.Initialized += PhotoCamera_Initialized;
        }

        private void PhotoCamera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            cameraReady = true;
        }

        private void ExterminateButton_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/exterminate.wav");
        }       

        private void TardisButton_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/tardis.wav");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/delete.wav");
        }

        private void K9Button_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/k9_affirmative.wav");
        }

        private void DalekChorusButton_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/dalekchorus1a.wav");
        }

        private void WibblyWobblyButton_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Sounds/wibblywobbly.wav");
        }

        private void PlaySound(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    if (stream != null)
                    {
                        if (previousSoundEffect != null)
                        {
                            previousSoundEffect.Stop();
                        }
                        var effect = SoundEffect.FromStream(stream);
                        previousSoundEffect = effect.CreateInstance();
                        FrameworkDispatcher.Update();
                        previousSoundEffect.Play();
                    }
                }
            }
        }

        private void ScrewdriverButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            screwdriverActive = true;
            if (cameraReady)
            {
                if (camera.IsFlashModeSupported(FlashMode.On))
                {
                    camera.FlashMode = FlashMode.On;
                    camera.Focus();
                }
            }
            if (previousSoundEffect != null)
            {
                previousSoundEffect.Stop();
            }
            using (var stream = TitleContainer.OpenStream("Sounds/sonicscrewdriver.wav"))
            {
                if (stream != null)
                {
                    var effect = SoundEffect.FromStream(stream);
                    sonicInstance = effect.CreateInstance();
                    FrameworkDispatcher.Update();
                    sonicInstance.IsLooped = true;
                    sonicInstance.Play();
                }
            }
        }

        private void ScrewdriverButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            screwdriverActive = false;
            if (cameraReady)
            {
                if (camera.IsFlashModeSupported(FlashMode.Off))
                {
                    camera.FlashMode = FlashMode.Off;
                }
            }
            if (sonicInstance != null)
            {
                sonicInstance.Stop();
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //Stop all sounds
            if (sonicInstance != null)
            {
                sonicInstance.Stop();
            }
            if (previousSoundEffect != null)
            {
                previousSoundEffect.Stop();
            }
            if (cameraReady)
            {
                if (camera.IsFlashModeSupported(FlashMode.Off))
                {
                    camera.FlashMode = FlashMode.Off;
                }
            }
        }

        private void camera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            if (cameraReady)
            {
                if (screwdriverActive == true)
                {
                    camera.Focus();
                }
            }
        }

    }
}