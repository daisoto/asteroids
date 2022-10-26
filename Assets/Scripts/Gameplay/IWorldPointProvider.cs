using UnityEngine;

namespace Gameplay
{
public interface IWorldPointProvider
{
    Vector3 GetFromScreen(Vector2 pos);
}
}