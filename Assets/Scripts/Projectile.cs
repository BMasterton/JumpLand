using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactPrefab;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Untagged"
            || collision.gameObject.tag == "Platform"
            || collision.gameObject.tag == "AngryPig"
            || collision.gameObject.tag == "Ghost"
            || collision.gameObject.tag == "FatBird")
        {
            Destroy(this.gameObject);
        }
       
        GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
        Destroy(impact, 1);     // destroy the explosion after 2 seconds
        Destroy(gameObject);    // destroy the projectile
    }
}
