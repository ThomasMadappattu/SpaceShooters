using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace SpaceShooter
{
    class SoundPlayer
    {
        public static AudioEngine audioEngine;
        public static WaveBank waveBank;
        public static SoundBank soundBank;
       
        public static void init()
        {
            audioEngine = new AudioEngine("TestSound.xgs");
            waveBank = new WaveBank(audioEngine, "Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Sound Bank.xsb");

        
        }
        
    }
}
