using WMPLib;

namespace ConsoleMusicPlayer
{
    public class Frontend
    {
        private WindowsMediaPlayer _player;
        private AsciiArt _ascii;

        public Frontend(WindowsMediaPlayer player, AsciiArt ascii)
        {
            _player = player;
            _ascii = ascii;
        }

        public void PrintFrontend()
        {
            Console.Clear();

            PrintStringColor(GetTitle(), ConsoleColor.Blue);
            Console.WriteLine(GetNewLines(3));
            PrintMenu();
            Thread.Sleep(100);
            /* app breaks without this, Unhandled exception. System.Runtime.InteropServices.COMException (0x8001010A)
            : The message filter indicated that the application is busy.*/
            Console.WriteLine(GetNewLines(3));
            printInfo();
        }

        private void printInfo()
        {
            Console.WriteLine(GetPlayerState());
            PrintVolumeState();
            Console.WriteLine(GetVolumeBar());
            PrintMetadata();
            PrintPlaySpeed();
        }

        private void PrintMetadata()
        {
            Console.WriteLine(GetSongName());
            Console.WriteLine(GetDuration());
            Console.WriteLine(GetArtist());
        }

        private void PrintMenu()
        {
            Console.WriteLine("1. Play/Pause");
            Console.WriteLine("2. Change Volume");
            Console.WriteLine("3. Mute/Unmute");
            Console.WriteLine("4. Speed up song");
            Console.WriteLine("5. Slow down song");
            Console.WriteLine("6. Play new song");
            Console.WriteLine("7. Stop");
            Console.WriteLine("8. Quit");
        }

        private string GetNewLines(int lines)
        {
            return string.Join("", Enumerable.Repeat("\n", lines - 1));
        }

        private string GetTitle()
        {
            return _ascii.title;
        }

        public string GetUserFile()
        {
            Console.WriteLine("Geef bestand om af te spelen:");
            string filename = Console.ReadLine();
            if (!File.Exists(filename))
            {
                PrintStringColor("Het ingegeven bestand bestaat niet.");
                return GetUserFile();
            }
            return filename;
        }

        public int GetUserChoice()
        {
            Console.WriteLine("Choose an option:");
            int userChoice;
            bool validChoice = Int32.TryParse(Console.ReadLine(), out userChoice);
            if (!validChoice)
            {
                PrintStringColor("Input was not a number, please enter a number between 1-8");
                return GetUserChoice();
            }
            if (userChoice < 1 || userChoice > 8)
            {
                PrintStringColor("Input was not between 1-8, please enter a number between 1-8");
                return GetUserChoice();
            }
            return userChoice;
        }

        private void PrintVolumeState()
        {
            int volumeState = _player.settings.volume;
            if (_player.settings.mute)
            {
                PrintStringColor("muted", ConsoleColor.Red);
            }
            Console.WriteLine($"Volume = {volumeState} %");
        }

        private string GetVolumeBar()
        {
            int volume = _player.settings.volume;
            string volumebar = $"[{new string('*', volume / 5)}{new string(' ', 20 - volume / 5)}]";
            return volumebar;
        }

        private string GetPlayerState() //fit into enum, was ok
        {
            return $"State: {(PlayerStates)_player.playState}";
        }

        private void PrintPlaySpeed()
        {
            Console.Write($"playback speed = ");
            if (_player.settings.rate > 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (_player.settings.rate < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"{_player.settings.rate}x");
            Console.ResetColor();
        }

        private string GetDuration()
        {
            return $"song length: {TimeSpan.FromSeconds(_player.currentMedia.duration).ToString(@"m\:ss")}";
        }

        private string GetSongName()
        {
            return $"song name: {_player.currentMedia.name}";
        }

        private string GetArtist()
        {
            return $"Artist: {_player.currentMedia.getItemInfo("Artist")}";
        }

        public void PrintStringColor(string message, ConsoleColor color = ConsoleColor.DarkRed)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}