using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    #region Public Properties

    public AudioClip[] hurtSounds;
    public float hurtVolume = 1f;
    public AudioClip[] attackSounds;
    public float attackVolume = 1f;
    public AudioClip[] spawnSounds;
    public float spawnVolume = 1f;
    public AudioClip[] deathSounds;
    public float deathVolume = 1f;

    public string kPrefix = "";
    

    #endregion

    #region Private Properties
    Animator animator;
    AudioSource audioSource;

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    public string kIdleAnim = "Idle";
    public string kAttackingAnim = "Attack";
    public string kWalkingAnim = "Walk";
    public string kHitAnim = "Hit";
    public string kDyingAnim = "Death";


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
        switch (state) // ##TDOD: Expand
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
                EnterState(State.Attacking);
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
