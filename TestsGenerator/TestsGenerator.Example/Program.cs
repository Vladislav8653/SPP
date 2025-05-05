using TestsGenerator.Core;

Console.WriteLine("Укажите путь к папке с исходными .cs файлами:");
var sourceDir = Console.ReadLine();

if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir))
{
    ShowErrorAndExit("Папка с исходниками не найдена.");
    return;
}

Console.WriteLine("Куда сохранить сгенерированные тесты? Введите путь к выходной директории:");
var destDir = Console.ReadLine();

if (string.IsNullOrWhiteSpace(destDir))
{
    ShowErrorAndExit("Путь к выходной папке не может быть пустым.");
    return;
}

if (!Directory.Exists(destDir))
{
    Directory.CreateDirectory(destDir);
    Console.WriteLine($"Создана новая директория: {destDir}");
}

var csFiles = Directory.EnumerateFiles(sourceDir, "*.cs", SearchOption.AllDirectories).ToArray();
if (csFiles.Length == 0)
{
    Console.WriteLine("В указанной папке не найдено ни одного .cs файла.");
    WaitAndClose();
    return;
}

Console.WriteLine($"Обнаружено файлов: {csFiles.Length}. Запуск генерации тестов...");

var testGen = new TestGenerator();
await testGen.GenerateTestsAsync(csFiles, destDir);

Console.WriteLine("Генерация тестовых файлов завершена успешно!");
WaitAndClose();
return;

static void ShowErrorAndExit(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
    WaitAndClose();
}

static void WaitAndClose()
{
    Console.WriteLine("Нажмите любую клавишу для выхода...");
    Console.ReadKey();
}