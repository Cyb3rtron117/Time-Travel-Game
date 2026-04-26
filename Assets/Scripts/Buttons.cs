using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public List<GameObject> linkedObjects = new List<GameObject>();
    [SerializeField] private List<Animator> linkedAnim = new List<Animator>();
    public ObjectType linkedObjectType = ObjectType.None;

    private List<GameObject> objectsOnTop = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectsOnTop.Clear();
        linkedAnim.Clear();
        foreach (GameObject obj in linkedObjects)
        {
            linkedAnim.Add(obj.GetComponent<Animator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MovableObj"))
        {
            if(!objectsOnTop.Contains(collision.gameObject))
            {
                objectsOnTop.Add(collision.gameObject);
            }
            switch (linkedObjectType)
            {
                case ObjectType.None:
                    break;
                case ObjectType.Door:
                    foreach(Animator anim  in linkedAnim)
                    {
                        anim.SetBool("Open", true);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MovableObj"))
        {
            if (objectsOnTop.Contains(collision.gameObject))
            {
                objectsOnTop.Remove(collision.gameObject);
            }
            if (objectsOnTop.Count == 0)
            {
                switch (linkedObjectType)
                {
                    case ObjectType.None:
                        break;
                    case ObjectType.Door:
                        foreach (Animator anim in linkedAnim)
                        {
                            anim.SetBool("Open", false);
                        }
                        break;
                }
            }
        }
    }
}


public enum ObjectType
{
    None,
    Door,

}