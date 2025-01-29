using UnityEngine;

public class ProjectileBoardObjectView : IBoardObjectView
{
    private IBoardObject _boardObject;
    private GameObject _gameObject;

    public ProjectileBoardObjectView(IBoardObject boardObject)
    {
        _boardObject = boardObject;
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        _gameObject.transform.localScale = new Vector3(0.5f, .5f, 0.5f);
    
        _gameObject.GetComponent<Renderer>().material.color = Color.blue;
        _gameObject.name = "Projectile";
        _gameObject.transform.position = new Vector3(0, 0, -20);
    }

    public void Draw()
    {
        _gameObject.transform.position = new Vector3(_boardObject.Position.x, _boardObject.Position.y, 0);
        _gameObject.transform.rotation = Quaternion.Euler(0, 0, _boardObject.Rotation + 90);
    }

    public void Dispose()
    {
        Object.Destroy(_gameObject);
    }
}