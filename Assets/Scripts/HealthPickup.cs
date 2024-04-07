using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private int healthValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Entered the OnTriggerEnter");
            Messenger<int>.Broadcast(GameEvent.PICKUP_HEALTH, healthValue);
            Destroy(this.gameObject);

        }
    }
}
