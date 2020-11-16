using System.Collections;
using UnityEngine;
using EcoSimUtil;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {
    private GameObject target;
    private IEnumerator wanderNewMovement;
    private bool isWandering = false;
    private bool isRotLeft = false;
    private bool isRotRight = false;
    private bool isWalking = false;
    public float rotSpeed = 100f;
    private Vector3 velocity;

    private Rigidbody body;
    public float moveSpeed = 10f;

    private foodRadarScript radar;
    //Traits
    private float speed = 10, range = 6, angle = 120, rSpeed = 100;

    //Misc
    private EnvStats envStats;
    private int score;
    [SerializeField] LayerMask mask;

    public static Player Create(EnvStats env, GameObject prefab, Vector3 pos, Vector3 forward, OrgStats stats) {
        GameObject p = Instantiate(prefab, pos, Quaternion.LookRotation(
                forward
            ));
        Player playa = p.GetComponent<Player>();
        playa.Init(stats, env);
        return playa;
    }

    public void Init(OrgStats stats, EnvStats envStats) {
        speed = stats.speed; range = stats.range; angle = stats.angle; rSpeed = stats.rSpeed;
        moveSpeed = speed;
        rotSpeed = rSpeed;
        this.envStats = envStats;
        score = 0;
        if (radar == null) {
            Debug.LogError("Radar is Null");
        }
        radar.SetValues(range, angle);
    }
  
    void Start() {
        body = GetComponent<Rigidbody>();
    }

    public void setChild(foodRadarScript foodRadarScript) {
        radar = foodRadarScript;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("walls")) {
            Vector3 dirToReflect = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            transform.LookAt(transform.position + dirToReflect);
            //moveSpeed = moveSpeed * -1;
            //transform.LookAt(transform.position - transform.forward);
        }
        if (collision.gameObject.CompareTag("foodnew")) {
            Destroy(collision.gameObject);
            target = null;
            ++score;
            envStats.DecFood();
        }
    }

    private bool IsPathBlocked() {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.Raycast(ray, 0.5f, mask);
    }

    void FixedUpdate() {
        if (target == null) {
            wander();
        } else {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void wander() {
        if (isWandering == false) {
            StartCoroutine(wanderNew());
        }
        if (isRotRight == true) {
            transform.Rotate(transform.up * Time.deltaTime * rSpeed);
        }
        if (isRotLeft == true) {
            transform.Rotate(transform.up * Time.deltaTime * -rSpeed);
        }
        if (isWalking) {
            if (!IsPathBlocked())
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
            else
                transform.LookAt(transform.position - transform.forward);
        }
    }

    public void setTarget(GameObject other) {
        Vector3 targetPos = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
        transform.LookAt(targetPos);
        target = other;
    }

    public GameObject getTarget() { return target; }

    IEnumerator wanderNew() {
        int rotTime = 1;
        int rotWait = Random.Range(1, 2);
        int rotLorR = Random.Range(1, 3);
        int walkWait = Random.Range(1, 2);
        int walkTime = Random.Range(5, 7);
        isWandering = true;
        yield return null; //new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return null;//new WaitForSeconds(rotWait);
        if (rotLorR == 1) {
            //right
            isRotRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotRight = false;
        }
        if (rotLorR == 2) {
            //left
            isRotLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotLeft = false;
        }
        isWandering = false;
    }

    public int Score() {
        return score;
    }

    public void End() {
        Destroy(this.gameObject);
    }

    public OrgStats Stats() {
        return new OrgStats(angle, range, speed, rSpeed);
    }
}
