using Zenject;

public interface ISquaresWeightGenerator : IInitializable, ITickable
{
    void Generate();
}
