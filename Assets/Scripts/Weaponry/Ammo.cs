using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float AmmoMin;
    public float AmmoMax;

    private Collider2D _collider;
    private GameUIController _uiController;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _uiController = GameObject.Find("GameHUD").GetComponent<GameUIController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _uiController.AddAmmo(Random.Range(AmmoMin, AmmoMax));
        Debug.Log("Added ammo");
        Destroy(gameObject);
    }
}
