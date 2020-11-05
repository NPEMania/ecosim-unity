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
            Vector2 circ = Random.insideUnitCircle;
            Vector3 pos = Random.Range(5, 50) * new Vector3(circ.x, 0, circ.y);
            GameObject food = Instantiate(foodPrefab,
                pos,
                Quaternion.identity
            );
            food.transform.parent = foodSpace.transform;
            foodSet.Add(food);
        }
    }

    private void spawnOrganisms() {
        Organism org = Organism.Create(foodSet, orgPrefab, orgSpace.transform,
            new Vector3(0, 3, -90), Vector3.forward,
            7, 8, 180
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
