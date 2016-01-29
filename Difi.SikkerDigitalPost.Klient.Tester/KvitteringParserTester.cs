using Microsoft.VisualStudio.TestTools.UnitTesting;
using Difi.SikkerDigitalPost.Klient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Tests
{
    [TestClass()]
    public class KvitteringParserTester
    {
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.Skjema.Eksempler.Kvitteringer");

        [TestClass]
        public class TilKvitteringMethod : KvitteringParserTester
        {
            [TestMethod]
            public void ParserLeveringskvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("Leveringskvittering.xml");

                //Act

                //Assert
            }

            [TestMethod]
            public void ParserMottakskvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("Mottakskvittering.xml");


                //Act

                //Assert
            }

            [TestMethod]
            public void ParserReturpostkvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("Returpostkvittering.xml");

                //Act

                //Assert
            }

            [TestMethod]
            public void ParserVarslingFeiletKvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("VarslingFeiletKvittering.xml");

                //Act

                //Assert
            }

            private XmlDocument TilXmlDokument(string kvittering)
            {
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, kvittering)));
            }

            [TestMethod]
            public void ParserÅpningskvittering()
            {
                //Arrange
                //XmlUtility.TilXmlDokument(ResourceUtility.ReadAllBytes(true, k));

                //Act

                //Assert
            }

        }

    }
}