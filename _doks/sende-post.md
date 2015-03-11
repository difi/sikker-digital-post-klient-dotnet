---
title: Sende post
layout: default
description: Hvordan sende fysisk og digital post
isHome: false
---

I disse eksempler er det Posten som er den som produserer informasjon/brev/post som skal formidles (Behandlingsansvarlig) og Posten som er teknisk avsender.

_Det anbefales å bruke dokumentasjon i klassene for mer detaljert beskrivelse av inputparametere_ 

<h3 id="postinfodigital">PostInfo for digital post</h3>

Først, lag en motaker av type `DigitalPostMottaker`:

{% highlight csharp %}
var mottaker = new DigitalPostMottaker(personnummer, postkasseadresse, mottakersertifikat, orgnummerPostkasse)
{% endhighlight%}

Opprett så en `DigitalPostInfo`:
{% highlight csharp %}
postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå, åpningskvittering)
{% endhighlight%}

<h3 id="postinfofysisk">PostInfo for fysisk post</h3>

Skal du sende fysisk post må du først lage en `FysiskPostMottaker`:
{% highlight csharp %}
var mottaker = new FysiskPostMottaker(navn, adresse, mottakersertifikat, orgnummerPostkasse)
{% endhighlight%}

Her er adressen av type `NorskAdresse` eller `UtenlandskAdresse`.

Ved sending av fysisk post må man oppgi en returadresse, uavhengig av om brevet er satt til `Posthåndtering.MakuleringMedMelding`. Oppretting av en FysiskPostInfo vil da se slik ut:

{% highlight csharp %}
postInfo = new FysiskPostInfo(mottaker, Posttype.A, Utskriftsfarge.SortHvitt, Posthåndtering.MakuleringMedMelding, returMottaker);
{% endhighlight%}

<h3 id="oppsettfoersending">Oppsett før sending</h3>

Lag en behandlingsansvarlig og en teknisk avsender:
{% highlight csharp %}
behandlingsansvarlig = new Behandlingsansvarlig(orgnummerBehandlingsansvarlig)
behandlingsansvarlig.Avsenderidentifikator = "Digipost";

tekniskAvsender = new Databehandler(orgnummerDatabehandler, avsendersertifikat);
{% endhighlight%}

<h3 id="oppretteforsendelse">Opprette forsendelse</h3>
Deretter, opprett forsendelse. Forsendelsen inneholder de dokumentene som skal til mottakeren:
{% highlight csharp %}
var hoveddokumentSti = "/Dokumenter/Hoveddokument.pdf";
var hoveddokument = new Dokument(tittel, hoveddokumentsti, "application/pdf", "NO", "filnavn");

var dokumentpakke = new Dokumentpakke(hoveddokument);

var vedleggssti = "/Dokumenter/Vedlegg.pdf":
var vedlegg = new Dokument("Vedlegg", vedleggsti, "application/pdf", "NO", "filnavn");

dokumentpakke.LeggTilVedlegg(vedlegg);



{% endhighlight %}