using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Railway;
using TestData.Railway;

namespace UnitTest
{
    [TestClass]
    public class RailwayUT
    {
        [TestMethod]
        public void FindRailwayById()
        {
            //Arrange..?
            var railwayC = new RailwayContainer(new RailwayDALTest());
             
            //Act
            var output = railwayC.FindRailwayById(2);

            //Assert
            int actualValue = 3;
            Assert.AreEqual(actualValue, output.EndStationId);
        }

        [TestMethod]
        public void GetRailways()
        {
            //Arrange..?
            var stationC = new RailwayContainer(new RailwayDALTest());

            //Act
            var output = stationC.GetAllRailways ();

            //Assert
            bool actualValue = true;
            Assert.AreEqual(actualValue, output[1].State);
        }

        [TestMethod]
        public void GetRailways2()
        {
            //Arrange..?
            var stationC = new RailwayContainer(new RailwayDALTest());

            //Act
            var output = stationC.GetAllRailways();

            //Assert
            Assert.IsTrue(output.Count == 3);
        }

        [TestMethod]
        public void InsertRailway()
        {
            //Arrange..?
            var railwayDAL = new RailwayDALTest();
            var railwayC = new RailwayContainer(railwayDAL);

            //Act
            var output = railwayC.InsertRailway(new RailwayModel(1, 1, true, 19));

            //Assert
            Assert.AreEqual(true, output);
        }

        [TestMethod]
        public void InsertRailway2()
        {
            //Arrange..?
            var railwayDAL = new RailwayDALTest();
            var railwayC = new RailwayContainer(railwayDAL);

            //Act
            var output = railwayC.InsertRailway(new RailwayModel(1, 1, true, 19));

            //Assert
            Assert.AreEqual(4, railwayDAL.railways.Count);
        }

        [TestMethod]
        public void UpdateRailway()
        {
            //Arrange..?
            var railwayDAL = new RailwayDALTest();
            var railwayC = new RailwayContainer(railwayDAL);
            var railway = railwayC.FindRailwayById(1);

            //Act
            railway.EndStationId = 1;
            var output = railway.UpdateRailway(new RailwayDALTest());

            //Assert
            Assert.AreEqual(3, railwayDAL.railways.Count);
        }

        [TestMethod]
        public void UpdateRailway2()
        {
            //Arrange..?
            var railwayDAL = new RailwayDALTest();
            var railwayC = new RailwayContainer(railwayDAL);
            var railway = railwayC.FindRailwayById(1);

            //Act
            railway.EndStationId = 1;
            var output = railway.UpdateRailway(new RailwayDALTest());

            //Assert
            Assert.AreEqual(true, output);
        }
    }
}