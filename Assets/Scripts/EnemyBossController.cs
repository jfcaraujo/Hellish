using UnityEngine;

public class EnemyBossController : EnemyController
{
    // STATE MANAGEMENT

    /// <summary>
    /// Enemy Boss states (in accordance to the animations in the animator).
    /// Each constant contains the hash value that represents each state.
    /// These values should be compared with <see cref="AnimatorStateInfo.fullPathHash"/>.
    /// </summary>
    public static class State
    {
        public static readonly int IdleTaunt = Animator.StringToHash("Base Layer.EnemyBossIdleTaunt");
    }

    /// <summary>
    /// Animator parameters (in accordance to the parameters set in the animator).
    /// Each constant contains the string value of each parameter.
    /// It does not contain information about the type of each parameter.
    /// </summary>
    public static class AnimatorParameters
    {
        public static readonly string Taunt = "Taunt";
        public static readonly string Stomp = "Stomp";
        public static readonly string MagicAttack = "MagicAttack";
    }

    [SerializeField]
    [Range(0f, 1f)]
    private float tauntChances = 0.25f;

    [SerializeField]
    private float tauntInterval = 10f;

    private float timePassed = 0f;

    protected override void Start()
    {
        base.Start();

        timePassed = tauntInterval;
    }

    protected override void Update()
    {
        base.Update();

        timePassed += Time.deltaTime;

        if (timePassed >= tauntInterval && Random.value <= tauntChances)
        {
            _animator.SetTrigger(AnimatorParameters.Taunt);
            timePassed = 0;
        }
    }

    protected override void SeeingPlayer(float targetDistance)
    {
        base.SeeingPlayer(targetDistance);

        if (targetDistance <= chaseTargetRadius && _agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            _animator.SetTrigger(AnimatorParameters.MagicAttack);
        }
    }

    protected override void AttackTarget()
    {
        _agent.speed = 0;
        base.AttackTarget();
    }

    protected override void ShootBall()
    {
        handFireball.gameObject.SetActive(false);
        GameObject bullet = Instantiate(fireball, handFireball.position, handFireball.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 distance = PlayerManager.Instance.playerCenter.position - handFireball.position;
        float heightDifference = distance.y;
        distance.y = 0;

        float horizontalTravellingSpeed = 10;
        float gravityForce = Physics.gravity.y;

        float neededTime = distance.magnitude / horizontalTravellingSpeed;

        float verticalTravellingSpeed = (heightDifference - 0.5f * gravityForce * Mathf.Pow(neededTime, 2)) / neededTime;

        rb.velocity = distance.normalized * horizontalTravellingSpeed + Vector3.up * verticalTravellingSpeed;
    }

    public void InhibitMovement()
    {
        _agent.speed = 0;
    }

    public void RegainMovement()
    {
        _agent.speed = maxMovingVelocity;
    }
}