using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

private Animator anim;
public GameObject camera;
public Rigidbody2D rb2d;

private Vector2 charScale;
private float charScaleX;
public float speed;


public float graceTime;

public AudioSource bgSource;
public AudioSource winSource;
public AudioSource pickupSource;
public AudioSource enemySource;
public AudioSource jumpSource;
public AudioSource powerupSource;

public TextMeshProUGUI scoreText;
public TextMeshProUGUI livesText;
public TextMeshProUGUI winText;
public TextMeshProUGUI overText;
public TextMeshProUGUI restartText;

public TextMeshProUGUI restartWinText;

private int score;
public int lives;
public float jumpForce;

private float powerTime = 4;

private bool facingRight = true;

private bool restart;

    void Start()
    {
        anim = GetComponent<Animator>();
        bgSource.Play();
        rb2d = GetComponent<Rigidbody2D>();

        score = 0;
        SetScoreText();
        winText.text = "";
        overText.text = "";
        restartText.text = "";
        restartWinText.text = "";
        restart = false;

        lives = 3;
        SetLivesText();
        StartCoroutine (Restart());
    }

    void Update()
    {
        if (lives <= 1)
        {
            livesText.color = new Color(168f/255f, 23f/255f, 23f/255f);
        }
        if (lives == 0)
        {
            overText.text = "Game Over";
            Destroy(rb2d);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Animator>());
        }

        if (Input.GetKeyDown("right")) 
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("right")) 
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown("left"))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("left")) 
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            anim.SetBool("Jump Bool", true);
        }
        if (restart)
        {
            if (Input.GetKeyDown ("space"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
            else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }

    }

    void OnTriggerEnter2D(Collider2D other) 
        {
        if (other.gameObject.CompareTag("Pickup")) 
            {
                other.gameObject.SetActive (false);
                pickupSource.Play();
                score = score + 1;
                SetScoreText();
            }

        if (other.gameObject.CompareTag("Powerup")) 
            {
                other.gameObject.SetActive (false);
                powerupSource.Play();
                speed = 20;
                StartCoroutine("PowerUpTime");
            }

        if (score == 4 && !other.gameObject.CompareTag("Enemy"))
            {
                transform.position = new Vector3(16f, -2.2f, 0f);
                camera.transform.position = new Vector3(23.75f, 0.6f, -10f);
                livesText.color = Color.black;
                lives = 3;
                SetLivesText();
            }

        else if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.SetActive(false);
                enemySource.Play();
                lives = lives - 1;
                SetLivesText ();
            }

        }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpSource.Play();
            }
            anim.SetBool("Jump Bool", false);
        }
    }


    IEnumerator PowerUpTime()
    {  
        yield return (new WaitForSeconds(3));
        speed = 10;
    }
    IEnumerator Restart()
    {
        while (true)
        {
            yield return new WaitForSeconds(graceTime);
            if (string.Compare(overText.text, "Game Over") == 0)
            {
                restartText.text = "Press 'Spacebar' to Restart";
                restart = true;
                break;
            }
            if (score == 8)
            {
                restartWinText.text = "Press 'Spacebar' to Restart";
                restart = true;
                break;
            }
        }
    }


    void SetScoreText ()
    {
        scoreText.text = "Score: " + score.ToString ();
            if (score >= 8) {
            bgSource.Stop();
            winSource.Play();
            winText.text = "You Win!";
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            GameObject.Destroy(enemy);
            }
    }
    void SetLivesText ()
    {
        livesText.text = "Lives: " + lives.ToString ();
    }
    void Flip ()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
