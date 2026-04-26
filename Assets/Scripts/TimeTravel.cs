using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimeTravel : MonoBehaviour
{
    private PlayerInputSystem playerInputSys; //input system reference
    public static Timeline CurrentTime = Timeline.Past;
    public GameObject Player;
    [SerializeField] private GameObject PlayerHoldPos;
    public CinemachineCamera cineCam;
    [SerializeField] private float WaitTime = 0.1f;

    public List<GameObject> Cubes = new List<GameObject>();
    public List <GameObject> PastCubes = new List<GameObject>();
    public List<GameObject> FutureCubes = new List<GameObject>();

    private Vector3 minusPos = new Vector3(0, -49.5f, 0);
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
        if(Player != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerInputSys.Player.TimeTravel.WasPressedThisFrame())
        {
            switch(CurrentTime)
            {
                case Timeline.Past:
                    CurrentTime = Timeline.Future;

                    StartCoroutine(MoveCineCam(WaitTime));
                    Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 50, Player.transform.position.z);
                    
                    break;


                case Timeline.Future:
                    CurrentTime = Timeline.Past;

                    StartCoroutine(MoveCineCam(WaitTime));
                    Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 50, Player.transform.position.z);

                    break;
            }
        }

        Cubes = GameObject.FindGameObjectsWithTag("MovableObj").ToList();
        foreach(var c in Cubes)
        {
            if(c.transform.parent != PlayerHoldPos.transform)
            {
                c.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

            if (c.transform.position.y > -30)
            {
                if (!PastCubes.Contains(c))
                {
                    PastCubes.Add(c);
                }
                if (FutureCubes.Contains(c))
                {
                    FutureCubes.Remove(c);
                }
            }
            else if (c.transform.position.y < -30)
            {
                if (!FutureCubes.Contains(c))
                {
                    FutureCubes.Add(c);
                }
                if (PastCubes.Contains(c))
                {
                    PastCubes.Remove(c);

                }
            }
        }

    }

    IEnumerator MoveCineCam(float waitTime)
    {
        cineCam.GetComponent<CinemachinePositionComposer>().Damping.y = 0;
        yield return new WaitForSeconds(waitTime);
        cineCam.GetComponent<CinemachinePositionComposer>().Damping.y = 1;
    }
}

public enum Timeline
{
    Past,
    Future
}


