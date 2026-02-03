using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static string folderPath = "Dictionaries";
    static string currentDictionary = "";
    static string language = "EN"; // EN = English, RU = Русский

    static Dictionary<string, (string en, string ru)> text = new Dictionary<string, (string, string)>()
    {
        {"menu_title", ("=== MAIN MENU ===", "=== ГЛАВНОЕ МЕНЮ ===")},
        {"create", ("1 - Create dictionary", "1 - Создать словарь")},
        {"open", ("2 - Open dictionary", "2 - Открыть словарь")},
        {"lang", ("3 - Change language", "3 - Сменить язык")},
        {"exit", ("0 - Exit", "0 - Выйти")},

        {"dict_menu", ("=== DICTIONARY ===", "=== СЛОВАРЬ ===")},
        {"add", ("1 - Add word", "1 - Добавить слово")},
        {"show", ("2 - Show words", "2 - Показать слова")},
        {"back", ("0 - Back to menu", "0 - Выйти в меню")},

        {"enter_name", ("Enter dictionary name:", "Введите имя словаря:")},
        {"created", ("Dictionary created!", "Словарь создан!")},
        {"select", ("Select dictionary number:", "Выберите номер словаря:")},
        {"empty", ("Dictionary is empty.", "Словарь пуст.")},

        {"word", ("Enter word:", "Введите слово:")},
        {"translation", ("Enter translation:", "Введите перевод:")},
        {"saved", ("Word saved!", "Слово сохранено!")},

        {"lang_changed", ("Language changed!", "Язык изменён!")}
    };

    static void Main()
    {
        CreateFolder();

        while (true)
        {
            ShowMainMenu();
        }
    }

    static void CreateFolder()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Print("menu_title");
        Console.WriteLine();
        Print("create");
        Print("open");
        Print("lang");
        Print("exit");

        Console.Write("\n> ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateDictionary();
                break;
            case "2":
                OpenDictionary();
                break;
            case "3":
                ChangeLanguage();
                break;
            case "0":
                Environment.Exit(0);
                break;
        }
    }

    static void CreateDictionary()
    {
        Console.Clear();
        Print("enter_name");
        Console.Write("\n> ");
        string name = Console.ReadLine();

        string path = Path.Combine(folderPath, name + ".txt");

        if (!File.Exists(path))
        {
            File.Create(path).Close();
            currentDictionary = name;
            Print("created");
            Console.ReadKey();
            DictionaryMenu();
        }
    }

    static void OpenDictionary()
    {
        Console.Clear();
        var files = Directory.GetFiles(folderPath, "*.txt");

        if (files.Length == 0)
        {
            Console.WriteLine("No dictionaries found.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < files.Length; i++)
        {
            Console.WriteLine($"{i + 1} - {Path.GetFileNameWithoutExtension(files[i])}");
        }

        Print("select");
        Console.Write("\n> ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice >= 1 && choice <= files.Length)
            {
                currentDictionary = Path.GetFileNameWithoutExtension(files[choice - 1]);
                DictionaryMenu();
            }
        }
    }

    static void DictionaryMenu()
    {
        while (true)
        {
            Console.Clear();
            Print("dict_menu");
            Console.WriteLine($"[{currentDictionary}]");
            Console.WriteLine();
            Print("add");
            Print("show");
            Print("back");

            Console.Write("\n> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddWord();
                    break;
                case "2":
                    ShowWords();
                    break;
                case "0":
                    return;
            }
        }
    }

    static void AddWord()
    {
        Console.Clear();
        Print("word");
        Console.Write("\n> ");
        string word = Console.ReadLine();

        Print("translation");
        Console.Write("\n> ");
        string translation = Console.ReadLine();

        string path = Path.Combine(folderPath, currentDictionary + ".txt");
        File.AppendAllText(path, $"{word}={translation}\n");

        Print("saved");
        Console.ReadKey();
    }

    static void ShowWords()
    {
        Console.Clear();
        string path = Path.Combine(folderPath, currentDictionary + ".txt");
        var lines = File.ReadAllLines(path);

        if (lines.Length == 0)
        {
            Print("empty");
        }
        else
        {
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                    Console.WriteLine($"{parts[0]} → {parts[1]}");
            }
        }

        Console.ReadKey();
    }

    static void ChangeLanguage()
    {
        Console.Clear();
        Console.WriteLine("1 - English");
        Console.WriteLine("2 - Русский");
        Console.Write("\n> ");

        string choice = Console.ReadLine();
        if (choice == "1") language = "EN";
        if (choice == "2") language = "RU";

        Print("lang_changed");
        Console.ReadKey();
    }

    static void Print(string key)
    {
        if (language == "EN")
            Console.WriteLine(text[key].en);
        else
            Console.WriteLine(text[key].ru);
    }
}
