namespace Gameplay
{
public class ProjectilesPool: Pool<ProjectileModel>
{
    public ProjectilesPool(IFactory<ProjectileModel> factory) : base(factory) { }
}
}