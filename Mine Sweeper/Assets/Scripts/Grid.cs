using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public static Element[,] elements = new Element[GameManager.width, GameManager.height];
    public static bool[,] visited = new bool[GameManager.width, GameManager.height];
    public static bool[,] flagged = new bool[GameManager.width,GameManager.height];
    public static bool isOver = false;
    public static void UncoverMines()
    {
        foreach (Element elem in elements)
        {
            if (elem.mine)
            {
                elem.LoadTexture(0);
            }
        }
        isOver = true;
    }
    public static bool MineAt(int x,int y)
    {
        if (x>=0&&y>=0&&x<GameManager.width&&y<GameManager.height)
        {
            return elements[x, y].mine;
        }
        return false;
    }
    public static int AdjacentMines(int x, int y)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (MineAt(x+i,y+j))
                {
                    count++;
                }
            }
        }
        return count;
    }
    public static void FFUncover(int x,int y,bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < GameManager.width && y < GameManager.height)
        {
            if (visited[x, y])
            {
                return;
            }
            visited[x, y] = true;
            if (elements[x,y].IsCovered())
            {
                if (elements[x, y].mine)
                {
                    UncoverMines();
                }
                else
                {
                    elements[x, y].LoadTexture(AdjacentMines(x, y));
                }
            }
            if (AdjacentMines(x,y)>0)
            {
                return;
            }
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Grid.FFUncover(x+i,y+j,visited);
                }
            }
        }
    }
    public static bool IsWon()
    {
        foreach (Element elem in elements)
        {
            if (elem.IsCovered() && !elem.mine)
            {
                return false;
            }
        }
        isOver = true;
        return true;
    }
    public static bool IsFlaggedAround(int x,int y)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (x+i >= 0 && y+j >= 0 && x+i < GameManager.width && y+j < GameManager.height)
                {
                    if (flagged[x + i, y + j])
                    {
                        count++;
                    }
                }
            }
        }
        if (count == AdjacentMines(x,y))
        {
            return true;
        }
        return false;
    }
    public static void CustomFFUncover(int x, int y, bool[,] visited)
    {

            visited[x, y] = true;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Grid.FFUncover(x + i, y + j, visited);
                }
            }
    }
}
