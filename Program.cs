// ./"Localisation Duplicate Finder.exe" "C:\SteamLibrary\steamapps\common\Europa Universalis IV\localisation" "G:\Ohjelmat\Arc5\bin\Debug\net6.0\target\localisation"
using Pastel;
using System.Text.RegularExpressions;
using System.Linq;

List<string> languages = new();
Console.WriteLine(string.Join(' ', args));
Dictionary<string, string> Localisation;
Regex reg = MyRegex();
Regex reg2 = MyOtherRegex();
foreach(string arg in args)
{
    string path = Path.Combine(arg, "languages.yml");
    if (File.Exists(path))
    {
        languages = new(); //Guessing it works like replacing the vanilla ones
        string file = File.ReadAllText(path);
        MatchCollection mc = reg2.Matches(file);
        languages.AddRange(from Match m in mc select m.Value);
    }
}

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
        if(file.Contains($"{language}"))
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

    for (int i = 0; i < file.Length; i++)
    {
        string s = file[i];
        Match m = reg.Match(s);
        if (m.Success) {
            if (Localisation.ContainsKey(m.Value))
            {
                Console.WriteLine($"Existing Key {m.Value} in File {path} at line {i+1} original was from {Localisation[m.Value]}".Pastel(ConsoleColor.Red));
            }
            else
            {
                Localisation.Add(m.Value, path);
            }
        }
    }
}

partial class Program
{
    [GeneratedRegex("\\b\\S+(?=:\\d*\\s*\"[^\\n]*\")")]
    private static partial Regex MyRegex();
    [GeneratedRegex("(?<=^|\\n)l_\\S+(?=:)")]
    private static partial Regex MyOtherRegex();
}