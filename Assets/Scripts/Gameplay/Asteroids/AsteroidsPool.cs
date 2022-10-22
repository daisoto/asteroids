public class AsteroidsPool: Pool<AsteroidModel>
{ 
    public AsteroidsPool(IFactory<AsteroidModel> factory) : base(factory) { }
}