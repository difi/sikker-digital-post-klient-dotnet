---
title: Installere sertifikater
id: installeresertifikater
layout: default
description: Installere sertifikater for signering og kryptering av post
isHome: false
---

For å sende sikker digital post trenger du å installere sertifikater. 

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

<blockquote> Mottakerens sertifikat vil være sertifikatet til Digipost eller E-Boks.</blockquote>

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

<h3 id="mottakersertifikat">Finne installert sertifikat</h3>

Som bruker av dette biblioteket er du en Databehandler som har ansvar for sending av meldinger. For å gjøre dette trenger du et sertifikat for å kunne autentisere deg mot Meldingsformidleren. Du kan lese mer om aktørene [her](http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer).


De installerte sertifikatene kan hentes inn vha. thumbprint. Dette finner du ved å åpne sertifikatet i Explorer og kopier verdien _Thumbprint_.

Sertifikatene kan hentes på følgende måte:
{% highlight csharp %}
//Sertifikat for databehandler
X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
X509Certificate2 databehandlersertifikat;
storeMy.Open(OpenFlags.ReadOnly);
databehandlersertifikat = storeMy.Certificates.Find(
	X509FindType.FindByThumbprint, hash, true)[0];
storeMy.Close();

 //Sertifikat for mottaker
 var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
 X509Certificate2 mottakerSertifikat;
 storeTrusted.Open(OpenFlags.ReadOnly);
 mottakerSertifikat = storeTrusted.Certificates.Find(
 	X509FindType.FindByThumbprint, hash, true)[0];
 storeTrusted.Close();
{% endhighlight %}



