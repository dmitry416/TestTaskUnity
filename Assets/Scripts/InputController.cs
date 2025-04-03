using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action onStartDrag;
    public Action onEndDrag;
    public Action<Vector2> onDrag;

    private bool _isDrag = false;
    private Vector2 _startPos;
    private Vector2 _endPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDrag = true;
            _startPos = Input.mousePosition;
            onStartDrag?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDrag = false;
            onEndDrag?.Invoke();
        }
        if (_isDrag) 
        {
            _endPos = Input.mousePosition;
            onDrag?.Invoke(Camera.main.ScreenToWorldPoint(_startPos) - Camera.main.ScreenToWorldPoint(_endPos));
        }
    }
}
