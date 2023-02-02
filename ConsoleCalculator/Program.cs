global using ConsoleCalculator.Models;
global using System.Text.RegularExpressions;
using ConsoleCalculator.Handlers;

while (true)
{
    var calculationHandler = new CalculationHandler();
    Console.WriteLine("Введите выражение:");

    var userInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(userInput))
    {
        var result = calculationHandler.Calculate(new ExpressionModel(userInput));
        Console.WriteLine(result);
    }

    Console.WriteLine("Для продолжения нажмите любую кнопку. Для выхода ESC.");

    if (Console.ReadKey().Key == ConsoleKey.Escape)
    { 
        break;
    }
}