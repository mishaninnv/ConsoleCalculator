using System.Globalization;

namespace ConsoleCalculator.Models;

/// <summary>
/// Expression model.
/// </summary>
public class ExpressionModel
{
    public string Expression { get; set; }
    public List<string> Messages { get; set; } = new List<string>();

    public ExpressionModel(string expression)
    {
        Expression = ExpressionAdjustment(expression);
    }

    /// <summary>
    /// Bringing an expression to a general form.
    /// </summary>
    /// <param name="expression"> User input. </param>
    private string ExpressionAdjustment(string expression)
    {
        expression = expression.Replace(")(", ")*(");
        expression = expression.Replace(" ","");
        if (Regex.Match(expression, @"^-").Success)
        {
            expression = expression.Insert(0, "0");
        }

        var unidentifiedAction = Regex.Matches(expression, @"\d\(|\)\d").ToArray();

        foreach (var action in unidentifiedAction)
        {
            expression = expression.Replace(action.ToString(), action.ToString().Insert(1, "*"));
        }

        expression = Regex.Replace(expression, @"[.]+|[,]+", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

        return expression;
    }
}
