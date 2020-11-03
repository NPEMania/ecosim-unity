using EcoSimUtil;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour {
    public int changeTime = 5;

    // range is distance radius, angle is the field of view angle
    private float speed = 7, range = 10, angle = 90;
    private int score = 0;
    private Vector3 wanderTarget;
    private GameObject target;
    private Rigidbody body;
    private Vector3 velocity;
    private bool isWandering = true;
    public HashSet<GameObject> foodSet;

    public void setValues(float speed, float range, float angle) {
        this.speed = speed;
        this.range = range;
        this.angle = angle;
    }

    private void Start() {
        body = GetComponent<Rigidbody>();
        wanderTarget = transform.position + Constants.posAroundRadius(2, transform.position.y);
        FoodCollisionDetector detector = GetComponentInChildren<FoodCollisionDetector>();
        detector.initialize(range, angle);
    }

    private void FixedUpdate() {
        if (target == null) {
            wander();
        } else {
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            body.MovePosition(transform.position + velocity * Time.deltaTime);
        }
    }

    private void wander() {
        velocity = (transform.forward) * speed;
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
    }

    public void setTarget(GameObject other) {
        Vector3 targetPos = other.transform.position - new Vector3(0, other.transform.position.y - transform.position.y);
        velocity = Constants.dir(transform.position, targetPos) * speed;
        target = other;
        transform.LookAt(target.transform);
    }

    public GameObject getTarget() {
        return target;
    }
    
    private void OnCollisionEnter(Collision other) {
        Debug.Log("Organism OnCollisionEnter: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("food")) {
            foodSet.Remove(other.gameObject);
            Destroy(other.gameObject);
            target = null;
            ++score;
        } else if (other.gameObject.CompareTag("walls")){
            velocity *= -1;
        }
    }

    public static Organism Create(HashSet<GameObject> foods ,GameObject prefab, Transform parent, Vector3 pos, Vector3 dir, float speed, float range, float angle) {
        GameObject gObj = Instantiate(prefab, pos, Quaternion.LookRotation(dir));
        Organism org = gObj.GetComponent<Organism>();
        org.setValues(speed, range, angle);
        org.foodSet = foods;
        return org;
    }

    private void OnDrawGizmos() {
        Vector3 rayA = Constants.dirFromAngle(angle / 2);
        Vector3 rayB = Constants.dirFromAngle(-angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
        Gizmos.DrawLine(transform.position, transform.position + rayA * range);
        Gizmos.DrawLine(transform.position, transform.position + rayB * range);
    }
}
