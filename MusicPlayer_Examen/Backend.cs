using WMPLib;

namespace ConsoleMusicPlayer
{
    internal class Backend
    {
        private WindowsMediaPlayer _player;
        private Frontend _frontend;

        public Backend(WindowsMediaPlayer player, Frontend frontend)
        {
            _player = player;
            _frontend = frontend;
        }

        public bool HandleChoice(UserChoice userChoice)
        {
            bool exitApplication = false;
            switch (userChoice)
            {
                case UserChoice.TogglePlayPause:
                    {
                        if (_player.playState == WMPPlayState.wmppsPlaying)
                        {
                            PauseSong();
                        }
                        else
                        {
                            PlaySong();
                        };
                        break;
                    }

                case UserChoice.ChangeVolume: { _player.settings.volume = ChangeVolume(); break; }
                case UserChoice.ToggleMute: { ToggleMute(); break; }
                case UserChoice.SpeedUpSong: { FastForward(); break; }
                case UserChoice.SlowDownSong: { SlowDown(); break; }
                case UserChoice.PlayNewSong: { _player.URL = _frontend.GetUserFile(); break; }
                case UserChoice.StopPlayingCurrentSong: { StopSong(); break; }
                case UserChoice.StopApplication: { exitApplication = true; break; }
            }
            return exitApplication;
        }

        private void PauseSong()
        {
            _player.controls.pause();
        }

        private void PlaySong()
        {
            _player.controls.play();
        }

        private void StopSong()
        {
            _player.controls.stop();
        }

        private int ChangeVolume()
        {
            Console.WriteLine("Give new volume level: ");
            int volume;
            if (!Int32.TryParse(Console.ReadLine(), out volume))
            {
                _frontend.PrintStringColor("Input was not a number, please give a number between 0 - 100");

                return ChangeVolume();
            }
            if (volume < 0 || volume > 100)
            {
                _frontend.PrintStringColor("Input was not between 0-100, please give a number between 0-100");

                return ChangeVolume();
            }
            return volume;
        }

        private void ToggleMute()
        {
            _player.settings.mute ^= true;
        }

        private void FastForward()
        {
            _player.settings.rate += 0.25;
        }

        private void SlowDown()
        {
            if (_player.settings.rate > 0.25)
            {
                _player.settings.rate -= 0.25;
            }
            else
            {
                Thread.Sleep(2000);
                _frontend.PrintStringColor("Can't slow down more");
            }
        }
    }
}