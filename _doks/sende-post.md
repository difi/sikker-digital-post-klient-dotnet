---
title: Sende post
id: sendepost
layout: default
description: Hvordan sende fysisk og digital post
isHome: false
---

For å gjøre det lettere å komme i gang med sending av Sikker digital post så følger det under noen konkrete eksempler.

<blockquote>Det anbefales å bruke dokumentasjon i klassene for mer detaljert beskrivelse av inputparametere.</blockquote>

<h3 id="postinfodigital">PostInfo for digital post</h3>

Først, lag en motaker av type `DigitalPostMottaker`:

{% highlight csharp %}
var mottaker = new DigitalPostMottaker(personnummer, postkasseadresse, mottakersertifikat, orgnummerPostkasse)
{% endhighlight%}

<blockquote>Husk at<code>OrgnummerPostkasse</code> er organisasjonsnummer til leverandør av postkassetjenesten. Organisasjonsnummeret leveres fra oppslagstjenesten sammen med postkasseadressen og sertifikatet til innbygger.</blockquote>

Opprett så en `DigitalPostInfo`:
{% highlight csharp %}
var postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå)
{% endhighlight%}

<h3 id="postinfofysisk">PostInfo for fysisk post</h3>

Skal du sende fysisk post må du først lage en `FysiskPostMottaker`:
{% highlight csharp %}
var mottaker = new FysiskPostMottaker(navn, adresse, mottakersertifikat, orgnummerPostkasse)
{% endhighlight%}

Her er adressen av type `NorskAdresse` eller `UtenlandskAdresse`.

Ved sending av fysisk post må man oppgi en returadresse, uavhengig av om brevet er satt til `Posthåndtering.MakuleringMedMelding`. Oppretting av en FysiskPostInfo vil da se slik ut:

{% highlight csharp %}
var postInfo = new FysiskPostInfo(mottaker, Posttype.A, Utskriftsfarge.SortHvitt, Posthåndtering.MakuleringMedMelding, returMottaker);
{% endhighlight%}

<h3 id="oppsettfoersending">Oppsett før sending</h3>

Opprett en avsender og en databehandler:
{% highlight csharp %}
var avsender = new Avsender(orgnummerAvsender);

var databehandler = new Databehandler(orgnummerDatabehandler, avsendersertifikat);
{% endhighlight%}

Hvis man har flere avdelinger innenfor samme organisasjonsnummer, har disse fått unike avsenderidentifikatorer, og kan settes på følgende måte:

{% highlight csharp %}
avsender.Avsenderidentifikator = "avsenderidentifikatorIOrganisasjon"
{% endhighlight %}

<h3 id="oppretteforsendelse">Opprette forsendelse</h3>
Deretter, opprett forsendelse. Forsendelsen inneholder de dokumentene som skal til mottakeren:

{% highlight csharp %}
var hoveddokumentSti = "/Dokumenter/Hoveddokument.pdf";
var hoveddokument = new Dokument(tittel, hoveddokumentsti, "application/pdf", "NO", "filnavn");

var dokumentpakke = new Dokumentpakke(hoveddokument);

var vedleggssti = "/Dokumenter/Vedlegg.pdf";
var vedlegg = new Dokument("tittel", vedleggsti, "application/pdf", "NO", "filnavn");

dokumentpakke.LeggTilVedlegg(vedlegg);
{% endhighlight %}

<blockquote>
Postkassetjenesteleverandørene har ulik behandling av ikke-sensitiv tittel. Se [http://begrep.difi.no](http://begrep.difi.no/Felles/ikkeSensitivTittel) for detaljer om denne forskjellen.
</blockquote>

Deretter er det bare å opprette en forsendelse med `PostInfo` (`DigitalPostInfo` eller `FysiskPostInfo`). 

{% highlight csharp %}
var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke);
{% endhighlight %}

<h3 id="opprettKlient">Opprette klient og sende post </h3>
Siste steg er å opprette en `SikkerDigitalPostKlient`:

{% highlight csharp %}
var klientKonfig = new Klientkonfigurasjon();
klientKonfig.MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"); //Testmiljø, standard er produksjon.

var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);

var transportkvittering = sdpKlient.Send(forsendelse)
{% endhighlight %}

Transportkvitteringen kan enten være av type `TransportOkKvittering` eller `TransportFeiletKvittering`. For sistnevnte er `Alvorlighetsgrad` og `Beskrivelse` nyttige felter når det går galt.

{% highlight csharp %}
if(transportkvittering.GetType() == typeof(TransportOkKvittering))
{
	//Gjør logikk når alt går fint	
}

if(transportkvittering.GetType() == typeof(TransportFeiletKvittering))
{
	//Gjør logikk når det går galt	
	 var feil = ((TransportFeiletKvittering)kvittering).Beskrivelse;
}
{% endhighlight %}

Transportkvitteringen får du tilbake umiddelbart; den trenger du ikke å polle for å få. 

<h3 id="henteKvitteringer"> Hente kvitteringer</h3>
For å hente kvitteringer må du sende en kvitteringsforespørsel:

{% highlight csharp %}
var kvitteringsforespørsel = new Kvitteringsforespørsel(Prioritet, MpcId)

var kvittering = sdpKlient.HentKvittering(kvitteringsforespørsel);
{% endhighlight %}

<blockquote>
Husk at det ikke er mulig å hente nye kvitteringer før du har bekreftet mottak av nåværende. 
</blockquote>

{%highlight csharp%}
sdpKlient.Bekreft((Forretningskvittering)kvittering);
{% endhighlight%}

Kvitteringer du mottar når du gjør en kvitteringsforespørsel kan være av følgende typer: `Leveringskvittering`,`Åpningskvittering`, `Returpostkvittering`, `Mottakskvittering` eller `Feilmelding`. Kvittering kan også være av typen`TransportFeiletKvittering`. Dette kan skje når selve kvitteringsforespørselen er feilformatert.

<blockquote>
Husk at hvis kvitteringen er <code>null</code> så er køen tom. Du henter bare kvitteringer fra kø gitt av <code>MpcId</code> og <code>Prioritet</code> på Dokumentpakken som ble sendt. Hvis ikke dette ble satt spesifikt vil <code>MpcId = ""</code> og <code>Prioritet = Prioritet.Normal</code>.
</blockquote>

Et eksempel på sjekk om kvittering er `Åpningskvittering`:
{%highlight csharp%}
if(kvittering is Åpningskvittering)
{
	Console.WriteLine("Åpningskvittering mottatt!")
}
{% endhighlight%}

