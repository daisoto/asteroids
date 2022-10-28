using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
public class CameraShaker
{
    private readonly Camera _camera;

    public CameraShaker(Camera camera)
    {
        _camera = camera;
    }

    public void Shake(float duration, Vector3 strength) => 
        _camera.DOShakePosition(duration, strength);
}
}
