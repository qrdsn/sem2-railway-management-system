using Interface;
using Interface.Journey;
using Logic.Railway;

namespace Logic.Journey
{
    public class JourneyContainer
    {
        private IJourneyContainerDAL _journeyDAL;

        public JourneyContainer(IJourneyContainerDAL journeyDAL)
        {
            _journeyDAL = journeyDAL;
        }

        public List<JourneyModel> GetAllJourneys()
        {
            List<JourneyDTO> journeyDTOList = _journeyDAL.GetJourneys();
            if (journeyDTOList == null)
                return null;

            List<JourneyModel> journeyList = new List<JourneyModel>();

            foreach (JourneyDTO journeyDTO in journeyDTOList)
            {
                if (journeyDTO != null)
                {
                    journeyList.Add(new JourneyModel(journeyDTO));
                }
            }

            return journeyList;
        }

        public JourneyModel FindJourneyById(int id)
        {
            return new JourneyModel(_journeyDAL.FindById(id));
        }

        public bool InsertJourney(JourneyModel journey)
        {
            JourneyDTO journeyDTO = new JourneyDTO() { RailwayId = journey.RailwayId, TrainId = journey.TrainId, StartStationId = journey.StartStationId, DepartureTime = journey.DepartureTime, ArrivalTime = journey.ArrivalTime, State = journey.State, AdjustedDepartureTime = journey.AdjustedDepartureTime, AdjustedArrivalTime = journey.AdjustedArrivalTime };

            if (_journeyDAL.InsertJourney(journeyDTO) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteJourney(int journeyId)
        {
            if (_journeyDAL.DeleteJourney(journeyId) == 1 && _journeyDAL.DisableLinkedTickets(journeyId) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// finds best route from start to end station and returns a list of best suited railways
        /// </summary>
        /// <param name="startStationId"></param>
        /// <param name="endStationId"></param>
        /// <param name="nodeListDefault"></param>
        /// <returns></returns>
        public List<RailwayModel> FindBestRoute(int startStationId, int endStationId, Dictionary<int, List<RailwayModel>> nodeListDefault)
        {
            foreach (KeyValuePair<int, List<RailwayModel>> railwayList in nodeListDefault) //sort nodeListDefault by lengths of connecting railways (with [0] being the smallest
                railwayList.Value.Sort((x, y) => x.Length.CompareTo(y.Length));


            Dictionary<int, List<RailwayModel>> nodeList = new Dictionary<int, List<RailwayModel>>();
            foreach (KeyValuePair<int, List<RailwayModel>> keyPair in nodeListDefault) //deeply deep² copies the node dictionary so this stuff can be removed n stuff
                nodeList.Add(keyPair.Key, keyPair.Value.ConvertAll(railway => new RailwayModel() { RailwayId = railway.RailwayId, Length = railway.Length, EndStationId = railway.EndStationId, StartStationId = railway.StartStationId }));


            Dictionary<int, List<RailwayModel>> smallestDistanceRoute = new Dictionary<int, List<RailwayModel>>() { }; //per node the smallest distance route from the start saved as a list of railways

            List<RailwayModel> nextNodeRailwayRoute = new List<RailwayModel>();

            int smallestPathToEndNode = int.MaxValue;

            searchNode(startStationId);

            void searchNode(int nodeId)
            {
                int currentPathLength = 0;
                foreach (RailwayModel route in nextNodeRailwayRoute)
                    currentPathLength += route.Length;

                int nextNode;

                if (nodeList[nodeId].Count > 0)
                {
                    if (nodeList[nodeId][0].EndStationId == nodeId)
                    { //switch end and start station
                        int startS = nodeList[nodeId][0].StartStationId;
                        int endS = nodeList[nodeId][0].EndStationId;
                        nodeList[nodeId][0].EndStationId = startS;
                        nodeList[nodeId][0].StartStationId = endS;
                    }
                }

                if (currentPathLength >= smallestPathToEndNode)
                {
                    nodeList[nodeId].Clear();
                    nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch
                    nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; // nodeList[nodeId][0].EndingStationId; //set nextNode as ending node from last 
                    searchNode(nextNode);
                }

                if (!smallestDistanceRoute.ContainsKey(nodeId))
                { //if first time searching node
                    if (nodeList[nodeId].Count == 0)
                    {
                        searchNode(startStationId);
                        return;
                    }
                    nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

                    smallestDistanceRoute[nodeId] = new List<RailwayModel>(nextNodeRailwayRoute.ToList()); //copy currentRailwayRoute
                    smallestDistanceRoute[nodeId].RemoveAt(smallestDistanceRoute[nodeId].Count - 1); //set the smallestdistanceRoute from current node to the previous node of the current/next railwayroute

                    if (nodeId == endStationId)
                    { // if end node

                        int smallestPathLength = 0;
                        foreach (RailwayModel route in smallestDistanceRoute[nodeId])
                            smallestPathLength += route.Length;

                        smallestPathToEndNode = smallestPathLength;

                        nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch
                        nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                        if (nextNodeRailwayRoute.Count == 0)
                        {
                            return;
                        }
                        nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; //set nextNode as ending node from last 

                        //nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest)
                    }
                    else
                    {
                        nextNode = nodeList[nodeId][0].EndStationId; //save nextNode before deleting to be used in recursive function
                        nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest)
                    }


                    searchNode(nextNode);
                }
                else if (smallestDistanceRoute.ContainsKey(nodeId))
                { //if node previously found
                    int smallestPathLength = 0;
                    foreach (RailwayModel route in smallestDistanceRoute[nodeId])
                        smallestPathLength += route.Length;


                    if (currentPathLength > smallestPathLength)
                    { //if blocked ∞∞

                        if (nextNodeRailwayRoute.Count == 0)
                        {
                            nextNode = startStationId;
                        }
                        else
                        {
                            nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch
                            if (nextNodeRailwayRoute.Count == 0)
                            {
                                nextNode = startStationId;
                            }
                            else
                            {
                                nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; // nodeList[nodeId][0].EndingStationId; //set nextNode as ending node from last 
                            }
                        }


                        searchNode(nextNode);
                    }
                    else if (currentPathLength < smallestPathLength)
                    { // if currentpath length is smaller than already exisitng smallest path to this node
                        if (nodeId == endStationId)
                        {
                            smallestDistanceRoute[nodeId] = new List<RailwayModel>(nextNodeRailwayRoute.ToList()); //copy currentRailwayRoute

                            smallestPathToEndNode = currentPathLength;

                            nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                            if (nextNodeRailwayRoute.Count == 0)
                            {
                                return;
                            }
                            nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; //set nextNode as ending node from last 

                            //nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                            searchNode(nextNode);

                        }
                        else
                        {
                            nodeList[nodeId] = nodeListDefault[nodeId].ToList(); // refresh nodes

                            if (currentPathLength + nodeList[nodeId][0].Length >= smallestPathToEndNode)
                            { //if next to search length is bigger than the smallest path to end node

                                nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                                if (nextNodeRailwayRoute.Count == 0)
                                {
                                    nextNode = startStationId;
                                }
                                else
                                {
                                    nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; //set nextNode as ending node from last 

                                    nodeList[nodeId].Clear(); //remove all paths from current node
                                }

                                searchNode(nextNode);
                            }
                            else
                            {
                                smallestDistanceRoute[nodeId] = new List<RailwayModel>(nextNodeRailwayRoute.ToList()); //copy currentRailwayRoute

                                nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

                                nextNode = nodeList[nodeId][0].EndStationId; //save nextNode before deleting to be used in recursive function

                                nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                                searchNode(nextNode);
                            }
                        }
                    }
                    else if (currentPathLength == smallestPathLength)
                    { //if not blocked
                        if (nodeList[nodeId].Count != 0)
                        { //if still has paths
                            if (currentPathLength + nodeList[nodeId][0].Length >= smallestPathToEndNode)
                            { //if next to search length is bigger than the smallest path to end node
                                if (nextNodeRailwayRoute.Count == 0)
                                {
                                    if (nodeId == startStationId)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        searchNode(startStationId);
                                        return;
                                    }
                                }

                                nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                                if (nextNodeRailwayRoute.Count == 0)
                                {
                                    if (nodeId == startStationId)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        searchNode(startStationId);
                                        return;
                                    }
                                }
                                nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; //set nextNode as ending node from last 

                                nodeList[nodeId].Clear(); //remove all paths from current node

                                searchNode(nextNode);
                            }
                            else
                            {
                                nextNodeRailwayRoute.Add(nodeList[nodeId][0]); //add the currently searching railway to the currentRoute

                                nextNode = nodeList[nodeId][0].EndStationId; //save nextNode before deleting to be used in recursive function

                                nodeList[nodeId].RemoveAt(0); //remove the smallest node from nodeList (so next iteration the second smallest node will be the smallest

                                searchNode(nextNode);
                            }
                        }
                        else
                        { // if no more paths
                            if (nodeId != startStationId)
                            {
                                nextNodeRailwayRoute.RemoveAt(nextNodeRailwayRoute.Count - 1); //go back a single branch

                                if (nextNodeRailwayRoute.Count != 0)
                                    nextNode = nextNodeRailwayRoute[nextNodeRailwayRoute.Count - 1].EndStationId; //set nextNode as ending node from last currentRailwayRoute
                                else
                                    nextNode = startStationId;

                                searchNode(nextNode);
                            }
                        }
                    }
                }
            }

            return smallestDistanceRoute[endStationId];
        }

        /// <summary>
        /// finds best consecutively timed journeys with given list of railwayid's after indicated time
        /// </summary>
        /// <param name="railwayList"></param>
        /// <param name="departureTime"></param>
        /// <param name="startStationId"></param>
        /// <returns></returns>
        public List<JourneyModel> FindBestJourney(List<RailwayModel> railwayList, DateTime departureTime, int startStationId)
        {
            int currEndStation = 0;
            for (int i = 0; i < railwayList.Count; i++)
            { //sort list so end and start station actually make sense
                if (i == 0)
                {
                    currEndStation = startStationId;
                }
                int prevStartStation = railwayList[i].StartStationId;
                int prevEndStation = railwayList[i].EndStationId;

                if (prevStartStation != currEndStation)
                { //switch if no good
                    railwayList[i].StartStationId = prevEndStation;
                    railwayList[i].EndStationId = prevStartStation;
                }

                currEndStation = railwayList[i].EndStationId;
            }

            List<JourneyModel> allJourneyList = GetAllJourneys();
            List<JourneyModel> quickestJourneyList = new List<JourneyModel>();

            TimeSpan currDepartureTime = departureTime.TimeOfDay;
            foreach (RailwayModel railway in railwayList)
            {
                List<JourneyModel> toBeSorted = new List<JourneyModel>();
                foreach (JourneyModel journey in allJourneyList)
                {
                    if (journey.RailwayId == railway.RailwayId && journey.StartStationId == railway.StartStationId)
                    {
                        toBeSorted.Add(journey);
                    }
                }

                if (toBeSorted.Count == 0)
                {
                    return null;
                }

                bool added = false;
                toBeSorted.Sort((y, x) => y.DepartureTime.CompareTo(x.DepartureTime)); //sort list by lowest departuretime
                for (int i = 0; i < toBeSorted.Count; i++)
                {
                    if (toBeSorted[i].DepartureTime >= departureTime.TimeOfDay)
                    {
                        added = true;
                        quickestJourneyList.Add(toBeSorted[i]);
                        break;
                    }
                }
                if (!added)
                {//add first journye of the day if no journeys later than departure time
                    quickestJourneyList.Add(toBeSorted[0]);
                }



/*                    TimeSpan shortestTime = TimeSpan.MaxValue;
                JourneyModel shortest = null;
                for (int i = 0; i < toBeSorted.Count; i++)
                {
                    if (toBeSorted[i].DepartureTime < shortestTime)
                    {
                        shortestTime = toBeSorted[i].DepartureTime;
                        shortest = new JourneyModel(toBeSorted[i].RailwayId, toBeSorted[i].TrainId, toBeSorted[i].StartStationId, toBeSorted[i].DepartureTime, toBeSorted[i].ArrivalTime, toBeSorted[i].State);
                    }
                }
                quickestJourneyList.Add(shortest);*/
            }

            return quickestJourneyList;
        }
    }
}