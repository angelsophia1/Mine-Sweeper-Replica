using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Element : MonoBehaviour
{
    public bool mine;
    public Sprite[] emptyTextures;
    public Sprite mineTexture,flagTexture,brickTexture;
    private int x, y;
    private bool flagged = false, countTime = false,leftPressed = false,rightPressed = false;
    private float multiPressTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        mine = Random.value < 0.15;
        flagged = false;
        countTime = false;
        leftPressed = false;
        rightPressed = false;
        x = (int)Mathf.Round(transform.position.x);
        y = (int)Mathf.Round(transform.position.y);
        Grid.elements[x, y] = this;
        Grid.visited[x, y] = false;
        Grid.flagged[x, y] = false;
    }
    private void Update()
    {
        if (countTime)
        {
            if (multiPressTime > 0)
            {
                multiPressTime -= Time.deltaTime;
                if (leftPressed&&rightPressed)
                {
                    CustomFFUncover();
                    multiPressTime = 0.1f;
                    countTime = false;
                    leftPressed = false;
                    rightPressed = false;
                }
            }else{
                multiPressTime = 0.1f;
                countTime = false;
                leftPressed = false;
                rightPressed = false;
            }
        }
    }
    public void LoadTexture(int adjacentCount)
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }
    public bool IsCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "Brick";
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)&&!Grid.isOver)
        {
            if (mine && !flagged)
            {
                Grid.UncoverMines();
                FindObjectOfType<GameManager>().DisplayRestart();
            }
            else
            {
                if (!flagged)
                {
                    LoadTexture(Grid.AdjacentMines(x, y));
                    if (Grid.AdjacentMines(x, y) < 1)
                    {
                        Grid.FFUncover(x, y, Grid.visited);
                    }
                }
                if (Grid.IsWon())
                {
                    FindObjectOfType<GameManager>().DisplayRestart();
                }
                if (new string[] {"1","2","3","4","5","6","7","8" }.Contains(GetComponent<SpriteRenderer>().sprite.texture.name))
                {
                    countTime = true;
                    leftPressed = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !Grid.isOver)
        {
            if (IsCovered())
            {
                GetComponent<SpriteRenderer>().sprite = flagTexture;
                flagged = true;
                Grid.flagged[x, y] = true;
            }
            else if (flagged)
            {
                GetComponent<SpriteRenderer>().sprite = brickTexture;
                flagged = false;
                Grid.flagged[x, y] = false;
            } else if (new string[] { "1", "2", "3", "4", "5", "6", "7", "8" }.Contains(GetComponent<SpriteRenderer>().sprite.texture.name))
            {
                countTime = true;
                rightPressed = true;
            }
        }
        if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1) && !Grid.isOver)
        {
            CustomFFUncover();
        }
    }
    void CustomFFUncover()
    {
        if (Grid.IsFlaggedAround(x, y))
        {
            Grid.CustomFFUncover(x, y, Grid.visited);
            if (Grid.isOver)
            {
                FindObjectOfType<GameManager>().DisplayRestart();
            }
            if (Grid.IsWon())
            {
                FindObjectOfType<GameManager>().DisplayRestart();
            }
        }
    }
}
