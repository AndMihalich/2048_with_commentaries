using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace oop_template
{
    public class SoundEffectEngine
    {
        private readonly SoundPlayer _moveSound; // Player za zvuk pokreta
        private readonly SoundPlayer _mergeSound; // Player za zvuk spajanja

        public SoundEffectEngine()
        {
            // Učitava zvukove iz datoteka
            _moveSound = new SoundPlayer("whoosh.wav");
            _mergeSound = new SoundPlayer("pop.wav");
        }

        // Pustanje zvuka za pokret
        public void PlayMoveSound()
        {
            _moveSound.Play();
        }

        // Pustanje zvuka za spajanje
        public void PlayMergeSound()
        {
            _mergeSound.Play();
        }
    }
}