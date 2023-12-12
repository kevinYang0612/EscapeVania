using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound; 
    [SerializeField] int pointsForCoinPickUp = 10;
    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickUp);
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }    
    }
}
