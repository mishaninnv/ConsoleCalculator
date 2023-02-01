using System.Configuration;
using System.Text.RegularExpressions;

namespace ConsoleCalculator.Handlers;

public class ValidateHandler
{
    private readonly string? _divisionByZeroPattern = ConfigurationManager.AppSettings["DivisionByZeroPattern"];
    private readonly string? _validSymbols = ConfigurationManager.AppSettings["ValidSymbols"];

    public bool CheckValidExpression(ExpressionModel model)
    {
        if (CheckDivisionByZero(model.Expression))
        {
            model.Messages.Add("Присутствует деление на ноль");
        }
        if (!CheckSymbols(model.Expression))
        {
            model.Messages.Add("Присутствуют недопустимые символы");
        }
        if (!CheckParenthesesSeries(model.Expression))
        {
            model.Messages.Add("Скобки расставлены неправильно");
        }
        return model.Messages.Count == 0;
    }

    private bool CheckDivisionByZero(string expr) => Regex.Match(expr, _divisionByZeroPattern).Success;

    private bool CheckSymbols(string expr) => Regex.Match(expr, _validSymbols + $"{{ { expr.Length} }}").Success;

    private bool CheckParenthesesSeries(string expr)
    {
        var counter = 0;

        foreach (var symbol in expr)
        {
            if (symbol.Equals('(')) counter++;
            else if (symbol.Equals(')')) counter--;

            if (counter < 0) return false;
        }

        return counter == 0;
    }
}