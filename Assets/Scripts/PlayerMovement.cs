using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    AudioSource audio;
    public float maxSpeed = 8f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Coletavel")
        {
            FindObjectOfType<GameUI>().AddPoint();
            audio.Play();
            Destroy(other.gameObject);
        }
    }

    public void IncreaseSpeed(float amount)
    {
        speed += Mathf.Min(speed + amount, maxSpeed);
    }
}
