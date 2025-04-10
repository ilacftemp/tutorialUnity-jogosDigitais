using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audio;
    public float speed;
    public float maxSpeed = 8f;

    [Header("Rotação orgânica")]
    public float angularVelocityThreshold = 50f;

    private bool teveGiroForte = false;
    private float tempoGiro = 0f;
    private bool corrigindo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!corrigindo)
        {
            if (Mathf.Abs(rb.angularVelocity) > angularVelocityThreshold)
            {
                teveGiroForte = true;
                tempoGiro = Time.time;
            }

            if (teveGiroForte && Mathf.Abs(rb.angularVelocity) < 5f && Time.time - tempoGiro > 0.1f)
            {
                corrigindo = true;
                rb.freezeRotation = true;
                StartCoroutine(CorrigirRotacaoFinal());
                teveGiroForte = false;
            }
        }
    }

    IEnumerator CorrigirRotacaoFinal()
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, 0f);
        float t = 0f;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.5f)
        {
            t += Time.deltaTime * 2f;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
        rb.freezeRotation = false;
        corrigindo = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coletavel"))
        {
            FindObjectOfType<GameUI>().AddPoint();
            audio.Play();
            Destroy(other.gameObject);
        }
    }

    public void IncreaseSpeed(float amount)
    {
        speed = Mathf.Min(speed + amount, maxSpeed);
    }
}
