---
title: Klientkonfigurasjon
identifier: klientkonfigurasjon
layout: default
description: Sette opp klientkonfigurasjon
isHome: false
---

Klientkonfigurasjon brukes for å sette opp koblingsspesifikke innstillinger mot meldingsformidleren, som `ProxyHost`, `ProxyScheme` og `TimeoutMillisekunder`. Denne må sendes med som innparameter til `SikkerDigitalPostKlient`.

For å sette url mot meldingsformidler, kan du gjøre dette slik:

{% highlight csharp%}
var klientkonfigurasjon = new Klientkonfigurasjon();

klientkonfigurasjon.MeldingsformidlerUrl = new Uri("https://eksempelendepunkt.no")

{% endhighlight%}