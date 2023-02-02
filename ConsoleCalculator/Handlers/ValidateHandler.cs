namespace ConsoleCalculator.Handlers;

public class ValidateHandler
{
    private readonly string _divisionByZeroPattern = @"(\d\/0$)|(\d\/0[^.,])";
    private readonly string _validSymbols = @"[\d\(\)\-\/*+,.]";
    private readonly string _severalOperatorsPattern = @"[+\-*\/][+\-*\/]";
    private readonly string _invalidEndSymbols = @"[-|\+|*|\/]$";
    private readonly string _invalidStartSymbols = @"^[\+|*|\/]";
    private delegate bool Methods(string expr);
    private Dictionary<Methods, string> _methods = new Dictionary<Methods, string>();

    public ValidateHandler()
    {
        _methods.Add(CheckDivisionByZero, "Присутствует деление на ноль");
        _methods.Add(CheckEndSymbol, "Недопустимый конечный символ");
        _methods.Add(CheckParenthesesSeries, "Скобки расставлены неправильно");
        _methods.Add(CheckStartSymbol, "Недопустимый начальный символ");
        _methods.Add(CheckSeveralOperatorsRow, "Несколько операторов подряд");
        _methods.Add(CheckSymbols, "Присутствуют недопустимые символы");
    }

    public bool CheckValidExpression(ExpressionModel model)
    {
        foreach (var method in _methods)
        {
            if (method.Key.Invoke(model.Expression))
            {
                model.Messages.Add(method.Value);
            }
        }
        return model.Messages.Count == 0;
    }

    private bool CheckStartSymbol(string expr) => Regex.Match(expr, _invalidStartSymbols).Success;

    private bool CheckEndSymbol(string expr) => Regex.Match(expr, _invalidEndSymbols).Success;

    private bool CheckSeveralOperatorsRow(string expr) => Regex.Match(expr, _severalOperatorsPattern).Success;

    private bool CheckDivisionByZero(string expr) => Regex.Match(expr, _divisionByZeroPattern).Success;

    private bool CheckSymbols(string expr) => !Regex.Match(expr, _validSymbols + $"{{{expr.Length}}}").Success;

    private bool CheckParenthesesSeries(string expr)
    {
        var counter = 0;

        foreach (var symbol in expr)
        {
            if (symbol.Equals('(')) counter++;
            else if (symbol.Equals(')')) counter--;

            if (counter < 0) return true;
        }

        return counter != 0;
    }
}