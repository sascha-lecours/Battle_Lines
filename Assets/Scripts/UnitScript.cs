using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    #region Public Properties
    public string name = "";
    public int facing = 1; // 1: faces rightward. -1 = leftward.
    public string kPrefix = "";

    public float attackInterval = 1f;
    public float initialIdleTime = 0.5f;
    public bool alwaysAttack = false;

    public Transform attackTransform;
    public Vector3 attackOffset = new Vector3(0.05f, 0, 0);
    public int attackDamage = 1;

    public string specialText = "";


    public AudioClip[] hurtSounds;
    public float hurtVolume = 1f;
    public AudioClip[] attackSounds;
    public float attackVolume = 1f;
    public AudioClip[] spawnSounds;
    public float spawnVolume = 1f;
    public AudioClip[] deathSounds;
    public float deathVolume = 1f;

    public int myTeam;


    #endregion

    #region Private Properties
    Animator animator;
    AudioSource audioSource;
    MoveScript ms;
    HealthScript hs;
    Collider2D collider2d;
    SpriteRenderer sr;

    string corpseSortingLayer = "Corpses";

   

    float stateStartTime;
    float lastAttackTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }
    float timeSinceLastAttack
    {
        get { return Time.time - lastAttackTime; }
    }

    private string kIdleAnim = "Idle";
    private string kAttackingAnim = "Attack";
    private string kWalkingAnim = "Walk";
    private string kHitAnim = "Hit";
    private string kDyingAnim = "Death";
    

    enum State
    {
        Idle,
        Attacking,
        Walking,
        Dying
    }
    State state;

    #endregion

    #region MonoBehaviour Events
    // Start is called before the first frame update
    void Start()
    {
        // ##TODO; change move speed by a small random fraction to help units separate?
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ms = GetComponent<MoveScript>();
        hs = GetComponent<HealthScript>();
        collider2d = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        myTeam = hs.team;

        kIdleAnim = kPrefix + kIdleAnim;
        kAttackingAnim = kPrefix + kAttackingAnim;
        kWalkingAnim = kPrefix + kWalkingAnim;
        kHitAnim = kPrefix + kHitAnim;
        kDyingAnim = kPrefix + kDyingAnim;

        Vector3 defaultScale = transform.localScale;
        transform.localScale = new Vector3(defaultScale.x * facing, defaultScale.y, defaultScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Update state
        ContinueState();
    }
    #endregion
    #region Public Methods
    public void PlaySound(AudioClip[] soundArray, float volume)
    {
        var i = Random.Range(0, soundArray.Length);
        AudioSource.PlayClipAtPoint(soundArray[i], transform.position, volume);
    }

    public void Die()
    {
        sr.sortingLayerName  = corpseSortingLayer;
        EnterState(State.Dying);
    }
    #endregion
    #region Private Methods
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }


    void AttackEvent()
    {
        var newAttack = Instantiate(attackTransform) as Transform;
        newAttack.position = transform.position + new Vector3(attackOffset.x*facing, attackOffset.y, attackOffset.z);
        var newAttackMoveScript = newAttack.GetComponent<MoveScript>();
        newAttackMoveScript.direction = new Vector2(facing, 0); // TODO: this will need to eventually handle vertical attacks too.

        var newAttackShotScript = newAttack.GetComponent<ShotScript>();
        newAttackShotScript.team = myTeam;
        newAttackShotScript.damage = attackDamage;
        lastAttackTime = Time.time;
    }

    void EnemyDetected() // Used to trigger range-limited attacks like melee attacks, and to stop when bumping into an enemy
    {
        StopMoving();
        if (timeSinceLastAttack >= attackInterval && (state == State.Idle || state == State.Walking))
        {
            EnterState(State.Attacking);
        }
    }

    void StopMoving()
    {
        ms.StopFast();
    }



    void WalkForward()
    {
        ms.direction = new Vector2(facing, 0);
    }

    void SetOrKeepState(State state)
    {
        if (this.state == state) return;
        EnterState(state);
    }

    void ExitState()
    {
    }


    void EnterState(State state)
    {
        ExitState();
        switch (state)
        {
            case State.Idle:
                animator.Play(kIdleAnim);
                StopMoving();
                break;
            case State.Walking:
                animator.Play(kWalkingAnim);
                WalkForward();
                break;
            case State.Attacking:
                animator.Play(kAttackingAnim);
                StopMoving();
                break;
            case State.Dying:
                animator.Play(kDyingAnim);
                gameObject.layer = LayerMask.NameToLayer("Dead");
                StopMoving();
                ms.speed = new Vector2(0, 0);
                break;
        }

        this.state = state;
        stateStartTime = Time.time;
    }

    void ContinueState()
    {
        switch (state)
        {

            case State.Idle:
                // Transition to start moving or attacking
                if (timeInState >= initialIdleTime)
                {
                    if(alwaysAttack && timeSinceLastAttack >= attackInterval)
                    {
                        EnterState(State.Attacking);
                        break;
                    }
                    // Todo: Prevent this part from happening if in contact w/ enemy?
                    EnterState(State.Walking);
                }
                break;

            case State.Walking:
                // Transition to attacking, maybe go idle if encounter obstacle?
                if (alwaysAttack && timeSinceLastAttack >= attackInterval)
                {
                    EnterState(State.Attacking);
                    break;
                }
                    break;

            case State.Attacking:
                if (!AnimatorIsPlaying())
                {
                        EnterState(State.Idle);
                }
                break;

            case State.Dying:
                // TODO: reduce sprite alpha gradually, and finally despawn entire object
                StopMoving();
                break;

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HealthScript target = collision.gameObject.GetComponent<HealthScript>();
        if (target != null && target.team != myTeam && !target.dead)
        {
            EnemyDetected();
        }
            
    }

    #endregion
}
