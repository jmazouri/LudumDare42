using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour {

	public int HealthMin;
	public int HealthMax;

	private Collider2D _collider;

	private void Start()
	{
		_collider = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player")) return;
		collision.GetComponent<PlayerController>().Heal(Random.Range(HealthMin, HealthMax));
		Debug.Log("Added health");
		Destroy(gameObject);
	}
}
