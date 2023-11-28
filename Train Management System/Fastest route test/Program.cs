using Fastest_route_test;

#region just in case
/*List<Dictionary<int, int>> nodeList = new List<Dictionary<int, int>>() { //Dictionary: railwayId, length
    new Dictionary<int, int>() { //0
        { 1, 6 },
        { 4, 6 }
    },
    new Dictionary<int, int>() { //1
        { 2, 7 },
        { 3, 12 }
    },
    new Dictionary<int, int>() { //2
        { 2, 7 },
        { 4, 6 },
        { 5, 4 },
        { 13, 9 }
    },
    new Dictionary<int, int>() { //3
        { 5, 4 },
        { 8, 7 },
        { 12, 8 }
    },
    new Dictionary<int, int>() { //4
        { 3, 12 },
        { 7, 5 },
        { 6, 6 }
    },
    new Dictionary<int, int>() { //5
        { 7, 5 }
    },
    new Dictionary<int, int>() { //6
        { 6, 6 },
        { 8, 7 },
        { 11, 6 },
        { 10, 7 },
        { 9, 7 }
    },
    new Dictionary<int, int>() { //7
        { 11, 6 },
        { 12, 8 },
        { 13, 9 },
        { 14, 5 }
    },
    new Dictionary<int, int>() { //8
        { 5, 14 },
        { 15, 4 }
    },
    new Dictionary<int, int>() { //9
        { 9, 7 }
    },
    new Dictionary<int, int>() { //10
        { 10, 7 }
    },
    new Dictionary<int, int>() { //11
        { 15, 4 }
    },
};*/


/*List<Dictionary<int, Railway>> nodeList = new List<Dictionary<int, Railway>>() { //Dictionary: stationId, Railway{id, length}
    new Dictionary<int, Railway>() { //0
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //1
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //2
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //3
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //4
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //5
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //6
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //7
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //8
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //9
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //10
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },
    new Dictionary<int, Railway>() { //11
        { 0, new Railway() {Id = 0, Length = 0} },
        { 0, new Railway() {Id = 0, Length = 0} }
    },

};*/

#endregion

Dictionary<int, List<Railway>> nodeListDefault = new Dictionary<int, List<Railway>>() { //starting node id, list<railway(railwayid, endingNodeId, length)>
    {0, new List<Railway>() {
            new Railway() {Id = 1, EndingStationId = 2, Length = 6 },
            new Railway() {Id = 4, EndingStationId = 1, Length = 6 },
        } 
    },
    {1, new List<Railway>() {
            new Railway() {Id = 4, EndingStationId = 0, Length = 6 },
            new Railway() {Id = 2, EndingStationId = 2, Length = 7 },
            new Railway() {Id = 3, EndingStationId = 4, Length = 12 },
        }
    },
    {2, new List<Railway>() {
            new Railway() {Id = 2, EndingStationId = 1, Length = 7 },
            new Railway() {Id = 1, EndingStationId = 0, Length = 6 },
            new Railway() {Id = 5, EndingStationId = 3, Length = 4 },
            new Railway() {Id = 13, EndingStationId = 7, Length = 9 },
        }
    },
    {3, new List<Railway>() {
            new Railway() {Id = 5, EndingStationId = 2, Length = 4 },
            new Railway() {Id = 8, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 12, EndingStationId = 7, Length = 8 },
        }
    },
    {4, new List<Railway>() {
            new Railway() {Id = 3, EndingStationId = 1, Length = 12 },
            new Railway() {Id = 6, EndingStationId = 6, Length = 6 },
            new Railway() {Id = 7, EndingStationId = 5, Length = 5 },
        }
    },
    {5, new List<Railway>() {
            new Railway() {Id = 7, EndingStationId = 4, Length = 5 },
        }
    },
    {6, new List<Railway>() {
            new Railway() {Id = 6, EndingStationId = 4, Length = 6 },
            new Railway() {Id = 8, EndingStationId = 3, Length = 7 },
            new Railway() {Id = 9, EndingStationId = 9, Length = 7 },
            new Railway() {Id = 10, EndingStationId = 10, Length = 7 },
            new Railway() {Id = 11, EndingStationId = 7, Length = 6 },
        }
    },
    {7, new List<Railway>() {
            new Railway() {Id = 11, EndingStationId = 6, Length = 6 },
            new Railway() {Id = 12, EndingStationId = 3, Length = 8 },
            new Railway() {Id = 13, EndingStationId = 2, Length = 9 },
            new Railway() {Id = 14, EndingStationId = 8, Length = 5 },
        }
    },
    {8, new List<Railway>() {
            new Railway() {Id = 14, EndingStationId = 7, Length = 5 },
            new Railway() {Id = 15, EndingStationId = 11, Length = 4 },
        }
    },
    {9, new List<Railway>() {
            new Railway() {Id = 9, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 16, EndingStationId = 10, Length = 9 },
        }
    },
    {10, new List<Railway>() {
            new Railway() {Id = 10, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 16, EndingStationId = 9, Length = 9 },
        }
    },
    {11, new List<Railway>() {
            new Railway() {Id = 15, EndingStationId = 8, Length = 5 },
        }
    }
};

Dictionary<int, List<Railway>> nodeList = new Dictionary<int, List<Railway>>() { //starting node id, list<railway(railwayid, endingNodeId, length)>
    {0, new List<Railway>() {
            new Railway() {Id = 1, EndingStationId = 2, Length = 6 },
            new Railway() {Id = 4, EndingStationId = 1, Length = 6 },
        }
    },
    {1, new List<Railway>() {
            new Railway() {Id = 4, EndingStationId = 0, Length = 6 },
            new Railway() {Id = 2, EndingStationId = 2, Length = 7 },
            new Railway() {Id = 3, EndingStationId = 4, Length = 12 },
        }
    },
    {2, new List<Railway>() {
            new Railway() {Id = 2, EndingStationId = 1, Length = 7 },
            new Railway() {Id = 1, EndingStationId = 0, Length = 6 },
            new Railway() {Id = 5, EndingStationId = 3, Length = 4 },
            new Railway() {Id = 13, EndingStationId = 7, Length = 9 },
        }
    },
    {3, new List<Railway>() {
            new Railway() {Id = 5, EndingStationId = 2, Length = 4 },
            new Railway() {Id = 8, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 12, EndingStationId = 7, Length = 8 },
        }
    },
    {4, new List<Railway>() {
            new Railway() {Id = 3, EndingStationId = 1, Length = 12 },
            new Railway() {Id = 6, EndingStationId = 6, Length = 6 },
            new Railway() {Id = 7, EndingStationId = 5, Length = 5 },
        }
    },
    {5, new List<Railway>() {
            new Railway() {Id = 7, EndingStationId = 4, Length = 5 },
        }
    },
    {6, new List<Railway>() {
            new Railway() {Id = 6, EndingStationId = 4, Length = 6 },
            new Railway() {Id = 8, EndingStationId = 3, Length = 7 },
            new Railway() {Id = 9, EndingStationId = 9, Length = 7 },
            new Railway() {Id = 10, EndingStationId = 10, Length = 7 },
            new Railway() {Id = 11, EndingStationId = 7, Length = 6 },
        }
    },
    {7, new List<Railway>() {
            new Railway() {Id = 11, EndingStationId = 6, Length = 6 },
            new Railway() {Id = 12, EndingStationId = 3, Length = 8 },
            new Railway() {Id = 13, EndingStationId = 2, Length = 9 },
            new Railway() {Id = 14, EndingStationId = 8, Length = 5 },
        }
    },
    {8, new List<Railway>() {
            new Railway() {Id = 14, EndingStationId = 7, Length = 5 },
            new Railway() {Id = 15, EndingStationId = 11, Length = 4 },
        }
    },
    {9, new List<Railway>() {
            new Railway() {Id = 9, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 16, EndingStationId = 10, Length = 9 },
        }
    },
    {10, new List<Railway>() {
            new Railway() {Id = 10, EndingStationId = 6, Length = 7 },
            new Railway() {Id = 16, EndingStationId = 9, Length = 9 },
        }
    },
    {11, new List<Railway>() {
            new Railway() {Id = 15, EndingStationId = 8, Length = 5 },
        }
    }
};

foreach (KeyValuePair<int, List<Railway>> railwayList in nodeList) //sort nodeList by lengths of connecting railways (with [0] being the smallest
    railwayList.Value.Sort((x, y) => x.Length.CompareTo(y.Length));

foreach (KeyValuePair<int, List<Railway>> railwayList in nodeListDefault)
    railwayList.Value.Sort((x, y) => x.Length.CompareTo(y.Length));


//Dictionary<int, List<Railway>> nodeList = new Dictionary<int, List<Railway>>(nodeListDefault); //copy default nodeList (as in this one stuff will be removed)




Dictionary<int, List<Railway>> smallestDistanceRoute = new Dictionary<int, List<Railway>>() {}; //per node the smallest distance route from the start saved as a list of railways

List<Railway> nextNodeRailwayRoute = new List<Railway>();



const int startNode = 0;
const int endNode = 10;
int smallestPathToEndNode = int.MaxValue;
//nodeList[endNode].Clear();
searchNode(startNode);

void searchNode(int nodeId)
{
    int currentPathLength = 0;
    foreach (Railway route in nextNodeRailwayRoute)
        currentPathLength += route.Length;
    
    int nextNode;

    if (nodeList[0].Count == 0 && nodeId == 5)
        Console.WriteLine("hol up");

    if (currentPathLength >= smallestPathToEndNode)
    {
        nodeList[nodeId].Clear();
        nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch
        nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; // nodeList[nodeId][0].EndingStationId; //set nextNode as ending node from last 
        searchNode(nextNode);
    }

    if (!smallestDistanceRoute.ContainsKey(nodeId))
    { //if first time searching node
        nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

        smallestDistanceRoute[nodeId] = new List<Railway>(nextNodeRailwayRoute.ToList()); //copy currentRailwayRoute
        smallestDistanceRoute[nodeId].RemoveAt(smallestDistanceRoute[nodeId].Count - 1); //set the smallestdistanceRoute from current node to the previous node of the current/next railwayroute

        if (nodeId == endNode)
        { // if end node

            int smallestPathLength = 0;
            foreach (Railway route in smallestDistanceRoute[nodeId])
                smallestPathLength += route.Length;

            smallestPathToEndNode = smallestPathLength;

            nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch
            nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch


            nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last 

            //nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest)
        } else
        {
            nextNode = nodeList[nodeId][0].EndingStationId; //save nextNode before deleting to be used in recursive function
            nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest)
        }


        searchNode(nextNode);
    }
    else if (smallestDistanceRoute.ContainsKey(nodeId))
    { //if node previously found
        int smallestPathLength = 0;
        foreach (Railway route in smallestDistanceRoute[nodeId])
            smallestPathLength += route.Length;


        if (currentPathLength > smallestPathLength)
        { //if blocked ∞∞
/*            if (nodeList[nodeId].Count != 0)
            {
                if (currentPathLength + nodeList[nodeId][0].Length >= smallestPathToEndNode)
                { //if next to search length is bigger than the smallest path to end node
                    nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                    nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last 

                    nodeList[nodeId].Clear(); //remove all paths from current node

                    searchNode(nextNode);
                }
            }*/

            nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

            nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; // nodeList[nodeId][0].EndingStationId; //set nextNode as ending node from last 

            searchNode(nextNode);          
        }
        else if (currentPathLength < smallestPathLength)
        { // if currentpath length is smaller than already exisitng smallest path to this node
            if (nodeId == endNode)
            {
                smallestPathToEndNode = currentPathLength;

                nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last 

                //nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                searchNode(nextNode);

            }
            nodeList[nodeId] = nodeListDefault[nodeId].ToList(); // refresh nodes
            
            if (currentPathLength + nodeList[nodeId][0].Length >= smallestPathToEndNode)
            { //if next to search length is bigger than the smallest path to end node

                nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last 

                nodeList[nodeId].Clear(); //remove all paths from current node

                searchNode(nextNode);
            } else
            {
                smallestDistanceRoute[nodeId] = new List<Railway>(nextNodeRailwayRoute.ToList()); //copy currentRailwayRoute

                nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

                nextNode = nodeList[nodeId][0].EndingStationId; //save nextNode before deleting to be used in recursive function

                nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                searchNode(nextNode);
            }
        }
        else if (currentPathLength == smallestPathLength)
        { //if not blocked
            if (nodeList[nodeId].Count != 0)
            { //if still has paths
                if (currentPathLength + nodeList[nodeId][0].Length >= smallestPathToEndNode)
                { //if next to search length is bigger than the smallest path to end node
                    nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                    nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last 

                    nodeList[nodeId].Clear(); //remove all paths from current node

                    searchNode(nextNode);
                }
                else
                {
                    nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

                    nextNode = nodeList[nodeId][0].EndingStationId; //save nextNode before deleting to be used in recursive function

                    nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                    searchNode(nextNode);
                }
            }
            else
            { // if no more paths
                if (nodeId != startNode)
                {
                    nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                    if (nextNodeRailwayRoute.Count != 0)
                        nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndingStationId; //set nextNode as ending node from last currentRailwayRoute
                    else
                        nextNode = startNode;


                    searchNode(nextNode);
                } else
                {
                    return;
                    //fucking quit this shit
                    //basically the ending // do absolutely nothing
                }
            }
        }
    }
    return;
    //what the fuck i'm so confused
}

//TODO finish literally all of this
Console.Read();