using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    public GameObject foodPrefab, orgPrefab;
    public GameObject foodSpace, orgSpace;
    public HashSet<GameObject> foodSet;
    public List<Organism> organisms;

    void Start() {
        foodSet = new HashSet<GameObject>();
        organisms = new List<Organism>();
        foodSpace = GameObject.FindGameObjectWithTag("foodSpace");
        orgSpace = GameObject.FindGameObjectWithTag("orgSpace");
        spawnFood();
        spawnOrganisms();
    }

    private void spawnFood() {
        for (int i = 0; i < 20; ++i) {
            GameObject food = Instantiate(foodPrefab,
                new Vector3(Random.Range(-25, 25), 0.5f, Random.Range(-25, 25)),
                Quaternion.identity
            );
            food.transform.parent = foodSpace.transform;
            foodSet.Add(food);
        }
    }

    private void spawnOrganisms() {
        Organism org = Organism.Create(foodSet, orgPrefab, orgSpace.transform,
            new Vector3(0, 3, -48), Vector3.forward,
            7, 8, 120
        );
        //North Wall
        //South Wall
        //East Wall
        //West Wall
    }

    private void Update() {
        if (foodSet.Count == 0) {
            Debug.Log("All food consumed");
        }
    }
}
