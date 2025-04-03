using Spine.Unity;
using UnityEngine;

public static class ArrowAnim
{
    public const string Idle = "idle";
    public const string Attack = "attack";
}

[RequireComponent(typeof(SkeletonAnimation), typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionPower;

    private SkeletonAnimation _anim;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _anim = GetComponent<SkeletonAnimation>();
        _rb = GetComponent<Rigidbody2D>();

        _anim.AnimationState.SetAnimation(0, ArrowAnim.Idle, true);
    }

    private void Update()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explose();
    }

    private void Explose()
    {
        _anim.AnimationState.SetAnimation(0, ArrowAnim.Attack, false);
        _rb.simulated = false;
        _rb.velocity = Vector3.zero;

        foreach (Collider2D nearbyObject in Physics2D.OverlapCircleAll(transform.position, _explosionRadius))
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * _explosionPower, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject, 1);
    }

    public void Throw(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }
}
