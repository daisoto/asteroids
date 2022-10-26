using UnityEngine;

namespace Gameplay
{
public class WallsSetter : MonoBehaviour, IWorldPointProvider
{
    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private Transform _topRightWall;
    
    [SerializeField]
    private Transform _bottomLeftWall;
    
    [SerializeField]
    private Transform _spaceShip;
    
    private float _depth;
    
    private void Awake() =>
        _depth = _camera.transform.position.y - _spaceShip.position.y;
    
    private void Start() => SetBoundaries();
    
    public Vector3 GetFromScreen(Vector2 pos) =>
        _camera.ScreenToWorldPoint(
            new Vector3(pos.x, pos.y, _depth));

    private void SetBoundaries()
    {
        var w = Screen.width;
        var h = Screen.height;
        
        var topRightPos = GetFromScreen(new Vector2(w, h));
        var bottomLeftPos  = GetFromScreen(Vector2.zero);
        
        _topRightWall.position = topRightPos;
        _bottomLeftWall.position = bottomLeftPos;
    }
}
}