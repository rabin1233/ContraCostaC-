using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;


    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;
    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;
    //public AudioSource shootSound;


    Rigidbody2D rigidBody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;
    TrailRenderer trail;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        rigidBody.simulated = false;
    }
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapSound.Play();

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ScoreZone")
        {
            //register a score event
            OnPlayerScored(); //event sent to GameManager
            scoreSound.Play();                  //play a sound

        }
        if (col.gameObject.tag == "DeadZone")
        {
            rigidBody.simulated = false;
            //register a deadevent
            OnPlayerDied();
            dieSound.Play();//event sent to GameManager; 
            //play a sound
        }
    }
}

