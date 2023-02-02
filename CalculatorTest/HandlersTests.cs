using ConsoleCalculator.Models;
using System.Globalization;

namespace CalculatorTest;

public class HandlersTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CheckValidExpressionTest()
    {
        //arrange
        var validateHandler = new ValidateHandler();
        var expressionList = new List<ExpressionModel>();

        expressionList.Add(new ExpressionModel("98502-5293*+/-()999))"));
        expressionList.Add(new ExpressionModel("dfsdfae15648/-*kk"));
        expressionList.Add(new ExpressionModel(""));
        expressionList.Add(new ExpressionModel("1"));
        expressionList.Add(new ExpressionModel("   "));
        expressionList.Add(new ExpressionModel("\n"));
        expressionList.Add(new ExpressionModel("*8+9"));
        expressionList.Add(new ExpressionModel(")9+8("));
        expressionList.Add(new ExpressionModel("9/0"));
        expressionList.Add(new ExpressionModel("8+846484/13848*156.5461"));
        expressionList.Add(new ExpressionModel("8++9"));
        expressionList.Add(new ExpressionModel("9+8*"));

        var result = new List<bool>();
        //act
        foreach (var express in expressionList)
        {
            result.Add(validateHandler.CheckValidExpression(express));
        }
        //assert
        Assert.False(result[0]);
        Assert.False(result[1]);
        Assert.True(result[2]);
        Assert.True(result[3]);
        Assert.True(result[4]);
        Assert.True(result[5]);
        Assert.False(result[6]);
        Assert.False(result[7]);
        Assert.False(result[8]);
        Assert.True(result[9]);
        Assert.False(result[10]);
        Assert.False(result[11]);
    }

    [Test]
    public void CalculateTest()
    {
        //arrange
        var exprList = new List<ExpressionModel>();
        exprList.Add(new ExpressionModel("(9+5)(14/7)")); // 7
        exprList.Add(new ExpressionModel("(9+5/0)(14/7)")); // Расчет невозможен
        exprList.Add(new ExpressionModel("(9+(5-2))(14/7)")); // 24
        exprList.Add(new ExpressionModel("(9+5)(14/7")); // Расчет невозможен
        exprList.Add(new ExpressionModel("5,5-4.2")); // 1.3

        var result = new List<string>();
        //act
        foreach (var express in exprList)
        {
            var calculationHandler = new CalculationHandler();
            result.Add(calculationHandler.Calculate(express));
        }
        //assert
        Assert.That(result[0], Is.EqualTo("28"));
        Assert.That(result[1], Is.EqualTo("Расчет невозможен"));
        Assert.That(result[2], Is.EqualTo("24"));
        Assert.That(result[3], Is.EqualTo("Расчет невозможен"));
        Assert.That(result[4], Is.EqualTo($"1{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}3"));
    }
}