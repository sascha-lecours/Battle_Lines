using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    #region Public Properties
    public string kPrefix = "";

    public float walkSpeed = 1f;
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
    void SetOrKeepState(State state)
    {
        if (this.state == state) return;
        EnterState(state);
    }

    void ExitState()
    {
    }

    void AttackEvent()
    {
        Debug.Log("Attack happened");
    }

    void EnterState(State state)
    {
        ExitState();
        switch (state)
        {
            case State.Idle:
                animator.Play(kIdleAnim);
                break;
            case State.Walking:
                animator.Play(kWalkingAnim);
                break;
            case State.Attacking:
                animator.Play(kAttackingAnim);
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
                break;

            case State.Attacking:
                
                break;

            case State.Dying:
                // DIE
                break;

        }
    }

    #endregion
}
