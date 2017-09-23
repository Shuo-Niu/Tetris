using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shape : MonoBehaviour {

    public static float speed = 1.0f;

    float lastMoveDown = 0;

	// Use this for initialization
	void Start () {
		if(!IsInGrid())
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.gameOver);

            Invoke("OpenGameOverScene", .5f);
        }

        InvokeRepeating("IncreaseSpeed", 2.0f, 2.0f);
	}

    void OpenGameOverScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    void IncreaseSpeed()
    {
        Shape.speed -= .001f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            transform.position += new Vector3(-1, 0, 0);
            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(1, 0, 0);
            } else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            transform.position += new Vector3(1, 0, 0);
            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            } else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Time.time - lastMoveDown >= Shape.speed)
        {
            transform.position += new Vector3(0, -1, 0);
            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0);

                GameBoard.DeleteAllFullRows();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeStop);

                enabled = false;

                FindObjectOfType<ShapeSpawner>().SpawnShape();
            } else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }

            lastMoveDown = Time.time;
        }
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            transform.Rotate(0, 0, 90);
            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, -90);
            } else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateSound);
            }
        }
    }
    
    public bool IsInGrid()
    {

        foreach(Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);

            if (!IsInBorder(vect))
            {
                return false;
            }

            // if the childBlock is in a grid that is not empty and taken by other shapes before, then !IsInGrid()
            if(GameBoard.gameBoard[(int)vect.x, (int)vect.y] != null &&
               GameBoard.gameBoard[(int)vect.x, (int)vect.y].parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsInBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
            (int)pos.x <= 9 &&
            (int)pos.y >= 0 );
    }

    public Vector2 RoundVector(Vector2 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

    public void UpdateGameBoard()
    {
        for(int y = 0; y < 20; ++y)
        {
            for(int x = 0; x < 10; ++x)
            {
                // make all grids in this shape null, meaning to clean the previous position
                if(GameBoard.gameBoard[x, y] != null &&
                   GameBoard.gameBoard[x, y].parent == transform)
                {
                    GameBoard.gameBoard[x, y] = null;
                }
            }
        }

        foreach(Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);

            // set all grids in this shape
            GameBoard.gameBoard[(int)vect.x, (int)vect.y] = childBlock;

            Debug.Log("Cube at: (" + vect.x + ", " + vect.y + ")");
        }

        // GameBoard.PrintArray();
    }
}
