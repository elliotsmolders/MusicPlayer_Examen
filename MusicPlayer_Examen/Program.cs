// See https://aka.ms/new-console-template for more information
using ConsoleMusicPlayer;
using WMPLib;

WindowsMediaPlayer player = new WindowsMediaPlayer();
AsciiArt ascii = new AsciiArt();
Frontend frontend = new Frontend(player, ascii);
Backend backend = new Backend(player, frontend);

string filename = frontend.GetUserFile();
player.URL = filename;

bool exitProgram = false;

while (!exitProgram)
{
    frontend.PrintFrontend();
    int userChoice = frontend.GetUserChoice();
    exitProgram = backend.HandleChoice((UserChoice)userChoice);
}