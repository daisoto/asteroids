using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
[Serializable]
public class Exploder
{
    [SerializeField]
    private ParticleSystem _explosion;
    
    public async UniTask ToggleExplosion()
    {
        _explosion.Play();
        
        await UniTask.WaitUntil(HasStoppedExploding);
    }
    
    private bool HasStoppedExploding() => 
        _explosion == null || !_explosion.isPlaying;
}
}