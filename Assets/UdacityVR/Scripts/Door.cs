using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour 
{
    // Reference to score canvas.
    public Text scores;

    // Create a boolean value called "locked" that can be checked in OnDoorClicked() 
    public bool locked = true;

    // Create a boolean value called "opening" that can be checked in Update() 
    public bool opening = false;

    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip openingSound;

    void Update() {
        // If the door is opening and it is not fully raised
        // Animate the door raising up
        if (opening && transform.position.y < 8.13f)
        {
            transform.Translate(0, 1.5f * Time.deltaTime, 0, Space.World);
        }
    }

    public void OnDoorClicked() {
        // If the door is clicked and unlocked
        if (!locked)
        {
            // Set the "opening" boolean to true
            opening = true;
            audioSource.clip = openingSound;
            audioSource.Play();

            // Display scores
            scores.text = "You collected " + Coin.collected + " coin"+(Coin.collected != 1 ? "s" : "")+"!\nClick for another game";
        }
        else
        {
            // (optionally) Else
            // Play a sound to indicate the door is locked
            audioSource.clip = lockedSound;
            audioSource.Play();
        }
    }

    public void Unlock()
    {
        // You'll need to set "locked" to false here
        locked = false;
    }
}
