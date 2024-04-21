using Zenject;

public abstract class SquaresNoiseGenerator : IInitializable
{
    public abstract void Initialize();
    public abstract void Generate();
}
