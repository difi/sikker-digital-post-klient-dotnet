---
title: Installere sertifikater
layout: default
description: Installere sertifikater for signering og kryptering av post
isHome: false
---

For å sende sikker digital post trenger du å installere sertifikater. En enkel oppsettsguide finner du her.   

<h3 id="databehandlersertifikat">Legg inn databehandlersertifikat i certificate store</h3>

1.  Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
2.  Velg at sertifikatet skal lagres i _Current User_ og trykk _Next_
3.  Filnavn skal nå være utfylt. Trykk _Next_
4.  Skriv inn passord for privatekey og velg _Mark this key as exportable ..._, trykk _Next_
5.  Velg _Automatically select the certificate store based on the type of certificate_
6.  _Next_ og _Finish_
7.  Får du spørsmål om å godta sertifikatet så gjør det.
8.  Du skal da få en dialog som sier at importeringen var vellykket. Trykk _Ok_

<h3 id="mottakersertifikat">Legg inn mottakersertifikat i certificate store</h3>

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