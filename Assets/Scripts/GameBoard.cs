using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour {

    public static Transform[,] gameBoard = new Transform[10, 20];

    /*public static void PrintArray()
    {
        string arrayOutput = "";

        int iMax = gameBoard.GetLength(0) - 1;
        int jMax = gameBoard.GetLength(1) - 1;

        for(int j = jMax; j >= 0; j--)
        {
            for(int i = 0; i <= iMax; i++)
            {
                if(gameBoard[i,j] == null)
                {
                    arrayOutput += "N ";
                } else
                {
                    arrayOutput += "O ";
                }
            }
            arrayOutput += "\n";
        }

        var myArrayComp = GameObject.Find("MyArray").GetComponent<Text>();
        myArrayComp.text = arrayOutput;
    }*/

    public static void DeleteAllFullRows()
    {
        int addScore = 0;

        int times = 1;

        for (int row = 19; row >= 0; --row)
        {
            if (IsRowFull(row))
            {
                DeleteGBRow(row);

                addScore += 10 * (times++);

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rowDelete);
            }
        }

        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score += addScore;

        textUIComp.text = score.ToString();
    }

    public static bool IsRowFull(int row)
    {
        for(int col = 0; col < 10; ++col)
        {
            if(gameBoard[col,row] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static void DeleteGBRow(int row)
    {
        for(int col = 0; col < 10; ++col)
        {
            Destroy(gameBoard[col, row].gameObject);
            gameBoard[col, row] = null;
        }

        row++;

        for(int j = row; j < 20; ++j)
        {
            for(int col = 0; col < 10; ++col)
            {
                if(gameBoard[col,j] != null)
                {
                    gameBoard[col, j - 1] = gameBoard[col, j];
                    gameBoard[col, j] = null;
                    gameBoard[col, j - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
