using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

    public GameObject[] shapes;

    public GameObject[] nextShapes;

    GameObject upNextObject = null;

    int shapeIndex = 0;
    int nextShapeIndex = 0;

    public void SpawnShape()
    {
        shapeIndex = nextShapeIndex;

        Instantiate(shapes[shapeIndex], transform.position, Quaternion.identity);

        nextShapeIndex = Random.Range(0, 7);

        if(upNextObject != null)
        {
            Destroy(upNextObject);
        }

        upNextObject = Instantiate(nextShapes[nextShapeIndex], new Vector3(-10.2f, 15.5f, 0), Quaternion.identity);
    }

	// Use this for initialization
	void Start () {

        nextShapeIndex = Random.Range(0, 7);

        SpawnShape();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
