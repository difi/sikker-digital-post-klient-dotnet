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

### Legg inn avsendersertifikat i certificate store

1.  Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
2.  Velg at sertifikatet skal lagres i _Current User_ og trykk _Next_
3.  Filnavn skal nå være utfylt. Trykk _Next_
4.  Skriv inn passord for privatekey og velg _Mark this key as exportable ..._, trykk _Next_
5.  Velg _Automatically select the certificate store based on the type of certificate_


### Legg inn mottakersertifikat i certificate store

1.  Start mmc.exe (Trykk windowstast og skriv _mmc.exe_)
2.  Velg _File_ -> _Add/Remove Snap-in..._ 
3.  Merk _Certificates_ og trykk _Add >_
4.  Velg _My user account_ og trykk _Finish_
5.  Åpne noden _Certificates - Current User_
6.  Høyreklikk på _Trusted People_ og velg _All Tasks_ -> _Import..._
7.  Trykk _Next_
8.  Finn mottaker-sertifikatet og legg det til. Trykk _Next_
9.  Sertifikatet skal legges til i _Trusted People_
10. _Next_ og _Finish_
