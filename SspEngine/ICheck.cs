using SspEngine.DomainModel;

namespace SspEngine
{
    public interface ICheck
    {
        string Description { get; }

        int Ordinality { get; }

        RatingResult RunCheck(Risk risk);
    }
}