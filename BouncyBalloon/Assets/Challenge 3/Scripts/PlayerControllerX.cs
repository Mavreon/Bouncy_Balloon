using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce = 1.0f;
    public float recoveryForce = 10.0f;
    public float yMaxPosition = 14.0f;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yMaxPosition)//If player Y position is below the yMaxPosition then receive input...
        {
            // While space is pressed and player is low enough, float up
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.AddForce(Vector3.up * floatForce, ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop(); //Stop playing audio on Main Camera gameObject...
            Invoke( "QuitGame", 3.0f);
        }
        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        //If the player collides with the ground add an upward force....
        else if(other.gameObject.CompareTag("Ground"))
        {
            if (!gameOver)
            {
                playerAudio.PlayOneShot(bounceSound);
                playerRb.AddForce(Vector3.up * recoveryForce, ForceMode.Impulse);
            }
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
