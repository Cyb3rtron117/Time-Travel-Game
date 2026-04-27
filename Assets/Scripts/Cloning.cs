using System.Collections.Generic;
using UnityEngine;

public class Cloning : MonoBehaviour
{
    public List<GameObject> affectedCubes = new List<GameObject>();
    public List<GameObject> childrenOfAffectedCubes = new List<GameObject>();

    private Timeline cubeTime;

    private Vector3 minusPos = new Vector3(0, -50, 0);

    public GameObject myParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(transform.position.y < -25)
        {
            cubeTime = Timeline.Future;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Past"))
        {
            if(cubeTime == Timeline.Future)
            {
                cubeTime = Timeline.Past;
                CreateCube();

                if (myParent != null)
                {
                    myParent.GetComponent<Cloning>().CreateCube();
                }
            }
            else
            {
                if(affectedCubes.Count == 0)
                {
                    CreateCube();
                }
            }
            
        }

        if(collision.gameObject.CompareTag("Future"))
        {
            if(cubeTime == Timeline.Past)
            {
                cubeTime = Timeline.Future;

                BecomeOriginal();
            }
            if(myParent == null)
            {
                Debug.LogError("Original in future!");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Past"))
        {
            ClearChildren();
            ClearChildrenOfChildren();

        }
    }

    public void CreateCube()
    {
        GameObject instCube = CubePooling.Instance.SpawnFromPool("MovableObj", transform.position + minusPos, Quaternion.identity);

        affectedCubes.Add(instCube);
        instCube.GetComponent<Cloning>().myParent = gameObject;
        if(myParent != null)
        {
            myParent.GetComponent<Cloning>().childrenOfAffectedCubes.Add(instCube);
        }
    }

    public void ClearChildren()
    {
        /*
        if (affectedCubes.Count > 0)
        {
            foreach (GameObject obj in affectedCubes)
            {
                obj.SetActive(false);
                affectedCubes.Remove(obj);
            }
            affectedCubes.Clear();
        }   
        */
        while (affectedCubes.Count > 0)
        {
            affectedCubes[0].SetActive(false);
            affectedCubes.RemoveAt(0);
        }


    }

    public void ClearChildrenOfChildren()
    {
        /*
        if (childrenOfAffectedCubes.Count > 0)
        {
            foreach (GameObject obj in childrenOfAffectedCubes)
            {
                obj.SetActive(false);
                childrenOfAffectedCubes.Remove(obj) ;
            }
            childrenOfAffectedCubes.Clear();
        }
        */

        while (childrenOfAffectedCubes.Count > 0)
        {
            childrenOfAffectedCubes[0].SetActive(false);
            childrenOfAffectedCubes.RemoveAt(0);
        }
    }

    public void BecomeOriginal()
    {
        if (myParent != null)
        {
            int myIndex = myParent.GetComponent<Cloning>().affectedCubes.IndexOf(gameObject);

            while (myParent.GetComponent<Cloning>().affectedCubes.Count > (myIndex + 1))
            {
                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].GetComponent<Cloning>().ClearChildren();
                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].GetComponent<Cloning>().ClearChildrenOfChildren();

                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].gameObject.SetActive(false);
                myParent.GetComponent<Cloning>().affectedCubes.RemoveAt((myIndex + 1));
            }
            /*

                if (myParent.GetComponent<Cloning>().affectedCubes.Count > (myIndex + 1))
            {
                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].GetComponent<Cloning>().ClearChildren();
                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].GetComponent<Cloning>().ClearChildrenOfChildren();

                myParent.GetComponent<Cloning>().affectedCubes[(myIndex + 1)].gameObject.SetActive(false);
                myParent.GetComponent<Cloning>().affectedCubes.RemoveAt((myIndex + 1));
            }*/
        }
    }
}
