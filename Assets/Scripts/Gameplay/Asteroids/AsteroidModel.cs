public class AsteroidModel
{
    public AsteroidSize Size { get; }
    
    public int Damage { get; }
    
    public AsteroidModel(AsteroidSize size, int damage)
    {
        Size = size;
        Damage = damage;
    }
}

public enum AsteroidSize { Small, Medium, Big }