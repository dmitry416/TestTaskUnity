using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int _dotsCount;
    [SerializeField] private GameObject _dotsPrefab;
    [SerializeField] private Transform _dotsParent;
    [SerializeField] private float _dotsSpacing;
    [SerializeField] private float _dotsStartScale;
    [SerializeField] private float _dotsEndScale;

    private Transform[] _dots;

    private void Start()
    {
        HideDots();
        PrepareDots();
    }

    private void PrepareDots()
    {
        _dots = new Transform[_dotsCount];
        for (int i = 0; i < _dotsCount; ++i)
        {
            _dots[i] = Instantiate(_dotsPrefab, _dotsParent).transform;
            _dots[i].localScale = Vector3.one * Mathf.Lerp(_dotsStartScale, _dotsEndScale, (i + 1) / (float)_dotsCount);
        }
    }

    public void UpdateDots(Vector2 pos, Vector2 force)
    {
        for (int i = 1; i <= _dotsCount; ++i)
            _dots[i - 1].position = new Vector2(
                pos.x + force.x * _dotsSpacing * i, 
                pos.y + force.y * _dotsSpacing * i - Physics2D.gravity.magnitude * _dotsSpacing * _dotsSpacing * i * i / 2);
    }

    public void ShowDots()
    {
        _dotsParent.gameObject.SetActive(true);
    }

    public void HideDots()
    {
        _dotsParent.gameObject.SetActive(false);
    }
}
