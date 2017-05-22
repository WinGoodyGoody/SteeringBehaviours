using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //Member variables(class variable)
    public bool walkable;
    public Vector3 position;
    public int gridX;
    public int gridZ;
    public Node parent;

    public int gCost;//distance to star node
    public int hCost;//Heuristic
    public int fCost
    {
    get
        {
        return gCost + hCost;
        }
}
    //Constructor
    public Node(bool walkable, Vector3 position, int gridX, int gridZ)
    {
        this.walkable = walkable;
        this.position = position;
        this.gridX = gridX;
        this.gridZ = gridZ;

    }

}

    