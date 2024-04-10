using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] AudioSource src;
    [SerializeField] private AudioClip doorOpenSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DOOR_OPEN, this.destoryDoor);
    }
    private void OnDestroy()
    {
       
        Messenger.RemoveListener(GameEvent.DOOR_OPEN, this.destoryDoor);
    }

    public void destoryDoor()
    {
        
        StartCoroutine(waitBeforeDestroy(this.gameObject));
       
    }

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        src.PlayOneShot(doorOpenSound);
        yield return new WaitForSeconds(2.3f);
       
        Destroy(gameObject);
    }

}
