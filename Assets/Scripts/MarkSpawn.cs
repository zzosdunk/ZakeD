using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkSpawn : MonoBehaviour {

    public GameObject markPrefab;

	void Start () {
        Spawn();
	}

    public void Spawn()
    {
        Vector3 spawnPos = new Vector2(Random.Range(-3, 6), 0f);
        Instantiate(markPrefab, spawnPos, Quaternion.identity);
    }
}
