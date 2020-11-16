using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodRadarScript : MonoBehaviour
{

    private GameObject[] foods;
    private Player parent;
    public bool foodContact;
    public Transform closestfood;
    // Start is called before the first frame update

    private float radius, angle = 120;
    private SphereCollider sphereCollider;

    private void Awake() {
        parent = GetComponentInParent<Player>();
        sphereCollider = GetComponent<SphereCollider>();
        if (parent == null) {
            Debug.LogError("FoodCollisionDetector: parent organism is null");
        }
        parent.setChild(this);
    }

    public void SetValues(float r, float a) {
        radius = r;
        angle = a;
        sphereCollider.radius = r * 2 / 3;
    }

    void Start() {
        foodContact = false;
        closestfood = null;
    }

    private void OnTriggerStay(Collider collision) {
        if (collision.isTrigger!=true && collision.CompareTag("foodnew") && validDir(collision.gameObject)) {
            GameObject old = parent.getTarget();
            if (old == null) {
                closestfood = collision.gameObject.transform;
                //closestfood.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0.7f, 1);
                parent.setTarget(closestfood.gameObject);
                foodContact = true;
            } else {
                float distNew = Vector3.Distance(parent.transform.position, collision.gameObject.transform.position);
                float distOld = Vector3.Distance(parent.transform.position, old.transform.position);
                if (distNew < distOld) {
                    closestfood = collision.gameObject.transform;
                    parent.setTarget(closestfood.gameObject);
                }
            }
        }
    }

    private bool validDir(GameObject other) {
        Vector3 angleToTarget = (other.transform.position - sphereCollider.center).normalized;
        return Vector3.Angle(transform.parent.forward, angleToTarget) < angle / 2;
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.isTrigger != true && collision.CompareTag("foodnew")) {
    
            //closestfood.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            foodContact = false;
        }
    }

    public Transform getClosestFood() {
        foods = GameObject.FindGameObjectsWithTag("foodnew");
        float closestDist = Mathf.Infinity;
        Transform trans = null;
        foreach(GameObject food in foods) {
            float currentDist = Vector3.Distance(transform.position, food.transform.position);
            float angleToTarget = Vector3.Angle(parent.transform.forward, food.transform.position - parent.transform.position);
            if (currentDist < closestDist && angleToTarget <= angle / 2) {
                closestDist = currentDist;
                trans = food.transform;
            }
        }
        return trans;
    }
}
