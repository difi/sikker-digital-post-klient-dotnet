---
title: Sende post
identifier: sendepost
layout: default
description: Hvordan sende fysisk og digital post
isHome: false
---

> Det anbefales å bruke dokumentasjon i klassene for mer detaljert beskrivelse av inputparametere.

### Opprette digital post

Først, lag en motaker av type `DigitalPostMottaker`:

``` csharp
var personnummer = "01013300002";
var mottaker = new DigitalPostMottaker(
        personnummer 
);

var ikkeSensitivTittel = "En tittel som ikke er sensitiv";
var sikkerhetsnivå = Sikkerhetsnivå.Nivå3;
var postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå);
```

> Postkassetjenesteleverandørene har ulik behandling av ikke-sensitiv tittel. Se [https://difi.github.io/felleslosninger/ikkesensitivtittel.html](https://difi.github.io/felleslosninger/ikkesensitivtittel.html) for detaljer om denne forskjellen.


### Opprett fysisk post

``` csharp
var navn = "Ola Nordmann";
var adresse = new NorskAdresse("0001", "Oslo");
var mottaker = new FysiskPostMottaker(navn, adresse);

var returMottaker = new FysiskPostReturmottaker(
    "John Doe", 
    new NorskAdresse("0566", "Oslo")
    {
        Adresselinje1 = "Returgata 22"
    });

var fysiskPostMottakerPersonnummer = "27127000293"
var postInfo = new FysiskPostInfo(
            fysiskPostMottakerPersonnummer,
            mottaker, 
            Posttype.A, 
            Utskriftsfarge.SortHvitt, 
            Posthåndtering.MakuleringMedMelding, 
            returMottaker
);
```

Adressen kan også være av typen `UtenlandskAdresse`.

Ved sending av fysisk post må man oppgi en returadresse, uavhengig av om brevet er satt til `Posthåndtering.MakuleringMedMelding`.

### Oppsett før sending

Opprett en avsender og en databehandler:

``` csharp
var orgnummerAvsender = new Organisasjonsnummer("123456789");
var avsender = new Avsender(orgnummerAvsender);

var orgnummerDatabehandler = new Organisasjonsnummer("987654321");
var databehandler = new Databehandler(orgnummerDatabehandler);
```

Avsenderidentifikator benyttes for å identifisere en ansvarlig enhet innenfor en virksomhet. 
Identifikatoren tildeles ved tilkobling til tjenesten. 

``` csharp
avsender.Avsenderidentifikator = "Avsenderidentifikator.I.Organisasjon";
```

### Opprette forsendelse

``` csharp
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
```


### Opprette forsendelse med utvidelse

Difi har egne dokumenttyper, eller utvidelser, som kan sendes som metadata til hoveddokumenter. Disse utvidelsene er strukturerte xml-dokumenter med egne mime-typer.  
Disse utvidelsene benyttes av postkasseleverandørene til å gi en øket brukeropplevelse for innbyggere.   
Les mer om utvidelser på: https://difi.github.io/felleslosninger/sdp_utvidelser_index.html  

``` csharp
var raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                      "<lenke xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\">" +
                      "<url>https://difi.github.io/felleslosninger/sdp_lenke.html</url>" +
                      "<beskrivelse lang=\"nb\">Les mer om lenkeutvidelse</beskrivelse>" +
                      "</lenke>";

MetadataDocument metadataDocument = new MetadataDocument("lenke.xml", "application/vnd.difi.dpi.lenke", raw);

Dokumentpakke dokumentpakke = null; //Som initiert tidligere
Avsender avsender = null; //Som initiert tidligere
PostInfo postInfo = null; //Som initiert tidligere
var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke) { MetadataDocument = metadataDocument };
```

### Opprette klient og sende post


``` csharp
var integrasjonspunktLocalhost = new Miljø(new Uri("http://localhost:9093"));
var klientKonfig = new Klientkonfigurasjon(integrasjonspunktLocalhost);

Databehandler databehandler = null; //Som initiert tidligere
Forsendelse forsendelse = null;     //Som initiert tidligere

var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);
var transportkvittering = sdpKlient.Send(forsendelse);

if (transportkvittering is TransportOkKvittering)
{
    //Når sending til integrasjonspunktet gikk bra.		
}
else if(transportkvittering is TransportFeiletKvittering)
{
    var beskrivelse = ((TransportFeiletKvittering)transportkvittering).Beskrivelse;
}
```

### Hente kvitteringer

For å hente kvitteringer fra integrasjonspunktet må du sende en kvitteringsforespørsel:

``` csharp
SikkerDigitalPostKlient sdpKlient = null; //Som initiert tidligere

var kvitteringsForespørsel = new Kvitteringsforespørsel();
Kvittering kvittering = sdpKlient.HentKvittering(kvitteringsForespørsel);

if (kvittering is TomKøKvittering)
{
    Console.WriteLine("  - Kø er tom. Stopper å hente kvitteringer.");
}

if (kvittering is TransportFeiletKvittering)
{
    var feil = ((TransportFeiletKvittering) kvittering).Beskrivelse;
    Console.WriteLine("En feil skjedde under sending til integrasjonspunktet.");
}

if (kvittering is Leveringskvittering)
{
    Console.WriteLine("  - En leveringskvittering ble hentet!");
}

if (kvittering is Åpningskvittering)
{
    Console.WriteLine("  - Noen har åpnet et brev. ");
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
```

> Husk at det ikke er mulig å hente nye kvitteringer før du har bekreftet mottak av nåværende. 


``` csharp
sdpKlient.Bekreft((Forretningskvittering)kvittering);
```

Det er ikke mulig å hente nye kvitteringer før du har bekreftet mottak av liggende kvittering på køen.
For mer informasjon om de ulike kvitteringene henviser vi til Digitaliseringsdirektoratet: https://difi.github.io/felleslosninger/sdp_index.html

