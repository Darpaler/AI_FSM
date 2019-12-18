using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
    // Variables
    public bool canSee;
    [SerializeField]
    private float fovAngle = 45f;
    [SerializeField] private float turnSpeed = 10f;
    public GameObject test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sight();
    }

    protected void Sight()
    {
        if (!canSee) { return; }

        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector3 vectorToLookDown = (mousePosition - transform.position);
        vectorToLookDown.Normalize();

        Debug.Log(Vector3.Dot(-(mousePosition - transform.position), -transform.forward));

        // If the collider is within our field of view
        if (Vector3.Dot((mousePosition - transform.position), -transform.up) >= Mathf.Cos(fovAngle))
        {
            Debug.Log("can see");
            //Face That Point
            transform.up = -vectorToLookDown;
        }
    }

    public void FaceMouse()
    {

    }
}
