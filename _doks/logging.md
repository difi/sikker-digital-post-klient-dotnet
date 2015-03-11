---
title: Logging
layout: default
description: Integrer SDP.NET mot din loggplattform
isHome: true
---

Behovet for logging er forskjellig fra prosjekt til prosjekt. For å håndtere dette har vi lagt inn mulighet for å sette en egen loggklasse. `KlientKonfigurasjon.Logger` kan settes til en `Action<TraceEventType, Guid?, String, String>`, hvor `TraceEventType` er hvilken type loggmelding det er , `Guid` er Id på meldingen, nest siste parameter er metoden det ble logget i og til slutt har vi selve meldingen. 

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


