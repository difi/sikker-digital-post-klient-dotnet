---
title: Ytelse
identifier: ytelse
layout: default
description: Hvordan øke ytelsen på biblioteket
isHome: false
---

Klientbiblioteket benytter en `HttpWebRequest` for å kommunisere med Meldingsformidleren. I en konsollapplikasjon er denne begrenset til maks to samtidige forbindelser om gangen, mens den i en asp.net applikasjon er begrenset til ti. Dersom du ønsker å sende flere brev samtidig kan denne verdien endres f.eks til 3. Mer enn dette anbefales ikke.

{% highlight csharp%}
System.Net.ServicePointManager.DefaultConnectionLimit = 3;
{% endhighlight %}

Se [ServicePointManager.DefaultConnectionLimit](http://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.defaultconnectionlimit(v=vs.110).aspx) for mer informasjon.

Klassen `SikkerDigitalPostKlient` fungerer best ved at man oppretter en instans per applikasjon. `SikkerDigitalPostKlient` bruker en og samme instans av `HttpClient` (https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.110).aspx) under panseret. Denne klassen er trådsikker og beste måte å bruke denne klassen på er å ha en instans per applikasjon som gjenbrukes for alle http-kall i applikasjons levetid. Av trådsikkerhetshensyn og ytelse er dette også måten brukere av biblioteket bør benytte `SikkerDigitalPostKlient` på.
