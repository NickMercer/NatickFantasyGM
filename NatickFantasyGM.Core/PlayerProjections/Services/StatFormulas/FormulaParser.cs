using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;
using org.mariuszgromada.math.mxparser;
using System.Text.RegularExpressions;

namespace NatickFantasyGM.Core.PlayerProjections.Services.StatFormulas;

//This class should be the ONLY place mXparser is referenced. 
//It needs to be encapsulated incase there's an issue with mXparser (as our core domain needs a math parser). 
//And the naming conventions of mXparser are disgusting, so we shouldn't see them elsewhere in the program.
public class FormulaParser
{
    private static string _varNames = "xyzadhuolq";

    public double Calculate(string resultAbbreviation, string formula, IEnumerable<Stat> statCollection)
    {
        var includedStats = GetIncludedStatsFromFormula(resultAbbreviation, formula, statCollection);
        formula = ReplaceStatsWithSafeFormulaVariables(formula, includedStats);
        var expression = BuildExpression(formula, includedStats);

        double result = expression.calculate();
        return result;
    }

    private static List<Stat> GetIncludedStatsFromFormula(string resultAbbreviation, string formula, IEnumerable<Stat> statCollection)
    {
        var tokens = formula.Split(" ").ToList();
        var tokenList = new List<string>();
        tokens.ForEach(x =>
        {
            var trimmedToken = Regex.Replace(x, @"[^\w]*", string.Empty);
            if (trimmedToken.Length > 0)
            {
                tokenList.Add(trimmedToken);
            }
        });

        var includedStats = statCollection
            .OrderByDescending(x => x.StatIdentifier.Abbreviation.Length)
            .Where(x => tokenList.Contains(x.StatIdentifier.Abbreviation))
            .ToList();

        if(includedStats.Select(s => s.StatIdentifier.Abbreviation).Contains(resultAbbreviation))
        {
            throw new InvalidOperationException("Stat cannot contain itself in it's formula.");
        }
        
        return includedStats;
    }
    private static string ReplaceStatsWithSafeFormulaVariables(string formula, List<Stat> includedStats)
    {
        for (int i = 0; i < includedStats.Count; i++)
        {
            formula = formula
                .Replace(includedStats[i].StatIdentifier.Abbreviation, _varNames[i].ToString());
        }

        return formula;
    }
    private static Expression BuildExpression(string formula, List<Stat> includedStats)
    {
        var expression = new Expression(formula);
        var arguments = new Argument[includedStats.Count];
        for (int i = 0; i < includedStats.Count; i++)
        {
            arguments[i] = new Argument(_varNames[i].ToString(), includedStats[i].Value);
        }
        expression.addArguments(arguments);

        return expression;
    }
}
