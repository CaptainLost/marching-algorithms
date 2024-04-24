using Zenject;

public interface ISquaresNoiseGenerator : IInitializable, ITickable
{
    void Generate();
}
