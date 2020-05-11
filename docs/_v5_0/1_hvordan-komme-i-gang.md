---
title: Hvordan komme i gang
identifier: hvordankommeigang
layout: default
description: En liten oppstart for å komme i gang.
isHome: false
---

Klienten er tilgjengelig som en NuGet-pakke. Denne vil oppdateres jevnlig etter hvert som ny funksjonalitet legges til.

For å installere NuGet-pakken, gjør følgende i Visual Studio/Rider:

1. Velg _TOOLS -> NuGet Package Manager -> Manage Nuget Packages for Solution..._
1. Søk etter _Difi.SikkerDigitalPost.Klient_. Flere pakker vil dukke opp. Installer de som er relevant for deg. Pass på å _IKKE_ installer `difi-sikker-digital-post-klient` pakken. Den er ett .NET Framework bibliotek med uheldig likt navn.
Hvis du leter etter .NET Framework dokumentasjonen, se versjon [2](http://difi.github.io/sikker-digital-post-klient-dotnet/v2/).
	* Ønsker du pre-release, må du sørge for at det står _Include Prerelease_ i drop-down menyen rett over søkeresuløtatene (der det står _Stable Only_).
1. Velg _Difi.SikkerDigitalPost.Klient.X_ og trykk _Install_.


### Installer og bruk databehandlersertifikat
Som bruker av dette biblioteket er du en Databehandler som har ansvar for sending av meldinger. For å gjøre dette trenger du et sertifikat for å kunne autentisere deg mot Meldingsformidleren. Du kan lese mer om aktørene hos [begrep.difi.no](http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer). 
Dette bør installeres på maskinen som skal bruke klientbiblioteket. Grunnen til at vi ønsker å installere det er for å ikke ha passord i klartekst i koden.

<blockquote>SSL Certificates are small data files that digitally bind a cryptographic key to an organization's details. When installed on a web server, it activates the padlock and the https protocol (over port 443) and allows secure connections from a web server to a browser.</blockquote>

For å kommunisere over HTTPS trenger du å signere dine requests med ett databehandlersertifikat. Dette sertifikatet kan lastes direkte fra en fil eller fra Windows Certificate Store. 

Følgende steg vil installere sertifikatet i din Certificate Store. Dette burde gjøres på serveren hvor din applikasjon skal kjøre.

For mer informasjon, se [Microsoft Dokumentasjonen](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows#how-the-secret-manager-tool-works).

Filplasseringen og passord til sertifikatet må legges ett trygt sted.

Filplassering i Windows er:
```
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
```

Filplassering i MacOS/Linux er:
```
~/.microsoft/usersecrets/<user_secrets_id>/secrets.json
```

Legg til følgende `UserSecretsId` element i din `.csproj` fil:
``` xml
<PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
     <UserSecretsId>enterprise-certificate</UserSecretsId>
</PropertyGroup>
```

Dette betyr at elementet `<user_secrets_id>` in the filplasseringen vil være `enterprise-certificate`.

Fra command linjen, naviger til filplassering hvor din hoved `.csproj` fil ligger og kjør følgene kommandoer med dine egne sertifikat verdier:
```
dotnet user-secrets set "Certificate:Path:Absolute" "<your-certificate.p12>"
dotnet user-secrets set "Certificate:Password" "<your-certificate-password>"
```

#### Stol på sertifikatet i Windows:
1.  Dobbeltklikk på sertifikatet (Sertifikatnavn.p12)
1.  Velg at sertifikatet skal lagres i `Current User` eller `Local Machine` og trykk _Next_
If you are running the client library from a system account, but debugging from a different user, please install it on `Local Machine`, as this enables loading it from any user.
1.  Filnavn skal nå være utfylt. Trykk _Next_
1.  Skriv inn passord for privatnøkkel og velg _Mark this key as exportable ..._, trykk _Next_
1. Velg _Automatically select the certificate store based on the type of certificate_
1. Klikk _Next_ og _Finish_
1. Får du spørsmål om å godta sertifikatkjeden så du gjør det.
1. Du skal da få en dialog som sier at importeringen var vellykket. Trykk _OK_.


#### Stol på sertifikatet i MacOS:
1. Åpne `Keychain Access`
1. Velg `login` keychain
1. Trykk på _File_ og deretter _Import_
1. Velg databehandlersertifikatet og legg den til


#### Stol på sertifikatet i Linux:
Last ned _root_ og _intermediate_ sertifikatene fra [Difi](https://begrep.difi.no/SikkerDigitalPost/1.2.6/sikkerhet/sertifikathandtering) for din databehandlersertifkat utgiver.
Merk navnendringen til å ha `.crt` på slutten for `update-ca-certificates`:
 
```
sudo cp Buypass_Class_3_Test4_Root_CA.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_Root_CA.crt
sudo cp Buypass_Class_3_Test4_CA_3.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_CA_3.crt
sudo update-ca-certificates
```
