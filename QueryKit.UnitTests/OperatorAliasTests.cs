namespace QueryKit.UnitTests;

using FluentAssertions;
using WebApiTestProject.Entities.Recipes;

public class OperatorAliasTests
{
    [Fact]
    public void can_handle_alias_text_with_space()
    {
        var input = """Title ti "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "ti";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title == "titilating")""");
    }

    [Fact]
    public void can_handle_alias_text_with_casing_and_space()
    {
        var input = """Title ti "titilating ties a ti" || Title Ti "titilater" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "ti";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => ((x.Title == "titilating ties a ti") OrElse (x.Title == "titilater"))""");
    }

    [Fact]
    public void can_do_symbol_alias()
    {
        var input = """Title @ "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title == "titilating")""");
    }

    [Fact]
    public void can_do_symbols_alias()
    {
        var input = """Title @@$ "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@@$";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title == "titilating")""");
    }

    [Fact]
    public void can_do_symbols_alias_with_default_case_insensitive()
    {
        var input = """Title @@$* "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@@$";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title.ToLower() == "titilating".ToLower())""");
    }

    [Fact]
    public void can_do_symbols_alias_case_insensitive()
    {
        var input = """Title @@$~ "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@@$";
            config.ComparisonAliases.CaseInsensitiveAppendix = "~";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title.ToLower() == "titilating".ToLower())""");
    }

    [Fact]
    public void can_do_symbols_alias_with_case_insensitive_conflicting_chars()
    {
        var input = """Title @@$ "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@@$";
            config.ComparisonAliases.CaseInsensitiveAppendix = "$";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title == "titilating")""");
    }

    [Fact]
    public void can_use_contains_not_case_sensitive()
    {
        var input = """Title @@$ "titilating" """;
        
    
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.ContainsOperator = "@@$";
            config.ComparisonAliases.CaseInsensitiveAppendix = "$";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => x.Title.Contains("titilating")""");
    }

    [Fact]
    public void can_do_symbols_alias_case_insensitive_with_conflicting_chars()
    {
        var input = """Title @@$$ "titilating" """;
        
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "@@$";
            config.ComparisonAliases.CaseInsensitiveAppendix = "$";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title.ToLower() == "titilating".ToLower())""");
    }

    [Fact]
    public void can_do_symbols_alias_case_insensitive_with_no_symbols()
    {
        var input = """Title eqt "titilating" """;
        
        var config = new QueryKitConfiguration(config =>
        {
            config.ComparisonAliases.EqualsOperator = "eq";
            config.ComparisonAliases.CaseInsensitiveAppendix = "t";
        });
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title.ToLower() == "titilating".ToLower())""");
    }

    [Fact]
    public void can_have_custom_config_object()
    {
        var input = """Title eq "titilating" """;

        var config = new CustomQueryKitConfiguration();
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Title == "titilating")""");
    }

    [Fact]
    public void can_use_ints()
    {
        var input = """Rating gt 10""";

        var config = new CustomQueryKitConfiguration();
        var filterExpression = FilterParser.ParseFilter<Recipe>(input, config);
        filterExpression.ToString().Should().Be($"""x => (x.Rating > 10)""");
    }

    public class CustomQueryKitConfiguration : QueryKitConfiguration
    {
        public CustomQueryKitConfiguration(Action<QueryKitSettings>? configureSettings = null)
            : base(settings => 
            {
                settings.ComparisonAliases.EqualsOperator = "eq";
                settings.ComparisonAliases.NotEqualsOperator = "neq";
                settings.ComparisonAliases.GreaterThanOperator = "gt";
                settings.ComparisonAliases.GreaterThanOrEqualOperator = "gte";
                settings.ComparisonAliases.LessThanOperator = "lt";
                settings.ComparisonAliases.LessThanOrEqualOperator = "lte";
                settings.ComparisonAliases.ContainsOperator = "ct";
                settings.ComparisonAliases.StartsWithOperator = "sw";
                settings.ComparisonAliases.EndsWithOperator = "ew";

                configureSettings?.Invoke(settings);
            })
        {
        }
    }
}