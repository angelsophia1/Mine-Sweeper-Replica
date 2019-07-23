using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject brickPrefab,restartButton;
    public static int width = 20, height = 20;
    private void Awake()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(brickPrefab,new Vector3(x,y,0),Quaternion.identity);
            }
        }
    }
    public void DisplayRestart()
    {
        restartButton.SetActive(true);
    }
    public void Restart()
    {
        Grid.isOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
