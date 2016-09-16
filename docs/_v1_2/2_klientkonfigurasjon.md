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

<h3 id="endepunkter">Endepunkter</h3>

#### Funksjonelt testmiljø ####
<https://qaoffentlig.meldingsformidler.digipost.no/api/>

#### Produksjonsmiljø ####
<https://meldingsformidler.digipost.no/api/>

#### Norsk Helsenett - Funksjonelt testmiljø ####
<https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/>

#### Norsk Helsenett - Produksjonsmiljø ####
<https://meldingsformidler.nhn.digipost.no:4444/api/>