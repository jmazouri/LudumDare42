using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public float Speed;

    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis ("Horizontal");
        var vertical = Input.GetAxis ("Vertical");

        var movement = new Vector2 (horizontal, vertical);

        body.AddForce(movement * Speed);
    }
}