using Spine.Unity;
using Spine;
using UnityEngine;

public static class ArcherAnim
{
    public const string Idle = "idle";
    public const string AttackStart = "attack_start";
    public const string AttackFinish = "attack_finish";
    public const string AttackTarget = "attack_target";
}

public static class ArcherSkeleton
{
    public const string Bullet = "bullet";
    public const string Gun = "gun";
    public const string Shoot = "shoot";
}

[RequireComponent(typeof(SkeletonAnimation))]
public class Archer : MonoBehaviour
{
    [SerializeField] private Trajectory _trajectory;
    [SerializeField] private InputController _input;
    [SerializeField] private Arrow _arrow;
    [Space]
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _force;

    private SkeletonAnimation _anim;
    private Bone _gunBone;
    private Bone _bulletBone;
    private Vector2 _shootDir;

    private void Awake()
    {
        _anim = GetComponent<SkeletonAnimation>();
        _anim.AnimationState.Event += HandleAttack;
        _bulletBone = _anim.Skeleton.FindBone(ArcherSkeleton.Bullet);
        _gunBone = _anim.Skeleton.FindBone(ArcherSkeleton.Gun);
    }

    private void OnEnable()
    {
        _input.onStartDrag += StartAttack;
        _input.onEndDrag += FinishAttack;
        _input.onDrag += TargetAttack;
    }

    private void OnDisable()
    {
        _input.onStartDrag -= StartAttack;
        _input.onEndDrag -= FinishAttack;
        _input.onDrag -= TargetAttack;
    }

    private void Start()
    {
        _anim.AnimationState.SetAnimation(0, ArcherAnim.Idle, true);
    }

    private void StartAttack()
    {
        _anim.AnimationState.SetAnimation(0, ArcherAnim.AttackStart, false);
        _anim.AnimationState.AddAnimation(0, ArcherAnim.AttackTarget, true, 0);
        _trajectory.ShowDots();
    }

    private void FinishAttack()
    {
        _anim.AnimationState.SetAnimation(0, ArcherAnim.AttackFinish, false);
        _anim.AnimationState.AddAnimation(0, ArcherAnim.Idle, true, 0);
        _trajectory.HideDots();
    }

    private void TargetAttack(Vector2 dir)
    {
        float angle = Mathf.Clamp(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, -_maxAngle, _maxAngle);
        _shootDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * Mathf.Clamp(dir.magnitude, 0, _maxDistance);
        _gunBone.Rotation = angle;
        _anim.Skeleton.UpdateWorldTransform();
        _trajectory.UpdateDots(_bulletBone.GetWorldPosition(transform), _shootDir * _force);
    }

    private void HandleAttack(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == ArcherSkeleton.Shoot)
        {
            Arrow arrow = Instantiate(_arrow, _bulletBone.GetWorldPosition(transform), Quaternion.AngleAxis(_gunBone.Rotation, Vector3.forward));
            arrow.Throw(_shootDir * _force);
        }
    }
}
