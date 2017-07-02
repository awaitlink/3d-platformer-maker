using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 5f;
    public float jumpSpeed = 10f;

    private bool jumpAvailable = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Update()
    {
        if (State.isPlaying)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            float y = 0f;
            if (Input.GetKeyDown(KeyCode.Space) && jumpAvailable)
            {
                y = jumpSpeed;
                jumpAvailable = false;
            }

            rb.velocity = new Vector3(x * speed, rb.velocity.y + y, z * speed);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        jumpAvailable = true;
    }

}
