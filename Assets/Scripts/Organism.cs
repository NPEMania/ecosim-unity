using EcoSimUtil;
using UnityEditor;
using UnityEngine;

public class Organism : MonoBehaviour
{
    // range is distance radius, angle is the field of view angle
    private float speed, range = 10, angle = 90;

    private SphereCollider spCollider;

    private GameObject target;
    private Rigidbody body;

    public void setValues(float speed, float range, float angle) {
        this.speed = speed;
        this.range = range;
        this.angle = angle;
        spCollider.radius = range;
    }

    void Start() {
        spCollider = GetComponent<SphereCollider>();
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (target == null) {

        } else {
            body.MovePosition(target.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("food") && validDir(other.gameObject)) {
            if (target == null) {
                setTarget(target);
            } else {
                float oldDist = Vector3.Distance(transform.position, target.transform.position);
                float newDist = Vector3.Distance(transform.position, other.transform.position);
                if (newDist < oldDist) {
                    setTarget(target);
                }
            }
        }
    }

    private void wander() {

    }

    private void setTarget(GameObject other) {
        target = other;
        transform.LookAt(target.transform);
    }

    private bool validDir(GameObject other) {
        Vector3 angleToTarget = (other.transform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, angleToTarget) < angle / 2;
    }

    public Organism Create(GameObject prefab, Transform parent, Vector3 pos, Vector3 dir, float speed, float range, float angle) {
        GameObject gObj = Instantiate(prefab, pos, Quaternion.LookRotation(dir));
        Organism org = gObj.GetComponent<Organism>();
        org.setValues(speed, range, angle);
        return org;
    }

    private void OnDrawGizmosSelected() {
        Vector3 rayA = Constants.dirFromAngle(angle / 2);
        Vector3 rayB = Constants.dirFromAngle(-angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 20);
        Gizmos.DrawLine(transform.position, transform.position + rayA * range);
        Gizmos.DrawLine(transform.position, transform.position + rayB * range);
        
    }
}
