using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Station;
using TestData.Station;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class StationUT
    {
        [TestMethod]
        public void FindStationById()
        {
            //Arrange..?
            var stationC = new StationContainer(new StationDALTest());
             
            //Act
            var output = stationC.FindStationById(2);

            //Assert
            string actualValue = "tilburg university";
            Assert.AreEqual(actualValue, output.Name);
        }

        [TestMethod]
        public void GetStations()
        {
            //Arrange..?
            var stationC = new StationContainer(new StationDALTest());

            //Act
            var output = stationC.GetAllStations();

            //Assert
            Assert.IsTrue(output.Count == 3);
        }

        [TestMethod]
        public void GetStations2()
        {
            //Arrange..?
            var stationC = new StationContainer(new StationDALTest());

            //Act
            var output = stationC.GetAllStations();

            //Assert
            string actualValue = "tilburg university";
            Assert.AreEqual(actualValue, output[1].Name);
        }

        [TestMethod]
        public void InsertStation()
        {
            //Arrange..?
            var stationDAL = new StationDALTest();
            var stationC = new StationContainer(stationDAL);

            //Act
            var output = stationC.InsertStation(new StationModel("test1", "test2"));

            //Assert
            Assert.AreEqual(true, output);
        }

        [TestMethod]
        public void InsertStation2()
        {
            //Arrange..?
            var stationDAL = new StationDALTest();
            var stationC = new StationContainer(stationDAL);

            //Act
            var output = stationC.InsertStation(new StationModel("test1", "test2"));

            //Assert
            Assert.AreEqual(4, stationDAL.stations.Count);
        }

        [TestMethod]
        public void InsertStation3()
        {
            //Arrange..?
            var stationDAL = new StationDALTest();
            var stationC = new StationContainer(stationDAL);

            //Act
            stationC.InsertStation(new StationModel("test1", "test2"));
            
            //Assert
            Assert.AreEqual("test2", stationDAL.FindById(4).Name);
        }

        [TestMethod]
        public void UpdateStation()
        {
            //Arrange..?
            var stationDAL = new StationDALTest();
            var stationC = new StationContainer(stationDAL);
            var station = stationC.FindStationById(1);

            //Act
            station.Location = "something else";
            var output = station.UpdateStation(new StationDALTest());

            //Assert
            Assert.AreEqual(true, output);
        }


        [TestMethod]
        public void UpdateStation2()
        {
            //Arrange..?
            var stationDAL = new StationDALTest();
            var stationC = new StationContainer(stationDAL);
            var station = stationC.FindStationById(1);

            //Act
            station.Location = "something else";
            var output = station.UpdateStation(new StationDALTest());

            //Assert
            Assert.AreEqual(3, stationDAL.stations.Count);
        }
    }
} 