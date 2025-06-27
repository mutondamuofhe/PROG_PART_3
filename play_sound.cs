using System;
using System.IO;
using System.Media;

namespace PROG_PART_3
{
    internal class play_sound
    {
        public play_sound()
        {
            try
            {
                // Get the base directory of the application
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                // Combine it with the audio file name
                string fullPath = Path.Combine(baseDir, "voice_greeting.wav");

                // Play the audio file
                using (SoundPlayer player = new SoundPlayer(fullPath))
                {
                    player.Play(); // Non-blocking for GUI apps
                }
            }
            catch (Exception ex)
            {
                // You can log or handle the error as needed
                Console.WriteLine("Error playing sound: " + ex.Message);
            }
        }
    }
}
