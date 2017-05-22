using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public float nodeRadius = 1f;
    public LayerMask unwalkableMask;

    public Node[,] nodes;
    public List<Node> path;

    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    private Vector3 scale;
    private Vector3 halfScale;

	// Use this for initialization
	void Start () {
        CreateGrid();
	}
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        if(nodes != null)
        {
            //Loop thru all nodes
            for(int x = 0; x < nodes.GetLength(0);x++)
            {
                for (int z =0; z< nodes.GetLength(1);z++)
                {
                    //Get the node and store it in variable
                    Node node = nodes[x, z];
                    //Blue color
                    //Red color
                    Gizmos.color = node.walkable ? new Color(0, 0, 1, 0.5f) :
                                                   new Color(1, 0, 0, 0.5f);

                    if (path != null && path.Contains(node))
                        Gizmos.color = Color.black;

                    //Draw a sphere to represent the node
                    Gizmos.DrawSphere(node.position, nodeRadius);

                }
            }
        }
    }

    //Generates a 2D grid on the X and Z axis
    public void CreateGrid()
    {
        //Calculate the node diameter
        nodeDiameter = nodeRadius * 2f;

        //Get transform's scale
        scale = transform.localScale;

        //Half the scale
        halfScale = scale / 2f;

        gridSizeX = Mathf.RoundToInt(scale.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(scale.z / nodeDiameter);

        //Create a grid of that size
        nodes = new Node[gridSizeX, gridSizeZ];

        //Get the bottom left point of the position
        Vector3 bottomLeft = transform.position - Vector3.right * halfScale.x
                                                - Vector3.forward * halfScale.z;
        //Loop through all nodes in grid
        for (int x =0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                // Calculate offset for x and z
                float xOffset = x * nodeDiameter + nodeRadius;
                float zOffset = z * nodeDiameter + nodeRadius;
                // Create position using offsets
                Vector3 nodePoint = bottomLeft + Vector3.right * xOffset
                                               + Vector3.forward * zOffset;
                //Use Physics to check if node collided with non-walkable object
                bool walkable = !Physics.CheckSphere(nodePoint, nodeRadius, unwalkableMask);
                //Create the node and put it in the 2D array
                nodes[x, z] = new Node(walkable, nodePoint,x,z);
            }
        }


    }
    //Check the nodes to see if they are walkable or not
    void CheckWalkable()
    {
        //Loop through all the nodes
        for(int x=0; x < nodes.GetLength(0); x++)
        {
            for (int z =0; z < nodes.GetLength(1); z++)
            {
                // Grabbing node at index
                Node node = nodes[x, z];
                //Check physics if we have collided with non-walkable
                node.walkable = !Physics.CheckSphere(node.position, nodeRadius, unwalkableMask);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>

    public Node GetNodeFromPosition(Vector3 position)
    {
        // Calculate perdcentage of grid position
        float percentX = (position.x + halfScale.x) / scale.x;
        float percentZ = (position.z + halfScale.z) / scale.z;

        //Clamp percentage to a 0-1 ratio
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        //Translate percentage to x & z coordinates
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        //Return the node at translated coordinate
        return nodes[x, z];
    }
	
    //This function will return all neighbours surrounding node
    public List<Node> GetNeighbours(Node node)
    {
        //Make a ne list of neighbours
        List<Node> neighbours = new List<Node>();

        //Try and look at the surrounding neighbours
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                //Check if the coordinate is the current node
                if (x == 0 && z == 0)
                    continue; //Skip that node
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                //Check if the index is not out of bounds
                if (checkX >= 0 &&
                   checkX < gridSizeX &&
                   checkZ >= 0 &&
                   checkZ < gridSizeZ)
                {
                    //Add neighbour to list
                    neighbours.Add(nodes[checkX, checkZ]);

                }

            }

        }
        return neighbours;
    }
	// Update is called once per frame
	void Update () {
        CheckWalkable();	
	}
}
