using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Reference to Audio Source
    AudioSource audioSource;

    // Reference to the CoinPoofPrefab
    public GameObject coinPoof;

    // Reference to Coin container
    public Transform coinContainer;

    // Count collected coins
    public static int collected = 0;

    // Rotation speed
    public float speed = 10f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        collected = 0;
    }

    public void OnCoinClicked()
    {
        // Play sound
        audioSource.Play();

        // Instantiate the CoinPoof Prefab where this coin is located
        // Make sure the poof animates vertically
        Object.Instantiate(coinPoof, transform.position, Quaternion.Euler(-90f, 0f, 0f), coinContainer);

        // Destroy this coin. Check the Unity documentation on how to use Destroy
        Destroy(gameObject, 0.7f);

        collected++;
    }

    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
