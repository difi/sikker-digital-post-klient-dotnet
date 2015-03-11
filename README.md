#ONLINE DOKUMENTASJON#
[DOKUMENTASJON FINNER DU HER](http://difi.github.io/sikker-digital-post-net-klient)

#Sikker Digital Post .NET klient

Dette er en .NET-klient for sending av sikker digital post for det offentlige. Formålet for klienten er å forenkle integrasjonen 
som må utføres av avsendervirksomheter. For mer informasjon om sikker digital post, se [her](http://begrep.difi.no/SikkerDigitalPost/).

**NB: Klienten er under utvikling, og vil inneholde en del feil. Forberdinger vil komme fortløpende.**

#Hvordan komme i gang

##NuGet-pakke

Klienten er tilgjengelig som en NuGet-pakke. Denne vil oppdateres jevnlig etter hvert som ny funksjonalitet legges til.

For å installere NuGet-pakken, gjør følgende:

1. Velg "TOOLS -> NuGet Package Manager -> Manage Nuget Packages for Solution..."
2. Søk etter "Sikker Digital Post Klientbibliotek".
3. Siden NuGet-pakken for dette prosjektet er en pre-release, må du sørge for at det står "Include Prerelease" i drop-down menyen rett over søkeresuløtatene (der det står "Stable Only").
4. Velg "Sikker Digital Post Klientbibliotek" og trykk "Install".

##Eksempelkode

Det er satt opp et eksempelprosjekt som viser bruk av klienten til å definere de ulike entitetene som må opprettes før sending av digital post. 
Dette prosjektet finner du [her](https://github.com/difi/sikker-digital-post-net-klient-demo). Per dags dato er det kun Program.cs som er i bruk i eksempelprosjektet.


##Hvordan legge til signering og kryptering

### Legg inn databehandlersertifikat i certificate store

1.  Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
2.  Velg at sertifikatet skal lagres i _Current User_ og trykk _Next_
3.  Filnavn skal nå være utfylt. Trykk _Next_
4.  Skriv inn passord for privatekey og velg _Mark this key as exportable ..._, trykk _Next_
5.  Velg _Automatically select the certificate store based on the type of certificate_
6.  _Next_ og _Finish_
7.  Får du spørsmål om å godta sertifikatet så gjør det.
8.  Du skal da få en dialog som sier at importeringen var vellykket. Trykk _Ok_


### Legg inn mottakersertifikat i certificate store

1.  Start mmc.exe (Trykk windowstast og skriv _mmc.exe_)
2.  Velg _File_ -> _Add/Remove Snap-in..._ 
3.  Merk _Certificates_ og trykk _Add >_
4.  Velg _My user account_ og trykk _Finish_
5.  Åpne noden _Certificates - Current User_
6.  Høyreklikk på _Trusted People_ og velg _All Tasks_ -> _Import..._
7.  Trykk _Next_
8.  Finn mottaker-sertifikatet (Sertifikatnavn.cer) og legg det til. Trykk _Next_
9.  Sertifikatet skal legges til i _Trusted People_
10. _Next_ og _Finish_
11. Du skal da få en dialog som sier at importeringen var vellykket. Trykk Ok

## Logging

Klientbiblioteket har støtte for logging av informasjon rundt generering av meldingen som skal sendes samt svarene som motas fra meldingsformidleren. Ut av boksen så logges dette til en TraceSource med navn "SikkerDigitalPost.Klient". For å få frem denne kan man legge til litt xml i applikasjonens app- eller web.config.

**Eksempel:**

```xml
<system.diagnostics>
  <sources>
    <source name="SikkerDigitalPost.Klient" switchName="SdpSwitch">
      <listeners>
        <add name="logFile" />
      </listeners>
    </source>
  </sources>
  <switches>
    <add name="SdpSwitch" value="Verbose" />
  </switches>
  <sharedListeners>
    <add name="logFile" type="System.Diagnostics.TextWriterTraceListener" initializeData="sdp.txt"/>
  </sharedListeners>
  <trace autoflush="true">
    <listeners>
      <add name="logFile" />
    </listeners>
  </trace>
</system.diagnostics>
```

Dersom du ønsker å benytte et annet rammeverk for logging, f.eks log4net kan du endre metoden som benyttes for å logge ved hjelp av Klientkonfigurasjon klassens Logger property. 

**Eksempel:**

```c#
var klientkonfigurasjon = new Klientkonfigurasjon
{
    Logger = (severity, konversasjonsId, metode, melding) =>
    {
        System.Diagnostics.Debug.WriteLine("{0} - {1} [{2}]", DateTime.Now, melding, konversasjonsId.GetValueOrDefault());
    }
};
```

I tillegg så kan man få logging av alt som dreier seg om signering da klientbiblioteket benytter .NET sin innebygde SignedXml som implementerer logging gjennom en trace source med navn "System.Security.Cryptography.Xml.SignedXml".

## Ytelse

Klientbiblioteket benytter en HttpWebRequest for å kommunisere med meldingsformidleren. I en console applikasjon er denne begrenset til maks to samtidige forbindelser om gangen, mens den i en asp.net applikasjon er begrenset til ti. Dersom du ønsker å sende fler brev samtidig kan denne verdien endres f.eks til 20 gjennom:

```c#
System.Net.ServicePointManager.DefaultConnectionLimit = 20;
```

Se [ServicePointManager.DefaultConnectionLimit](http://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.defaultconnectionlimit(v=vs.110).aspx) for mer informasjon.
