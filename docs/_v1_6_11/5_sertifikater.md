---
title: Installere sertifikater
identifier: installeresertifikater
layout: default
description: Installere sertifikater for signering og kryptering av post
isHome: false
---

Som bruker av dette biblioteket er du en Databehandler som har ansvar for sending av meldinger. For å gjøre dette trenger du et sertifikat for å kunne autentisere deg mot Meldingsformidleren. Du kan lese mer om aktørene [hos begrep.difi.no](http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer). Dette bør installeres på maskinen. Grunnen til at vi ønsker å installere det er først og fremst sikkerhet.

Alle sertifikater har en unik identifikator som kalles thumbprint. Hvis du ikke ønsker å håndtere selv i koden hvordan sertifikatene skal lastes, så kan du følge guiden under, steg for steg. Til slutt gjennomgås det hvordan du kan finne thumbprint til det installerte sertifikatet.

### Legg inn databehandlersertifikat i certificate store

> Databehandlersertifikat er det sertifikatet du har fått utstedt for å kunne sende post.

1. Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
1. Velg at sertifikatet skal lagres i _Current User_ eller _Local Machine og trykk _Next_
1. Filnavn skal nå være utfylt. Trykk _Next_
1. Skriv inn passord for privatekey og velg _Mark this key as exportable ..._, trykk _Next_
1. Velg _Automatically select the certificate store based on the type of certificate_
1. Klikk _Next_ og _Finish_
1. Får du spørsmål om å godta sertifikatkjeden så du gjør det.
1. Du skal da få en dialog som sier at importeringen var vellykket. Trykk _Ok_.

### Finne thumbprint til installert sertifikat

For å finne _thumbprint_ så er det lettest å gjøre det vha _Microsoft Management Console_ (mmc.exe).

1. Velg _File_ -> _Add/Remove Snap-in..._ 
1. Merk _Certificates_ og trykk _Add >_
1. Hvis sertifikatet ble installert i _Current User_ velges _My user account_, og hvis det er installert på _Local Machine_ så velges _Computer Account_. Klikk _Finish_ og så _OK_
1. Ekspander _Certificates_-noden, velg _Personal_ og åpne _Certificates_
1. Dobbeltklikk på sertifikatet du installerte
1. Velg _Details_, scroll ned til _Thumbprint_ og kopier

`SikkerDigitalPostKlient` har støtte for å ta in _thumbprint_ direkte for `Databehandler` og `PostMottaker`. Ønsker du å sende inn sertifikater du har allerede har initialisert, kan du kalle konstruktøren som tar inn `X509Certificate2`.
