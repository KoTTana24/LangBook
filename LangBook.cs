using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static string folderPath = "Dictionaries";
    static string currentDictionary = "";
    static string language = "EN";  // По умолчанию — английский

    static string languageFile = "language.txt";  // Файл для сохранения языка

    static Dictionary<string, (string en, string ru)> text = new()
    {
        // Главный экран
        {"menu_title", ("=== MAIN MENU ===", "=== ГЛАВНОЕ МЕНЮ ===")},
        {"create", ("1 - Create dictionary", "1 - Создать словарь")},
        {"open", ("2 - Open dictionary", "2 - Открыть словарь")},
        {"lang", ("3 - Change language", "3 - Сменить язык")},
        {"exit", ("0 - Exit", "0 - Выйти")},

        // Меню словаря
        {"dict_menu", ("=== DICTIONARY ===", "=== СЛОВАРЬ ===")},
        {"add", ("1 - Add word", "1 - Добавить слово")},
        {"show", ("2 - Show words", "2 - Показать слова")},
        {"search", ("3 - Smart search", "3 - Умный поиск")},
        {"learn", ("4 - Learning mode", "4 - Режим обучения")},
        {"back", ("0 - Back to menu", "0 - Выйти в меню")},

        // Ввод слова
        {"enter_name", ("Enter dictionary name:", "Введите имя словаря:")},
        {"created", ("Dictionary created!", "Словарь создан!")},
        {"select", ("Select dictionary number:", "Выберите номер словаря:")},
        {"empty", ("Dictionary is empty.", "Словарь пуст.")},

        {"word", ("Enter word:", "Введите слово:")},
        {"translation", ("Enter translation:", "Введите перевод:")},
        {"example", ("Enter example sentence:", "Введите пример предложения:")},
        {"saved", ("Word saved!", "Слово сохранено!")},

        // Поиск
        {"search_word", ("Enter word to search:", "Введите слово для поиска:")},
        {"not_found", ("Word not found.", "Слово не найдено.")},
        {"maybe", ("Maybe you meant:", "Возможно вы имели в виду:")},

        // Режим обучения
        {"need_words", ("Need at least 2 words to start learning mode (5 recommended)",
                         "Нужно минимум 2 слова для режима обучения (рекомендуется 5)")},

        {"correct", ("Correct!", "Верно!")},
        {"almost", ("Almost correct! Correct answer:", "Почти правильно! Правильный ответ:")},
        {"wrong", ("Wrong! Correct answer:", "Неверно! Правильный ответ:")},

        // Сообщения о выходе
        {"exit_learning", ("Press any key to exit learning mode", "Нажмите любую клавишу для выхода из режима обучения")}
    };

    static void Main()
    {
        LoadLanguage();  // Загружаем выбранный язык
        CreateFolder();
        while (true) ShowMainMenu();
    }

    static void LoadLanguage()
    {
        if (File.Exists(languageFile))
        {
            language = File.ReadAllText(languageFile).Trim();
        }
    }

    static void SaveLanguage()
    {
        File.WriteAllText(languageFile, language);
    }

    static void CreateFolder()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    // ================= MAIN MENU =================

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
            case "1": CreateDictionary(); break;
            case "2": OpenDictionary(); break;
            case "3": ChangeLanguage(); break;
            case "0": Environment.Exit(0); break;
        }
    }

    // ================= DICTIONARIES =================

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
            Console.WriteLine($"{i + 1} - {Path.GetFileNameWithoutExtension(files[i])}");

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
            Console.WriteLine($"[{currentDictionary}]\n");
            Print("add");
            Print("show");
            Print("search");
            Print("learn");
            Print("back");

            Console.Write("\n> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddWord(); break;
                case "2": ShowWords(); break;
                case "3": SmartSearch(); break;
                case "4": LearningMode(); break;
                case "0": return;
            }
        }
    }

    // ================= WORD STORAGE =================

    static string GetPath()
    {
        return Path.Combine(folderPath, currentDictionary + ".txt");
    }

    static Dictionary<string, (string translation, string example)> LoadWords()
    {
        var dict = new Dictionary<string, (string, string)>();

        foreach (var line in File.ReadAllLines(GetPath()))
        {
            var parts = line.Split('=');
            if (parts.Length != 2) continue;

            var right = parts[1].Split('|');
            if (right.Length == 2)
                dict[parts[0]] = (right[0], right[1]);
        }
        return dict;
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

        Print("example");
        Console.Write("\n> ");
        string example = Console.ReadLine();

        File.AppendAllText(GetPath(), $"{word}={translation}|{example}\n");
        Print("saved");
        Console.ReadKey();
    }

    static void ShowWords()
    {
        Console.Clear();
        var words = LoadWords();

        if (words.Count == 0)
        {
            Print("empty");
        }
        else
        {
            foreach (var pair in words)
            {
                Console.WriteLine($"{pair.Key} → {pair.Value.translation}");
                Console.WriteLine($"   Example: {pair.Value.example}\n");
            }
        }
        Console.ReadKey();
    }

    // ================= SMART SEARCH =================

    static void SmartSearch()
    {
        Console.Clear();
        var words = LoadWords();

        Print("search_word");
        Console.Write("\n> ");
        string input = Console.ReadLine().ToLower();

        if (words.ContainsKey(input))
        {
            var data = words[input];
            Console.WriteLine($"{input} → {data.translation}");
            Console.WriteLine($"Example: {data.example}");
        }
        else
        {
            var matches = words.Keys
                .Select(w => new { Word = w, Distance = Levenshtein(input, w.ToLower()) })
                .Where(x => x.Distance <= 2)
                .OrderBy(x => x.Distance)
                .Take(5)
                .ToList();

            if (matches.Count == 0)
            {
                Print("not_found");
            }
            else
            {
                Print("maybe");
                foreach (var m in matches)
                    Console.WriteLine($"'{m.Word}'");
            }
        }
        Console.ReadKey();
    }

    // ================= LEARNING MODE =================

    static void LearningMode()
    {
        Console.Clear();
        var words = LoadWords();

        if (words.Count < 2)
        {
            Print("need_words");
            Console.ReadKey();
            return;
        }

        // Let user specify number of words to learn at a time
        Console.WriteLine("How many words would you like to study at a time? (default 10)");
        int numberToStudy = 10;
        string input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
            int.TryParse(input, out numberToStudy);

        var rnd = new Random();
        var list = words.ToList();
        int score = 0;

        // Randomize word order
        var shuffledWords = list.OrderBy(x => rnd.Next()).Take(numberToStudy).ToList();

        foreach (var pair in shuffledWords)
        {
            Console.Clear();
            Console.WriteLine($"Translate: {pair.Value.translation}");
            Console.Write("Word > ");
            string answer = Console.ReadLine().Trim();

            int dist = Levenshtein(answer.ToLower(), pair.Key.ToLower());

            if (dist == 0)
            {
                Print("correct");
                score++;
            }
            else if (dist <= 2)
            {
                Print("almost");
                Console.WriteLine(pair.Key);
                score++;
            }
            else
            {
                Print("wrong");
                Console.WriteLine(pair.Key);
            }

            Console.WriteLine($"\nExample: {pair.Value.example}");
            Console.ReadKey();
        }

        Console.Clear();
        Console.WriteLine($"Score: {score} / {shuffledWords.Count}");
        Console.WriteLine("\n" + text["exit_learning"].ru);
        Console.ReadKey();
    }

    // ================= UTILS =================

    static int Levenshtein(string a, string b)
    {
        int[,] d = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) d[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[a.Length, b.Length];
    }

    static void ChangeLanguage()
    {
        Console.Clear();
        Console.WriteLine("1 - English");
        Console.WriteLine("2 - Русский");
        Console.Write("\n> ");

        string choice = Console.ReadLine();
        if (choice == "1") 
        { 
            language = "EN"; 
            SaveLanguage(); 
        }
        if (choice == "2") 
        { 
            language = "RU"; 
            SaveLanguage(); 
        }
    }

    static void Print(string key)
    {
        if (language == "EN")
            Console.WriteLine(text[key].en);
        else
            Console.WriteLine(text[key].ru);
    }
}
