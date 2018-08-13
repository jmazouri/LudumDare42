using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int AmmoMin;
    public int AmmoMax;

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
        collision.GetComponentInChildren<Weapon>().Ammo += Random.Range(AmmoMin, AmmoMax);
        Debug.Log("Added ammo");
        Destroy(gameObject);
    }
}
