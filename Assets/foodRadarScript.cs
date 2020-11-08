using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodRadarScript : MonoBehaviour
{

    private GameObject[] foods;
    private player parentOrganism;
    public bool foodContact;
    public Transform closestfood;
    // Start is called before the first frame update

    private void Awake()
    {
        parentOrganism = GetComponentInParent<player>();
       // sphereCollider = GetComponent<SphereCollider>();
        if (parentOrganism == null)
        {
            Debug.LogError("FoodCollisionDetector: parent organism is null");
        }
    }

    void Start()

    {
        foodContact = false;
        closestfood = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.isTrigger!=true && collision.CompareTag("foodnew"))
        {

            closestfood = getClosestFood();
            closestfood.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0.7f, 1);
            parentOrganism.setTarget(closestfood.gameObject);
            foodContact = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.isTrigger != true && collision.CompareTag("foodnew"))
        {
    
            //closestfood.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            foodContact = false;
        }
    }
    public Transform getClosestFood()
    {
        foods = GameObject.FindGameObjectsWithTag("foodnew");
        float closestDist = Mathf.Infinity;
        Transform trans = null;
        foreach(GameObject food in foods)
        {
            float currentDist = Vector3.Distance(transform.position, food.transform.position);
            if (currentDist < closestDist) {
                closestDist = currentDist;
                trans = food.transform;

            }

        }
        return trans;
    }
}
