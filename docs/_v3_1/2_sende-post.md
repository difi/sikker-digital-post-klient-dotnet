---
title: Sende post
identifier: sendepost
layout: default
description: Hvordan sende fysisk og digital post
isHome: false
---

> Det anbefales å bruke dokumentasjon i klassene for mer detaljert beskrivelse av inputparametere.

### PostInfo for digital post

Først, lag en motaker av type `DigitalPostMottaker`:

> Postkassetjenesteleverandørene har ulik behandling av ikke-sensitiv tittel. Se [begrep.difi.no](http://begrep.difi.no/Felles/ikkeSensitivTittel) for detaljer om denne forskjellen. 

``` csharp
var personnummer = "01013300002";
var postkasseadresse = "ola.nordmann#2233";
var mottakersertifikat = new X509Certificate2(); //sertifikat hentet fra Oppslagstjenesten
var orgnummerPostkasse = new Organisasjonsnummer("123456789");
var mottaker = new DigitalPostMottaker(
        personnummer, 
        postkasseadresse, 
        mottakersertifikat, 
        orgnummerPostkasse
);

var ikkeSensitivTittel = "En tittel som ikke er sensitiv";
var sikkerhetsnivå = Sikkerhetsnivå.Nivå3;
var postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå);
```

> Husk at `OrgnummerPostkasse` er organisasjonsnummer til leverandør av postkassetjenesten. Organisasjonsnummeret leveres fra oppslagstjenesten sammen med postkasseadressen og sertifikatet til innbygger.

### PostInfo for fysisk post

Skal du sende fysisk post må du først lage en `FysiskPostMottaker`, en `FysiskPostReturMottaker` og sette informasjon om farge og makulering:

``` csharp
var navn = "Ola Nordmann";
var adresse = new NorskAdresse("0001", "Oslo");
var mottakersertifikat = new X509Certificate2(); // sertifikat hentet fra Oppslagstjenesten
var orgnummerPostkasse = new Organisasjonsnummer("123456789");
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
```

Her er adressen av type `NorskAdresse` eller `UtenlandskAdresse`.

Ved sending av fysisk post må man oppgi en returadresse, uavhengig av om brevet er satt til `Posthåndtering.MakuleringMedMelding`. Oppretting av en FysiskPostInfo vil da se slik ut:

### Oppsett før sending

Opprett en avsender og en databehandler:

``` csharp
var orgnummerAvsender = new Organisasjonsnummer("123456789");
var avsender = new Avsender(orgnummerAvsender);

var orgnummerDatabehandler = new Organisasjonsnummer("987654321");
var avsendersertifikat = new X509Certificate2();
var databehandler = new Databehandler(orgnummerDatabehandler, avsendersertifikat);
```

Hvis man har flere avdelinger innenfor samme organisasjonsnummer, har disse fått unike avsenderidentifikatorer, og kan settes på følgende måte:

``` csharp
avsender.Avsenderidentifikator = "Avsenderidentifikator.I.Organisasjon";
```

### Opprette forsendelse

Deretterer kan du opprette forsendelse. Forsendelsen inneholder de dokumentene
 som skal til mottakeren:

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

### Opprette forsendelse med Utvidelser

Difi har egne dokumenttyper, eller utvidelser, som kan sendes som metadata til hoveddokumenter. Disse utvidelsene er strukturerte xml-dokumenter med egne mime-typer.  
Disse utvidelsene benyttes av postkasseleverandørene til å gi en øket brukeropplevelse for innbyggere.   
Les mer om utvidelser på: https://difi.github.io/felleslosninger/sdp_utvidelser_index.html  

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
    filnavn: "filnavn"
);

dokumentpakke.LeggTilVedlegg(vedlegg);

var raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?><lenke xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\"><url>https://www.test.no</url><beskrivelse lang=\"nb\">This was raw string</beskrivelse></lenke>";

MetadataDocument metadataDocument = new MetadataDocument("lenke.xml", "application/vnd.difi.dpi.lenke", raw);

Avsender avsender = null; //Som initiert tidligere
PostInfo postInfo = null; //Som initiert tidligere
var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke) { MetadataDocument = metadataDocument };
```

### Opprette klient og sende post

Siste steg er å opprette en `SikkerDigitalPostKlient`:

``` csharp
var klientKonfig = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

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
```

Transportkvitteringen får du tilbake umiddelbart; den trenger du ikke å polle for å få. 


### Hente kvitteringer

For å hente kvitteringer må du sende en kvitteringsforespørsel:

``` csharp
var køId = "MpcId";
var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, køId);
Console.WriteLine(" > Henter kvittering på kø '{0}'...", kvitteringsForespørsel.Mpc);

Kvittering kvittering = sdpKlient.HentKvittering(kvitteringsForespørsel);

if (kvittering is TomKøKvittering)
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
```

> Husk at det ikke er mulig å hente nye kvitteringer før du har bekreftet mottak av nåværende. 


``` csharp
sdpKlient.Bekreft((Forretningskvittering)kvittering);
```

Kvitteringer du mottar når du gjør en kvitteringsforespørsel kan være av følgende typer: `Leveringskvittering`,`Åpningskvittering`, `Returpostkvittering`, `Mottakskvittering` eller `Feilmelding`. Kvittering kan også være av typen`TransportFeiletKvittering`. Dette kan skje når selve kvitteringsforespørselen er feilformatert.

> Husk at hvis du får `TomKøKvittering` så er køen tom. Du henter bare kvitteringer fra kø gitt av `MpcId` og `Prioritet`. Hvis ikke dette blir satt spesifikt vil det hentes fra kø hvor `MpcId = ""` og `Prioritet = Prioritet.Normal`.

