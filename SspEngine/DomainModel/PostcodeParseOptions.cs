using System;

namespace SspEngine.DomainModel
{
    [Flags]
    public enum PostcodeParseOptions
    {
        None = 0,
        IncodeOptional = 1,
        MatchBfpo = 2,
        MatchOverseasTerritories = 4,
        MatchGirobank = 8,
        MatchSanta = 16
    }
}