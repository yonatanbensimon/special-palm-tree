using UnityEngine;
using System.Collections;

public class HorseSpawner : MonoBehaviour
{
    public GameObject horseSpawner;
    public Transform spawnLocation;
    public float delayBeforeSpawn = 5f;

    void Start()
    {
        StartCoroutine(SpawnHorseAfterDelay());
    }

    IEnumerator SpawnHorseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSpawn);

        GameObject horse = Instantiate(horseSpawner, spawnLocation.position, Quaternion.identity);
        horse.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, 0f);
    }
}
