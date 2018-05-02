using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour 
{
    // Create a reference to the KeyPoofPrefab and Door
    public GameObject keyPoof;
    public Door door;

    public static bool keyCollected = false;

    // Rotation speed
    public float speed = 50f;

    void Update()
	{
        // Not required, but for fun why not try adding a Key Floating Animation here :)
        // Rotate the coin
        transform.Rotate(new Vector3(0,0,1), speed * Time.deltaTime);

        // bounce between 1.81 - 2.14
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(1.81f, 2.14f, Mathf.Abs(Mathf.Cos(Time.time * 3f))), transform.position.z);
    }

	public void OnKeyClicked()
	{
        // Instatiate the KeyPoof Prefab where this key is located
        // Make sure the poof animates vertically
        Object.Instantiate(keyPoof, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        // Call the Unlock() method on the Door
        door.Unlock();

        // Set the Key Collected Variable to true
        keyCollected = true;

        // Destroy the key. Check the Unity documentation on how to use Destroy
        Destroy(gameObject, 0.3f);
    }

}
