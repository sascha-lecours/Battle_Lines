using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    #region Public Properties
    public int facing = 1; // 1: faces rightward. -1 = leftward.
    public string kPrefix = "";

    // public float walkSpeed = 1f; // Might just use movescript values for this
    public float attackInterval = 1f;
    public float initialIdleTime = 0.5f;
    public bool alwaysAttack = false;



    public AudioClip[] hurtSounds;
    public float hurtVolume = 1f;
    public AudioClip[] attackSounds;
    public float attackVolume = 1f;
    public AudioClip[] spawnSounds;
    public float spawnVolume = 1f;
    public AudioClip[] deathSounds;
    public float deathVolume = 1f;

    
    

    #endregion

    #region Private Properties
    Animator animator;
    AudioSource audioSource;
    MoveScript ms;

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
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
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ms = GetComponent<MoveScript>();

        kIdleAnim = kPrefix + kIdleAnim;
        kAttackingAnim = kPrefix + kAttackingAnim;
        kWalkingAnim = kPrefix + kWalkingAnim;
        kHitAnim = kPrefix + kHitAnim;
        kDyingAnim = kPrefix + kDyingAnim;
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
    #endregion
    #region Private Methods
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }


    void AttackEvent()
    {
        Debug.Log("Attack happened");

        // TODO: Spawn attack!
    }

    void StopMoving()
    {
        // Decelerates
        ms.direction = new Vector2(0, 0);
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
                    if (alwaysAttack)
                    {
                        EnterState(State.Attacking);
                        break;
                    }
                    EnterState(State.Walking);
                }
                break;

            case State.Walking:
                // Transition to attacking, maybe go idle if encounter obstacle?
                if (alwaysAttack && timeInState >= attackInterval)
                {
                    EnterState(State.Attacking);
                    break;
                }
                // TODO: Code here to detect if target is in range and attack for those not on auto-fire
                    break;

            case State.Attacking:
                // If attack animation played out fully, back to walking.
                if (!AnimatorIsPlaying())
                {
                    EnterState(State.Walking);
                }
                break;

            case State.Dying:
                // DIE
                break;

        }
    }

    #endregion
}
