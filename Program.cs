// ./"Localisation Duplicate Finder.exe" "C:\SteamLibrary\steamapps\common\Europa Universalis IV\localisation" "G:\Ohjelmat\Arc5\bin\Debug\net6.0\target\localisation"
using Pastel;
using System.Text.RegularExpressions;
string[] languages = new string[] { "english" };
Console.WriteLine(string.Join(' ', args));
Dictionary<string, string> Localisation;
Regex reg = MyRegex();

foreach(string language in languages)
{
    Localisation = new();
    foreach (string arg in args)
    {
        Console.WriteLine($"Processing Argument {arg}".Pastel(ConsoleColor.Yellow));
        LoadFolder(arg, language);
    }
}
void LoadFolder(string path, string language)
{
    Console.WriteLine($"Reading Folder {path}".Pastel(ConsoleColor.Yellow));
    string[] Folders = Directory.GetDirectories(path);
    string[] Files = Directory.GetFiles(path);

    foreach (string file in Files)
    {
        if(file.EndsWith($"_l_{language}.yml"))
            LoadFile(file);
    }

    foreach(string folder in Folders)
    {
        if (folder.EndsWith("replace"))
            continue;

        LoadFolder(folder, language);
    }
}
void LoadFile(string path)
{
    Console.WriteLine($"Reading File {path}".Pastel(ConsoleColor.Yellow));
    string[] file = File.ReadAllLines(path);

    for(int i = 1; i < file.Length; i++)
    {
        Match m = reg.Match(file[i]);
        if(m.Success)
        {
            if (Localisation.ContainsKey(m.Groups[1].Value))
            {
                Console.WriteLine($"Existing Key {m.Groups[1].Value} in File {path} at line {i + 1} original was from {Localisation[m.Groups[1].Value]}".Pastel(ConsoleColor.Red));
            }
            else
            {
                Localisation.Add(m.Groups[1].Value, path);
            }
        }
    }
}

partial class Program
{
    [GeneratedRegex("^ ([A-Za-z0-9_.-]+):\\d* \"(.*)\"$")]
    private static partial Regex MyRegex();
}