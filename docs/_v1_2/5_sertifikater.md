---
title: Installere sertifikater
identifier: installeresertifikater
layout: default
description: Installere sertifikater for signering og kryptering av post
isHome: false
---

For å sende sikker digital post trenger du å installere sertifikater. Grunnen til at vi ønsker å installere de er først og fremst sikkerhet. Alle sertifikater har en unik identifikator som kalles thumbprint. Hvis du ikke ønsker å håndtere selv i koden hvordan sertifikatene skal lastes, så kan du følge guiden under, steg for steg. Til slutt gjennomgås det hvordan du kan finne thumbprint til de installerte sertifikatene.

<h3 id="databehandlersertifikat">Legg inn databehandlersertifikat i certificate store</h3>

<blockquote> Databehandlersertifikat er det sertifikatet du har fått utstedt for å kunne sende post.  </blockquote>

1.  Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
2.  Velg at sertifikatet skal lagres i _Current User_ og trykk _Next_
3.  Filnavn skal nå være utfylt. Trykk _Next_
4.  Skriv inn passord for privatekey og velg _Mark this key as exportable ..._, trykk _Next_
5.  Velg _Automatically select the certificate store based on the type of certificate_
6.  _Next_ og _Finish_
7.  Får du spørsmål om å godta sertifikatet så gjør det.
8.  Du skal da få en dialog som sier at importeringen var vellykket. Trykk _Ok_.

<h3 id="mottakersertifikat">Legg inn mottakersertifikat i certificate store</h3>

<blockquote> Mottakerens sertifikat vil være sertifikatet til Digipost eller E-Boks og brukes for å validere respons.</blockquote>

Hvis du skal kjøre mot Digiposts testmiljø (https://qaoffentlig.meldingsformidler.digipost.no/api/ebms), så skal du bruke [Test_Digipost.cer](https://github.com/difi/sikker-digital-post-klient-dotnet/blob/master/docs/cert/Test_Digipost.cer). Høyreklikk og lagre til disk.  

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
11. Du skal da få en dialog som sier at importeringen var vellykket. Trykk _Ok_.

<h3 id="finneinstallertsertifikat">Finne installert sertifikat</h3>

For å bruke sertifikatene i kode så må du finne _thumbprint_ til hver av de. Dette gjøres lettest gjennom _Microsoft Management Console_ (mmc.exe).

<code>SikkerDigitalPostKlient</code> har støtte for å ta in _thumbprint_ direkte for <code>Databehandler</code> og <code>PostMottaker</code>. Du finner _thumbprint_ på følgende måte:

1.  Start mmc.exe (Trykk windowstast og skriv _mmc.exe_)
2.  Velg _File_ -> _Add/Remove Snap-in..._ 
3.  Merk _Certificates_ og trykk _Add >_
4.  Velg _My user account_ og trykk _Finish_
5.	Åpne mappe for sertifikat
	a. Databehandlersertifikat: Åpne noden _Certificates - Current User - Personal - Certificates_
	b. Mottakersertifikat: Åpne noden _Certificates - Current User - Trusted People - Certificates_
6. 	Dobbeltklikk på sertifikatet du installerte
7.	Velg _Details_, scroll ned til _Thumbprint_ og kopier
8.	VIKTIG: Hvis du får problemer i kode med at sertifikat ikke finnes, så kan det hende du får med en usynling _BOM_(Byte Order Mark). Slett derfor denne med å sette peker før første tegn i thumbprint i en teksteditor og trykk backspace. Hvis det var en BOM der så forsvant ikke det første synlige tegnet i thumbprint. 

Ønsker du å sende inn sertifikater du har allerede har initialisert, kan du kalle konstruktøren som tar inn <code> X509Certificate2</code>.

Som bruker av dette biblioteket er du en Databehandler som har ansvar for sending av meldinger. For å gjøre dette trenger du et sertifikat for å kunne autentisere deg mot Meldingsformidleren. Du kan lese mer om aktørene [her](http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer).
