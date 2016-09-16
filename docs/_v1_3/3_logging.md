---
title: Logging
identifier: logging
layout: default
description: Integrer SDP.NET mot din loggplattform
isHome: false
---

<h3 id="customlogger">Logge slik du ønsker</h3>

Behovet for logging er forskjellig fra prosjekt til prosjekt. For å håndtere dette har vi lagt inn mulighet for å sette en egen loggfunksjon på [Klientkonfigurasjon]({{site.coreUrl}}/#Klientkonfigurasjon). `KlientKonfigurasjon.Logger` kan settes til en `Action<TraceEventType, Guid?, String, String>`, hvor `TraceEventType` er hvilken type loggmelding det er , `Guid` er Id på meldingen, nest siste parameter er metoden det ble logget i og til slutt har vi selve meldingen. Her vil meldinger 

Her er et eksempel:

{% highlight csharp %}
var klientkonfigurasjon = new Klientkonfigurasjon
{
    Logger = (severity, konversasjonsId, metode, melding) =>
    {
        System.Diagnostics.Debug.WriteLine("{0} - {1} [{2}]", 
            DateTime.Now, 
            melding, 
            konversasjonsId.GetValueOrDefault()
        );
    }
};
{% endhighlight %}

Det som vi logges gjennom denne funksjonen er info om sendingsprosessen og XML som ligger ved i dokumentpakken. 

<h3 id="lagrexmltilfil"> Lagre XML som sendes</h3>

Når det sendes brev gjennom Sikker Digital Post, så sendes legges det også ved en del ekstra informasjon. Denne informasjonen er strukturert som XML og er nødvending for at brevet skal leveres til mottaker. Ofte kan dette være svært nyttig informasjon å logge for å se hva som faktisk sendes. 

For å aktivere logging av XML så setter du følgende på <code>Klientkonfigurasjon</code>:

{% highlight csharp %}
Klientkonfigurasjon.LoggXmlTilFil = true;
{% endhighlight%}

Hvis <code> Klientkonfigurasjon.StandardLoggSti </code> ikke settes, så finner du loggfilene i _%AppData%/Digipost/Logg_.


<h3 id="dokumentpakkelogger">Lagre dokumentpakke som sendes</h3>

Som avsender kan det være ønskelig å lagre selve pakken med dokumenter som sendes. Denne inneholder også XML som sendes og er den faktiske pakken som krypteres og sendes. For å gjøre dette, setter du følgende på <code>Klientkonfigurasjon</code>:

{% highlight csharp %}
var transportkvittering = sikkerDigitalPostKlient.Send(forsendelse, lagreDokumentpakke: true);
{% endhighlight %}

Hvis <code> Klientkonfigurasjon.StandardLoggSti </code> ikke settes, så finner du dokumentpakkene i _%AppData%/Digipost/Dokumentpakke_.