using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPathController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2[] setPaths;
    public int currentPathIndex = 0;
    public float speed = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, setPaths[currentPathIndex], speed * Time.deltaTime);
        if (transform.position.x == setPaths[currentPathIndex].x && transform.position.y == setPaths[currentPathIndex].y)
        {
            currentPathIndex++;
            if(currentPathIndex >= setPaths.Length) { 
                currentPathIndex = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            collision.transform.parent = this.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
