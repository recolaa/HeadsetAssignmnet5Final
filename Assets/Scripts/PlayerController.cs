using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed = 720f; // degrees per second
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the caveman by setting velocity directly (no rolling, no accumulating force)
        Vector3 velocity = movement * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Face the direction of movement
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            winText.text = "You Win!";
        }
    }
}