---
title: Sende post
identifier: sendepost
layout: default
description: Hvordan sende fysisk og digital post
isHome: false
---

For å gjøre det lettere å komme i gang med sending av Sikker digital post så følger det under noen konkrete eksempler.

<blockquote>Det anbefales å bruke dokumentasjon i klassene for mer detaljert beskrivelse av inputparametere.</blockquote>

<h3 id="postinfodigital">PostInfo for digital post</h3>

Først, lag en motaker av type `DigitalPostMottaker`:

<blockquote>
Postkassetjenesteleverandørene har ulik behandling av ikke-sensitiv tittel. Se http://begrep.difi.no/Felles/ikkeSensitivTittel for detaljer om denne forskjellen.
</blockquote>

{% highlight csharp %}

var personnummer = "01013300002";
var postkasseadresse = "ola.nordmann#2233";
var mottakersertifikat = new X509Certificate2(); //sertifikat hentet fra Oppslagstjenesten
var orgnummerPostkasse = "123456789";
var mottaker = new DigitalPostMottaker(
	    personnummer, 
	    postkasseadresse, 
	    mottakersertifikat, 
	    orgnummerPostkasse
    );

var ikkeSensitivTittel = "En tittel som ikke er sensitiv";
var sikkerhetsnivå = Sikkerhetsnivå.Nivå3;
var postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå);

{% endhighlight%}

<blockquote>Husk at<code>OrgnummerPostkasse</code> er organisasjonsnummer til leverandør av postkassetjenesten. Organisasjonsnummeret leveres fra oppslagstjenesten sammen med postkasseadressen og sertifikatet til innbygger.</blockquote>

<h3 id="postinfofysisk">PostInfo for fysisk post</h3>

Skal du sende fysisk post må du først lage en `FysiskPostMottaker`, en `FysiskPostReturMottaker` og sette informasjon om farge og makulering:

{% highlight csharp %}

var navn = "Ola Nordmann";
var adresse = new NorskAdresse("0001", "Oslo");
var mottakersertifikat = new X509Certificate2(); // sertifikat hentet fra Oppslagstjenesten
var orgnummerPostkasse = "123456789";
var mottaker = new FysiskPostMottaker(navn, adresse, mottakersertifikat, orgnummerPostkasse);

var returMottaker = new FysiskPostReturmottaker(
    "John Doe", 
    new NorskAdresse("0566", "Oslo")
    {
        Adresselinje1 = "Returgata 22"
    });

var postInfo = new FysiskPostInfo(
            mottaker, 
            Posttype.A, 
            Utskriftsfarge.SortHvitt, 
            Posthåndtering.MakuleringMedMelding, 
            returMottaker
        );


{% endhighlight%}

Her er adressen av type `NorskAdresse` eller `UtenlandskAdresse`.

Ved sending av fysisk post må man oppgi en returadresse, uavhengig av om brevet er satt til `Posthåndtering.MakuleringMedMelding`. Oppretting av en FysiskPostInfo vil da se slik ut:

<h3 id="oppsettfoersending">Oppsett før sending</h3>

Opprett en avsender og en databehandler:
{% highlight csharp %}

var orgnummerAvsender = new Organisasjonsnummer("123456789");
var avsender = new Avsender(orgnummerAvsender);

var orgnummerDatabehandler = new Organisasjonsnummer("987654321");
var avsendersertifikat = new X509Certificate2();
var databehandler = new Databehandler(orgnummerDatabehandler, avsendersertifikat);

{% endhighlight%}

Hvis man har flere avdelinger innenfor samme organisasjonsnummer, har disse fått unike avsenderidentifikatorer, og kan settes på følgende måte:

{% highlight csharp %}
avsender.Avsenderidentifikator = "Avsenderidentifikator I Organisasjon";
{% endhighlight %}

<h3 id="oppretteforsendelse">Opprette forsendelse</h3>
Deretterer kan du opprette forsendelse. Forsendelsen inneholder de dokumentene
 som skal til mottakeren:

{% highlight csharp %}

var hoveddokument = new Dokument(
        tittel: "Dokumenttittel", 
        dokumentsti: "/Dokumenter/Hoveddokument.pdf", 
        mimeType: "application/pdf", 
        språkkode: "NO", 
        filnavn: "filnavn"
    );

var dokumentpakke = new Dokumentpakke(hoveddokument);

var vedleggssti = "/Dokumenter/Vedlegg.pdf";
var vedlegg = new Dokument(
    tittel: "tittel", 
    dokumentsti: vedleggssti, 
    mimeType: "application/pdf", 
    språkkode: "NO", 
    filnavn: "filnavn");

dokumentpakke.LeggTilVedlegg(vedlegg);

Avsender avsender = null; //Som initiert tidligere
PostInfo postInfo = null; //Som initiert tidligere
var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke);

{% endhighlight %}

<h3 id="opprettKlient">Opprette klient og sende post </h3>
Siste steg er å opprette en `SikkerDigitalPostKlient`:

{% highlight csharp %}

var klientKonfig = new Klientkonfigurasjon();

Databehandler databehandler = null; //Som initiert tidligere
Forsendelse forsendelse = null;     //Som initiert tidligere

var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);
var transportkvittering = sdpKlient.Send(forsendelse);

if (transportkvittering is TransportOkKvittering)
{
    //Når alt går fint	
}
else if(transportkvittering is TransportFeiletKvittering)
{
    var beskrivelse = ((TransportFeiletKvittering)transportkvittering).Beskrivelse;
}

{% endhighlight %}

Transportkvitteringen får du tilbake umiddelbart; den trenger du ikke å polle for å få. 

<h3 id="henteKvitteringer"> Hente kvitteringer</h3>
For å hente kvitteringer må du sende en kvitteringsforespørsel:

{% highlight csharp %}

var køId = "MpcId";
var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, køId);
Console.WriteLine(" > Henter kvittering på kø '{0}'...", kvitteringsForespørsel.Mpc);

Kvittering kvittering = sdpKlient.HentKvittering(kvitteringsForespørsel);

if (kvittering == null)
{
    Console.WriteLine("  - Kø '{0}' er tom. Stopper å hente meldinger. ", kvitteringsForespørsel.Mpc);
}

if (kvittering is TransportFeiletKvittering)
{
    var feil = ((TransportFeiletKvittering) kvittering).Beskrivelse;
    Console.WriteLine("En feil skjedde under transport.");
}

if (kvittering is Leveringskvittering)
{
    Console.WriteLine("  - En leveringskvittering ble hentet!");
}

if (kvittering is Åpningskvittering)
{
    Console.WriteLine("  - Har du sett. Noen har åpnet et brev. Moro.");
}

if (kvittering is Returpostkvittering)
{
    Console.WriteLine("  - Du har fått en returpostkvittering for fysisk post.");
}

if (kvittering is Mottakskvittering)
{
    Console.WriteLine("  - Kvittering på sending av fysisk post mottatt.");
}

if (kvittering is Feilmelding)
{
    var feil = (Feilmelding)kvittering;
    Console.WriteLine("  - En feilmelding ble hentet :" + feil.Detaljer, true);
}

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