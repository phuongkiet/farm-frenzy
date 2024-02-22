using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TimeAgent))]
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float spawnArea_height = 1.0f;
    [SerializeField] float spawnArea_width = 1.0f;

    [SerializeField] GameObject[] spawner;
    int length;
    [SerializeField] float probability = 0.1f;

    private void Start()
    {
        length = spawner.Length;
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }
    void Spawn()
    {
        if(Random.value > probability)
        {
            return;
        }
        GameObject go = Instantiate(spawner[Random.Range(0, length)]);
        Transform t = go.transform;

        Vector3 position = transform.position;
        position.x = UnityEngine.Random.Range(-spawnArea_width, spawnArea_width);
        position.y = UnityEngine.Random.Range(-spawnArea_height, spawnArea_height);

        t.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea_width * 2, spawnArea_height * 2));
    }
}
