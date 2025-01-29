using UnityEngine;

public class ObstacleBoardObjectView : IBoardObjectView
{
    private IBoardObject _boardObject;
    private GameObject _gameObject;

    public ObstacleBoardObjectView(IBoardObject boardObject)
    {
        _boardObject = boardObject;
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    
        _gameObject.GetComponent<Renderer>().material.color = Color.red;
        _gameObject.name = "Obstacle";
        _gameObject.transform.position = new Vector3(0, 0, -20);
    }

    public void Draw()
    {
        _gameObject.transform.position = new Vector3(_boardObject.Position.x, _boardObject.Position.y, 0);
    }

    public void Dispose()
    {
        Object.Destroy(_gameObject);
    }
}