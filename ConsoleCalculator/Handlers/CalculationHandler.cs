using System.Globalization;

namespace ConsoleCalculator.Handlers;

/// <summary>
/// Handler works with ExpressionModel. Fills ExpressionModel messages if find mistakes. Calculates result by reverse polish notation.
/// </summary>
public class CalculationHandler
{
    private Stack<string> _operatorList = new Stack<string>();
    private List<string> _rpnList = new List<string>();
    private readonly string _opetatorSymbols = @"-|\+|\/|\*";
    private readonly string _symbols = @"\d+[\.|\,]\d+|-|\+|\(|\)|\/|\*|\d+";
    private readonly string _operatorSymbols = @"^[\/\-*+]$";

    private delegate double MathOperationDelegate(double leftOperand, double rightOperand);
    private Dictionary<string, MathOperationDelegate> _actions = new Dictionary<string, MathOperationDelegate>()
    {
        { "+", (x, y) => x + y },
        { "-", (x, y) => x - y },
        { "*", (x, y) => x * y },
        { "/", (x, y) => x / y },
    };

    private readonly Dictionary<string, int> _symbolWeight = new Dictionary<string, int>() 
    { 
        { "(", 3 },
        { ")", 3 },
        { "+", 2 },
        { "-", 2 },
        { "/", 1 },
        { "*", 1 }
    };

    /// <summary>
    /// Calculates expression result in ExpressionModel.
    /// </summary>
    /// <param name="expr"> ExpressionModel with filled Expression property. </param>
    public string Calculate(ExpressionModel expr)
    {
        var validateHandler = new ValidateHandler();
        if (!validateHandler.CheckValidExpression(expr))
        {
            foreach (var message in expr.Messages)
            {
                Console.WriteLine(message);
            }
            return "Расчет невозможен";
        }
        
        ConvertRPN(expr.Expression);
        ResultCalculation();
        return _rpnList.First();
    }

    /// <summary>
    /// Converts expression to reverse polish notation.
    /// </summary>
    /// <param name="expr"> Expression for convert. </param>
    private void ConvertRPN(string expr)
    { 
        var symbolList = DivideExpressionBySymbols(expr);

        foreach (var symbol in symbolList)
        {
            if (symbol.Value.Equals("("))
            {
                _operatorList.Push(symbol.Value);
            }
            else if (symbol.Value.Equals(")"))
            {
                while (!_operatorList.Peek().Equals("("))
                {
                    _rpnList.Add(_operatorList.Pop());
                }
                _operatorList.Pop();
            }
            else if (Regex.Match(symbol.Value, _opetatorSymbols).Success)
            {
                FromStackToList(symbol.Value);
            }
            else
            {
                _rpnList.Add(symbol.Value);
            }
        }

        foreach (var operSymb in _operatorList)
        {
            _rpnList.Add(operSymb);
        }
    }

    /// <summary>
    /// Pushing characters off the stack onto a reverse polish notation list.
    /// </summary>
    /// <param name="operatorSymbol"> Operator symbol adding to stack. </param>
    private void FromStackToList(string operatorSymbol)
    {
        while (_operatorList.Count >= 0)
        {
            if (_operatorList.Count == 0)
            {
                _operatorList.Push(operatorSymbol);
                break;
            }

            _symbolWeight.TryGetValue(_operatorList.Peek(), out var weight);
            if (_symbolWeight[operatorSymbol] >= weight)
            {
                _rpnList.Add(_operatorList.Pop());
            }
            else
            {
                _operatorList.Push(operatorSymbol);
                break;
            }
        }
    }

    /// <summary>
    /// Сalculating the result using reverse polish notation.
    /// </summary>
    private void ResultCalculation()
    {
        while (_rpnList.Count > 1)
        {
            var mathOperator = _rpnList.First(x => Regex.Match(x, _operatorSymbols).Success);
            var operatorIndex = _rpnList.IndexOf(mathOperator);
            var leftOperand = double.Parse(_rpnList[operatorIndex - 2], CultureInfo.CurrentCulture);
            var rightOperand = double.Parse(_rpnList[operatorIndex - 1], CultureInfo.CurrentCulture);
            var result = _actions[mathOperator].Invoke(leftOperand, rightOperand);

            _rpnList[operatorIndex - 2] = Math.Round(result, 2).ToString();
            _rpnList.RemoveRange(operatorIndex - 1, 2);
        }
    }

    /// <summary>
    /// Divide expression to symbols.
    /// </summary>
    /// <param name="expr"> Calculated expression </param>
    /// <returns> List<Math> containing expression symbols. </returns>
    private List<Match> DivideExpressionBySymbols(string expr) => Regex.Matches(expr, _symbols).ToList();
}