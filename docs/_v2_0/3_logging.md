---
title: Logging
identifier: logging
layout: default
description: Integrer SDP.NET mot din loggplattform
isHome: false
---

### Generelt

Klienten bruker _Common.Logging API_ for å abstrahere logging. Det er opp til brukeren å imlementere API med et passende loggrammeverk, men vi viser hvordan dette kan gjøres med Log4Net.

Loggnivå `DEBUG` vil logge resultat for forespørsler som går bra og de  som feiler, `WARN` bare for feilede forespørsler eller verre, mens `ERROR`  bare skjer om sending av brev feilet. Disse loggerne vil være under `Difi.SikkerDigitalPost.Klient`

### Implementere Log4Net som logger

1. Installer Nuget-pakke `Common.Logging.Log4Net`. Denne vil da også installere avhengighetene `Common.Logging.Core` og `Common.Logging`. Merk at versjoneringen her er litt underlig, men et søk i Nuget Gallery vil f.eks. vise at Log4Net 2.0.3 har pakkenavn _Log4net [1.2.13] 2.0.3_. Da er det `Common.Logging.Log4Net1213` som skal installeres. 
2. Legg merke til hvilken versjon av Log4net som faktisk installeres. Av en eller annen grunn kan det bli 2.0.0 som installeres. Da må versjonen oppdateres til 2.0.3.

En fullstendig App.config med Log4Net-adapter og en `RollingFileAppender`:

{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>

  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter, Common.Logging.Log4net1213">
        <arg key="configType" value="INLINE" />
      </factoryAdapter>
    </logging>
  </common>

   <log4net>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <file value="${AppData}\Digipost\SikkerDigitalPost\" />
      <appendToFile value="true" />
      <rollingStyle value="Date" />
      <staticLogFileName value="false" />
      <rollingStyle value="Composite" />
      <param name="maxSizeRollBackups" value="10" />
      <datePattern value="yyyy.MM.dd' sikker-digital-post-klient-dotnet.log'" />
      <maximumFileSize value="100MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
   <root>
      <appender-ref ref="RollingFileAppender"/>
    </root>
  </log4net>
</configuration>

{% endhighlight %}


<h3 id="loggeforsporselogrespons"> Logge forespørsel og respons</h3>

Når det sendes brev gjennom Sikker Digital Post, så legges det også ved en del ekstra informasjon. Denne informasjonen er strukturert som XML og er nødvending for at brevet skal leveres til mottaker. Ofte kan dette være svært nyttig informasjon å logge. 

For å aktivere logging av forespørsel og respons så setter du følgende på <code>Klientkonfigurasjon</code>:

{% highlight csharp %}
Klientkonfigurasjon.LoggForespørselOgRespons = true;
{% endhighlight%}

Da  vil det logges til en logger med navn `Difi.SikkerDigitalPost.Klient.RequestResponse`.

<blockquote> Merk at logging av forespørsel og respons kan gi mye dårligere ytelse. Det er ingen grunn til å logge dette i et produksjonsmiljø.</blockquote>

<h3 id="dokumentpakkelogger">Prosessere dokumentpakke som sendes</h3>

Når man logger forespørsel og respons, så logges  bare XML som sendes, ikke selve dokumentpakken. Det er to måter å logge denne på:
1. Aktiver logging til disk vha <code>Klientkonfigurasjon.AktiverLagringAvDokumentpakkeTilDisk</code>.
2. Implementer <code>IDokumentPakkeProsessor</code> og legg til i <code>Klientkonfigurasjon.Dokumentpakkeprosessorer</code>