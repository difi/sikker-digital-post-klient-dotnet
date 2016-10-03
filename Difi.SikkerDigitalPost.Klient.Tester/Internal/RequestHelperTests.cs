﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Resources.Xml;
using Difi.SikkerDigitalPost.Klient.Tester.Fakes;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal
{
    public class RequestHelperTests
    {
        public class ConstructorMethod : RequestHelperTests
        {
            [Fact]
            public void Initializes_fields()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                var requestHelper = new RequestHelper(clientConfiguration);

                //Assert
                Assert.Equal(clientConfiguration, requestHelper.ClientConfiguration);
            }
        }

        public class SendMethod : RequestHelperTests
        {
            [Fact]
            public async Task Returns_receipt_successfully()
            {
                //Arrange
                var forretningsmeldingEnvelope = DomainUtility.GetForretningsmeldingEnvelope();

                var documentBundle = AsiceGenerator.Create(DomainUtility.GetForsendelseSimple(), new GuidUtility(), DomainUtility.GetAvsenderCertificate(), DomainUtility.GetKlientkonfigurasjon());

                var requestHelper = new RequestHelper(new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø));
                var fakeHttpClientHandlerResponse = new FakeResponseHandler()
                {
                    HttpContent = new StringContent(XmlResource.Response.GetTransportOk().OuterXml),
                    StatusCode = HttpStatusCode.OK
                };
                requestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act 
                var kvittering = await requestHelper.SendMessage(forretningsmeldingEnvelope, documentBundle).ConfigureAwait(false);

                Assert.IsType<TransportOkKvittering>(kvittering);
            }
        }

        public class HttpClientProperty
        {
            [Fact]
            public async Task Adds_user_agent()
            {
                //Arrange
                var forretningsmeldingEnvelope = DomainUtility.GetForretningsmeldingEnvelope();
                var documentBundle = AsiceGenerator.Create(DomainUtility.GetForsendelseSimple(), new GuidUtility(), DomainUtility.GetAvsenderCertificate(), DomainUtility.GetKlientkonfigurasjon());

                Action<HttpRequestMessage> testingAction = message =>
                {
                    Assert.Contains("sikker-digital-post", message.Headers.UserAgent.ToString());
                };

                var requestHelper = new RequestHelper(
                    new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø),
                    new FakeResponseHandler(testingAction)
                    {
                        HttpContent = new StringContent(XmlResource.Response.GetTransportOk().OuterXml)
                    });

                //Act 
                await requestHelper.SendMessage(forretningsmeldingEnvelope, documentBundle).ConfigureAwait(false); 
            }
        }
    }
}