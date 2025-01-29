using UnityEngine;

public class ShipBoardObjectView : IBoardObjectView
{
    private IBoardObject _boardObject;
    private GameObject _gameObject;

    public ShipBoardObjectView(IBoardObject boardObject)
    {
        _boardObject = boardObject;
        _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject front = GameObject.CreatePrimitive(PrimitiveType.Cube);
        front.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        front.transform.SetParent(_gameObject.transform);
        front.transform.localPosition = new Vector3(1, 0, 0);
    }

    public void Draw()
    {
        _gameObject.transform.position = new Vector3(_boardObject.Position.x, _boardObject.Position.y, 0);
        _gameObject.transform.rotation = Quaternion.Euler(0, 0, _boardObject.Rotation);
    }

    public void Dispose()
    {
        Object.Destroy(_gameObject);
    }
}