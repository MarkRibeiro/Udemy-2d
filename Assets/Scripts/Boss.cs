using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public bool bossActive;

    public float timeBetweenDrops;
    private float dropCount;

    public float waitForPlatforms;
    private float platformCount;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform dropSawSpawnPoint;

    public GameObject dropSaw;


    // Start is called before the first frame update
    void Start()
    {
        dropCount = timeBetweenDrops;
        platformCount = waitForPlatforms;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossActive){
            if(dropCount > 0) {
                dropCount -= Time.deltaTime;
            } else {
                dropSawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), dropSawSpawnPoint.position.y, dropSawSpawnPoint.position.z);
                Instantiate(dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
                dropCount = timeBetweenDrops;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            bossActive = true;
        }
    }
}
