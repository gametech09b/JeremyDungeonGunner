using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    /// <summary>
    /// Builds a path for the room, from the startGridPosition to the endGridPosition, and adds
    /// movement steps to the returned Stack. Returns null if no path could be found.
    /// </summary>
    public static Stack<Vector3> BuildPath(Room room, Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        // adjust positions by lower bounds
        startGridPosition -= (Vector3Int)room.templateLowerBounds;
        endGridPosition -= (Vector3Int)room.templateLowerBounds;

        // Create open list and closed hashset
        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();

        // Create gridnodes for pathfinding
        GridNodes gridNodes = new GridNodes(room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y+ 1);

        Node startNode = gridNodes.GetGridNode(startGridPosition.x, startGridPosition.y);
        Node targetNode = gridNodes.GetGridNode(endGridPosition.x, endGridPosition.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, room.instantiatedRoom);

        if (endPathNode != null)
        {
            return CreatePathStack(endPathNode,room);
        }

        return null;
    }

    /// <summary>
    /// Finds the shortest path - returns the end node if a path is found, otherwise returns null
    /// </summary>
    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        //add start node to open list
        openNodeList.Add(startNode);

        //loop through open list until it is empty
        while (openNodeList.Count > 0)
        {
            //sort list
            openNodeList.Sort();

            //current node = the node in the open list with the lowest f cost
            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);

            //if the current node is the target node
            if (currentNode == targetNode)
            {
                return currentNode;
            }

            //add current node to closed list
            closedNodeHashSet.Add(currentNode);

            //evaluate fcost of each neighbour of the current node
            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, instantiatedRoom);
        }

        return null;

    }

    /// <summary>
    /// Create a Stack<vector3> containing the movement path
    /// </summary>
    private static Stack<Vector3> CreatePathStack(Node targetnode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetnode;

        //get mid pointof cell
        Vector3 cellMidPoint = room.instantiatedRoom.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while (nextNode != null)
        {
            // convert grid position to world position
            Vector3 worldPosition = room.instantiatedRoom.grid.CellToWorld(new Vector3Int(nextNode.gridPosition.x + room.templateLowerBounds.x, nextNode.gridPosition.y + room.templateLowerBounds.y, 0));

            //set world position to mid point of cell
            worldPosition += cellMidPoint;

            movementPathStack.Push(worldPosition);

            nextNode = nextNode.parentNode;
        }

        return movementPathStack;
    }

    /// <summary>
    /// Evaluate the current node neighbours
    /// </summary>
    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        Vector2Int currentNodeGridPosition = currentNode.gridPosition;

        Node validNeighbourNode;

        // loop through all directions
        for (int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    // skip current node
                    continue;
                }
                validNeighbourNode = GetValidNodeNeighbour(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j, gridNodes, closedNodeHashSet, instantiatedRoom);

                    if (validNeighbourNode != null)
                    {
                        //calculate g cost
                        int newCostToNeighbour;

                        int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[validNeighbourNode.gridPosition.x, validNeighbourNode.gridPosition.y];

                        newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);

                        bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode);

                        if (newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList)
                        {
                            validNeighbourNode.gCost = newCostToNeighbour;
                            validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                            validNeighbourNode.parentNode = currentNode;

                            if (!isValidNeighbourNodeInOpenList)
                            {
                                openNodeList.Add(validNeighbourNode);
                            }
                        }
                }
            }
        }
    }

    /// <summary>
    /// return the distance between two nodes
    /// </summary>
    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    /// <summary>
    /// Evaluate a nieghbour node at nieghbourNodeXposition and neighbourNodeYposition using the
    /// specified gridnodes, closedNodeHashSet and instantiatedRoom. Returns null if the node is invalid
    /// </summary>
    private static Node GetValidNodeNeighbour(int neighbourNodeXposition, int neighbourNodeYposition, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        // if neighbour node position is beyond grid then return null
        if(neighbourNodeXposition >= instantiatedRoom.room.templateUpperBounds.x - instantiatedRoom.room.templateLowerBounds.x || neighbourNodeXposition < 0 || neighbourNodeYposition >= instantiatedRoom.room.templateUpperBounds.y - instantiatedRoom.room.templateLowerBounds.y || neighbourNodeYposition < 0)
        {
            return null;
        }

        // get neighbour node
        Node neighbourNode = gridNodes.GetGridNode(neighbourNodeXposition, neighbourNodeYposition);

        int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[neighbourNodeXposition, neighbourNodeYposition];

        // if neighbour node is in the closed list then skip
        if (movementPenaltyForGridSpace == 0 || closedNodeHashSet.Contains(neighbourNode))
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }
    }

}
