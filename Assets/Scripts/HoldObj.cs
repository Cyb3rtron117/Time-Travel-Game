using UnityEngine;

public class HoldObj : MonoBehaviour
{
    private PlayerInputSystem playerInputSys; //input system reference
    public GameObject grabbedObj = null;
    public GameObject holdPos;
    public BoxCollider2D holdCollider;

    void Awake()
    {
        playerInputSys = new PlayerInputSystem(); //initialising the input system

    }
    void OnEnable()
    {
        playerInputSys.Enable(); //needed for the input system
    }
    void OnDisable()
    {
        playerInputSys.Disable(); //needed for the input system
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdCollider.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (playerInputSys.Player.Grab.WasPressedThisFrame())
        {
            //print("pressed");
            grabbedObj = GetClosestCollider(gameObject.GetComponent<CircleCollider2D>().radius);
        }

        if(playerInputSys.Player.Grab.IsPressed() && grabbedObj != null)
        {
            if (grabbedObj.transform.parent != holdPos.transform)
            {
                grabbedObj.transform.SetParent(holdPos.transform);
                grabbedObj.transform.localScale = Vector3.one;
                grabbedObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                holdCollider.enabled = true;

            }
            grabbedObj.transform.localPosition = Vector3.zero;
        }
        if(playerInputSys.Player.Grab.WasReleasedThisFrame() && grabbedObj != null)
        {
            holdCollider.enabled = false;
            grabbedObj.transform.SetParent(null);
            grabbedObj.transform.localScale = Vector3.one;
            grabbedObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            grabbedObj = null;
        }
    }

    public GameObject GetClosestCollider(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("MovableObjects"));
        GameObject closest = null;
        float minSqrDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            // Calculate squared distance to the collider's position
            float sqrDistance = (col.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closest = col.gameObject;
            }
        }
        return closest;
    }
}
