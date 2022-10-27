namespace Gameplay
{
public class AsteroidsPool: Pool<AsteroidModel, AsteroidSize>
{ 
    public AsteroidsPool(IFactory<AsteroidModel, AsteroidSize> factory) : 
        base(factory) { }
}
}