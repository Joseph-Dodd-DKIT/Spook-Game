using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float JumpHeight;
    private bool jumping = false;
    private Animator animator;
    private int score = 0;

    private bool isPowerUp = false;
    private float PowerTime = 5;
    public float DefaultPowerUpTime = 5;
    private float PowerUpTimeRemain = 5;

    public GameObject ProjectilePrefad;

    private Vector2 startPosition;

    [SerializeField] private UIManager ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        Vector2 position = transform.position;

        if(position.y < -8.5)
        {
            position = startPosition;
        }
        else
        {
            position.x = position.x+(speed * Time.deltaTime * move);   
        }
        
        transform.position = position;
        updateAnimation(move);

        if(Input.GetKeyDown(KeyCode.Space) && jumping == false)
        {
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * JumpHeight)), ForceMode2D.Impulse);
            jumping = true;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Debug.Log("Fire");
            GameObject Projectile = Instantiate(ProjectilePrefad,
            rb.position, Quaternion.identity);
            Projectile pr = Projectile.GetComponent<Projectile>();
            pr.Launch(new Vector2(animator.GetInteger("Direction"), 0), 300);
        }

        if (isPowerUp)
        {
            PowerUpTimeRemain -= Time.deltaTime;
            if(PowerUpTimeRemain < 0)
            {
                isPowerUp = false;
                PowerUpTimeRemain = DefaultPowerUpTime;
                speed /= 2;
                animator.speed /= 2;
            }
        }

    }

    private void updateAnimation(float move)
    {
        animator.SetFloat("Move", move);
        if(move > 0)
        {
        animator.SetInteger("Direction", 1);
        }
        else if (move < 0)
        {
            animator.SetInteger("Direction", -1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "SpeedPower")
        {
            Destroy(collision.gameObject);
            speed = speed * 2;
            isPowerUp = true;
            animator.speed *= 2;
        }

        
    }

    public void CollectPumpkin()
    {
        score++;
        ui.SetScore(score);
    }
}

