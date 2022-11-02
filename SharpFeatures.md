# Фичи C# списком

## Паттерн-матчинг

### Паттерн свойства

**Начиная с C# 8**

```csharp
public static decimal ComputeSalesTax (Address location, decimal salePrice) =>
    location switch
    {
        { State: "WA" } => salePrice * 0.06M,
        { State: "MN" } => salePrice * 0.075M,
        { State: "MI" } => salePrice * 0.05M,
        _ => 0M
    };
```

### Паттерн кортежа

**Начиная с C# 8**

```csharp
public static string RockPaperScissors (string first, string second)
    => (first, second) switch
    {
        ("rock", "paper") => "rock is covered by paper. Paper wins.",
        ("rock", "scissors") => "rock breaks scissors. Rock wins.",
        ("paper", "rock") => "paper covers rock. Paper wins.",
        ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
        ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
        ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
        (_, _) => "tie"
    };
```

### Позиционный паттерн

**Начиная с C# 8**

```csharp
static Quadrant GetQuadrant (Point point) => point switch
{
    (0, 0) => Quadrant.Origin,
    var (x, y) when x > 0 && y > 0 => Quadrant.One,
    var (x, y) when x < 0 && y > 0 => Quadrant.Two,
    var (x, y) when x < 0 && y < 0 => Quadrant.Three,
    var (x, y) when x > 0 && y < 0 => Quadrant.Four,
    var (_, _) => Quadrant.OnBorder,
    _ => Quadrant.Unknown
};
```

## readonly-методы

**Начиная с C# 8**

Методы, которые не меняют состояние объекта, можно пометить ключевым словом `readonly`

```csharp
public struct Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public readonly double Distance => Math.Sqrt(X * X + Y * Y);

    public readonly override string ToString() =>
        $"({X}, {Y}) is {Distance} from the origin";
}
```

## Методы с реализацией по умолчанию в интерфейсах

**Начиная с C# 8**

```csharp
interface ICar
{
    void GetSpeed();
    void GetMileage();

    public void SendCommand()
    {
        // некая реализация по умолчанию
    }
}
```

## switch-выражения

**Начиная с C# 8**

```csharp
public enum Rainbow
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

public static RGBColor FromRainbow (Rainbow colorBand) =>
    colorBand switch
    {
        Rainbow.Red    => new RGBColor (0xFF, 0x00, 0x00),
        Rainbow.Orange => new RGBColor (0xFF, 0x7F, 0x00),
        Rainbow.Yellow => new RGBColor (0xFF, 0xFF, 0x00),
        Rainbow.Green  => new RGBColor (0x00, 0xFF, 0x00),
        Rainbow.Blue   => new RGBColor (0x00, 0x00, 0xFF),
        Rainbow.Indigo => new RGBColor (0x4B, 0x00, 0x82),
        Rainbow.Violet => new RGBColor (0x94, 0x00, 0xD3),
        _              => throw new ArgumentException
            (
                message: "invalid enum value",
                paramName: nameof(colorBand)
            ),
    };
```

# using-декларация переменной

**Начиная с C# 8**

```csharp
static int WriteLinesToFile (IEnumerable<string> lines)
{
    using var file = new System.IO.StreamWriter ("WriteLines2.txt");
    var skippedLines = 0;
    foreach (var line in lines)
    {
        if (!line.Contains ("Second"))
        {
            file.WriteLine (line);
        }
        else
        {
            skippedLines++;
        }
    }

    return skippedLines;
    // file is disposed here
}
```

## Статические локальные функции

**Начиная с C# 8**

```csharp
int Method()
{
    var y = 5;
    var x = 7;
    return Add (x, y);

    static int Add (int left, int right) => left + right;
}
```

# Очищаемые ref структуры

**Начиная с C# 8**

```csharp
public ref struct SomeStruct
{
    public void Dispose()
    {
       // некие действия по очистке
    }
}
```

# Асинхронные потоки

**Начиная с C# 8**

```csharp
public static async IAsyncEnumerable<int> GenerateSequence()
{
    for (var i = 0; i < 20; i++)
    {
        await Task.Delay (100);
        yield return i;
    }
}
```

# Асинхронная очистка

**Начиная с C# 8**

```csharp
public class SomeClass
    : IAsyncDisposable
{
    protected virtual ValueTask DisposeAsyncCore()
    {
        // некие действия по очистке
    }

    protected virtual void Dispose (bool disposing)
    {
        if (disposing)
        {
            // освобождение managed-ресурсов
        }
    }

    public async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore().ConfigureAwait (false);

        // Dispose of unmanaged resources.
        Dispose (false);

        #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize

        // Suppress finalization.
        GC.SuppressFinalize (this);

        #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }
}
```

# Индексы и диапазоны

**Начиная с C# 8**

```csharp
var words = new string[]
{
                // index from start    index from end
    "The",      // 0                   ^9
    "quick",    // 1                   ^8
    "brown",    // 2                   ^7
    "fox",      // 3                   ^6
    "jumped",   // 4                   ^5
    "over",     // 5                   ^4
    "the",      // 6                   ^3
    "lazy",     // 7                   ^2
    "dog"       // 8                   ^1
};              // 9 (or words.Length) ^0

Console.WriteLine ($"The last word is {words[^1]}");

var quickBrownFox = words[1..4];
var lazyDog = words[^2..^0];
var allWords = words[..]; // contains "The" through "dog".
var firstPhrase = words[..4]; // contains "The" through "fox"
var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"
```

# Присваивание с объединением

**Начиная с C# 8**

```csharp
List<int> numbers = null;
int? i = null;

numbers ??= new List<int>();
numbers.Add (i ??= 17);
numbers.Add (i ??= 20);

Console.WriteLine (string.Join(" ", numbers));  // output: 17 17
Console.WriteLine (i);  // output: 17
```

# Беззнаковый сдвиг

Оператор `>>>` всегда выполняет логический сдвиг. То есть пустые битовые позиции старшего порядка всегда устанавливаются равными нулю, независимо от типа левого операнда. Оператор `>>` выполняет арифметический сдвиг (то есть значение самого старшего бита распространяется на пустые битовые позиции старшего разряда), если левый операнд имеет знаковый тип.

**Начиная с C# 11**

```csharp
int x = -8;
Console.WriteLine ($"Before:    {x,11}, hex: {x,8:x}, binary: {Convert.ToString (x, toBase: 2), 32}");

int y = x >> 2;
Console.WriteLine ($"After  >>: {y,11}, hex: {y,8:x}, binary: {Convert.ToString (y, toBase: 2), 32}");

int z = x >>> 2;
Console.WriteLine ($"After >>>: {z,11}, hex: {z,8:x}, binary: {Convert.ToString (z, toBase: 2).PadLeft (32, '0'), 32}");

// Output:
// Before:             -8, hex: fffffff8, binary: 11111111111111111111111111111000
// After  >>:          -2, hex: fffffffe, binary: 11111111111111111111111111111110
// After >>>:  1073741822, hex: 3ffffffe, binary: 00111111111111111111111111111110
```

# Записи

**Начиная с C# 9**

```csharp
public record Person (string FirstName, string LastName);
```

### with

**Начиная с C# 9**

```csharp
var point1 = new Point (1, 2);
var point2 = point1 with { X = 5, Y = 6 };
```

## Объявление пространства имен на весь файл

**Начиная с C# 10**

```csharp
namespace Some;

...
```
