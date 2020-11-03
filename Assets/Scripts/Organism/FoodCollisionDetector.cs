using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollisionDetector : MonoBehaviour
{
    private Organism parentOrganism;
    private SphereCollider sphereCollider;
    private float angle;

    private void Awake() {
        parentOrganism = GetComponentInParent<Organism>();
        sphereCollider = GetComponent<SphereCollider>();
        if (parentOrganism == null) {
            Debug.LogError("FoodCollisionDetector: parent organism is null");
        }
    }

    public void initialize(float range, float angle) {
        sphereCollider.radius = range;
        this.angle = angle;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("food") && validDir(other.gameObject)) {
            GameObject oldTarget = parentOrganism.getTarget();
            if (oldTarget == null) {
                parentOrganism.setTarget(other.gameObject);
            } else {
                float oldDist = Vector3.Distance(transform.parent.position, oldTarget.transform.position);
                float newDist = Vector3.Distance(transform.parent.position, other.transform.position);

                if (newDist < oldDist) parentOrganism.setTarget(other.gameObject);
            }
        }
    }

    private bool validDir(GameObject other) {
        Vector3 angleToTarget = (other.transform.position - sphereCollider.center).normalized;
        return Vector3.Angle(transform.parent.forward, angleToTarget) < angle / 2;
    }
}
