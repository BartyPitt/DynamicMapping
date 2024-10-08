using DynamicMapping.Models;
using DynamicMapping;
using System;

namespace Unittests_DynamicMapping
{
    [TestClass]
    public class DynamicMapping
    {

        string ExampleXml = "<Reservation>" +
                            "<ClientName>client</ClientName>" +
                            "<RoomNumber>1</RoomNumber>" +
                           "<StartDate>2000-01-02T00:00:00</StartDate>" +
                           "<EndDate>2010-03-04T00:00:00</EndDate>" +
                           "</Reservation>";
        string ExampleJson = "{\"ClientName\":\"client\",\"RoomNumber\":1,\"StartDate\":\"2000-01-02T00:00:00\",\"EndDate\":\"2010-03-04T00:00:00\"}";

        [TestMethod]
        public void TestExportBasic()
        {
            Reservation TestReservation = new Reservation()
            {
                ClientName = "client",
                RoomNumber = 1,
                StartDate = new DateTime(3210, 9, 12),
                EndDate = new DateTime(3212, 5, 7),
            };

            string XMLReservation = (string)MapHandler.Map(TestReservation, MapHandler.Mappers.PredefinedModel, MapHandler.Mappers.Xml, MapHandler.Models.Reservation);
            string Json = (string)MapHandler.Map(TestReservation, MapHandler.Mappers.PredefinedModel, MapHandler.Mappers.Json, MapHandler.Models.Reservation);

            Assert.IsTrue(!String.IsNullOrEmpty(XMLReservation));
            Assert.IsTrue(!String.IsNullOrEmpty(Json));
        }

        [TestMethod]
        public void TestInvalidSeralize()
        {
            Assert.ThrowsException< NotSupportedException>(() => MapHandler.Map("�OLNDBG�ERBNH�", "NOT VALID", "XML", "Reservation"));
            Assert.ThrowsException< NotSupportedException>(() => MapHandler.Map("�OLNDBG�ERBNH�", "XML", "Not Valid", "Reservation"));
            Assert.ThrowsException< NotSupportedException>(() => MapHandler.Map("�OLNDBG�ERBNH�", "XML", "XML", "NOT VALID"));
            MapHandler.Map(ExampleXml, "XML", "XML", "Reservation");
        }
        [TestMethod]
        public void TestJSONSeralize()
        {
            Reservation TestReservation = new Reservation()
            {
                ClientName = "Differntdsgje",
                RoomNumber = 1,
                StartDate = new DateTime(1234, 9, 12),
                EndDate = new DateTime(3210, 5, 7),
            };
            string TestJson = (string)MapHandler.Map(TestReservation, MapHandler.Mappers.PredefinedModel, MapHandler.Mappers.Json, MapHandler.Models.Reservation);
            Reservation NewReservation = (Reservation)MapHandler.Map(TestJson, MapHandler.Mappers.Json, MapHandler.Mappers.PredefinedModel, MapHandler.Models.Reservation);
            Assert.AreEqual(TestReservation.ClientName, NewReservation.ClientName);
            Assert.AreEqual(TestReservation.RoomNumber, NewReservation.RoomNumber);
            Assert.AreEqual(TestReservation.StartDate, NewReservation.StartDate);
        }
    }
}