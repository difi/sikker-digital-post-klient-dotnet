---
title: Endringer fra sikker digital post klient
identifier: endringerfrasikkerdigitalpostklient
layout: default
---


Formålet med dette biblioteket er å tilby en drop-in erstatning for sikker-digital-post-klient-biblioteket, slik at man enkelt kan ta i bruk Integrasjonspunktet fremfor direkte sending til meldingsformidleren.
API-et til sikker-digital-post-klient er bevart etter beste evne. Under er det listet opp konsepter som skiller seg fra sikker-digital-post-klient.


- Konfigurasjon av miljø
- Endring i opprettelse av printforsendelse
- Metoder og klasser som er obsolete eller fjernet 
- mpcID
- Prioritet
- Nøkler og sertifikat 



### Konfigurasjon av miljø
Konfigurasjon av miljø gjøres nå ved å spesifisere URL-en til integrasjonspunktet som man ønsker å benytte.

`Klientkonfigurasjon` tilbyr nå tre metoder for å sette miljø. Vær OBS på å endre parameter hvis string- eller URI-builder-metoden ble benyttet i tidligere klientbibliotek da man i tilfellet ikke vil få noen typefeil/synlige feil i koden. 

```java
//builder(Miljo miljo)}
Miljo INTEGRASJONSPUNKT_LOCALHOST = new Miljo(URI.create("http://localhost:9093"));
KlientKonfigurasjon klientKonfigurasjon = KlientKonfigurasjon.builder(miljo).build();


//builder(URI integrasjonspunktRoot)
URI INTEGRASJONSPUNKT_URI = URI.create("http://localhost:9093");
KlientKonfigurasjon klientKonfigurasjon = KlientKonfigurasjon.builder(INTEGRASJONSPUNKT_URI).build();

//@Deprecated
//builder(String integrasjonspunktRootUri)
String INTEGRASJONSPUNKT_URI = "http://localhost:9093";
KlientKonfigurasjon klientKonfigurasjon = KlientKonfigurasjon.builder(INTEGRASJONSPUNKT_URI).build();
```    

### Endring i opprettelse av printforsendelse

Personidentifikator til mottaker må nå oppgis.

```java
// Tidligere
var postInfo = new FysiskPostInfo(
            mottaker, 
            Posttype.A, 
            Utskriftsfarge.SortHvitt, 
            Posthåndtering.MakuleringMedMelding, 
            returMottaker
);

// Nå   
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

### Metoder og klasser som er obsolete eller fjernet
Utdaterte konsepter er markert med obsolete. Det er anbefalt å benytte overloadet metoder. Under følger en listet over noen berørte klasser og felter.
Generelt er alle felter som har med sertifikat markert som obsolete. Bakgrunnen for dette er at integrasjonspunktet nå håndterer dette.

- `CertificateReader`: Klasse og alle metoder er obsolete.
- `Databehandler`: Konstruktør som tar inn sertifikat. Feltet `Sertifikat` er market som obsolete og `error:true` (kaster feil om det aksesseres).
- `Kvitteringsforespørsel`: Prioritet og MpcID er obsolete.
- `FysiskPostMottaker`: Konstruktør som tar inn sertifikat er obsolete.
- `DigitalPostMottaker`: Konstruktør som tar inn sertifikat er obsolete.
- `KlientKonfigurasjon`: `AktiverLagringAvDokumentpakkeTilDisk` og `Dokumentpakkeprosessorer` er fjernet. Det er integrasjonspunktet som er ansvarlig for å lage ASiC.
- `EpostVarsel`: Konstruktører som tar inn e-postadresse er obsolete.
- `SmsVarsel`: Konstruktører som tar inn mobilnummer er obsolete.
 

### mpcID
MpcID settes nå i integrasjonspunktet.

### Prioritet
Kø-prioritet settes nå i integrasjonspunktet.

### Nøkler og sertifikat
Nøkler og sertifikater håndteres av integrasjonspunktet.



