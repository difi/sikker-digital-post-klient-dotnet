---
title: Logging
id: logging
layout: default
description: Integrer SDP.NET mot din loggplattform
isHome: false
---

<h3 id="loggingrequestflow">Logging request flow</h3>
Klienten bruker _Common.Logging API_ som API for å abstrahere logging. Det er opp til brukeren å imlementere API med et passende loggrammeverk.

<blockquote>_Common Logging API_ er en lettvekts loggplattform som gjør at man lettere kan fokusere på krav til logger i stedet for hvilke loggverktøy og konfigurasjon man bruker. Dette gjør det lett å bytte loggrammeverk.</blockquote>

Settes loggnivå til `DEBUG` vil logge resultat for forespørsler som går bra og de  som feiler, `WARN` bare for feilede forespørsler eller verre, mens `ERROR`  bare skjer om sending av brev feilet. Disse loggerne vil være under `Difi.SikkerDigitalPost.Klient`

<h3 id="log4net">Implementere Log4Net som logger</h3>

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
      <datePattern value="yyyy.MM.dd' signature-api-client-dotnet.log'" />
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


<h3 id="lagrexmltilfil"> Lagre XML som sendes</h3>

Når det sendes brev gjennom Sikker Digital Post, så sendes legges det også ved en del ekstra informasjon. Denne informasjonen er strukturert som XML og er nødvending for at brevet skal leveres til mottaker. Ofte kan dette være svært nyttig informasjon å logge for å se hva som faktisk sendes. 

For å aktivere logging av XML så setter du følgende på <code>Klientkonfigurasjon</code>:

{% highlight csharp %}
Klientkonfigurasjon.LoggXmlTilFil = true;
{% endhighlight%}

Hvis <code> Klientkonfigurasjon.StandardLoggSti </code> ikke settes, så finner du loggfilene i _%AppData%/Digipost/Logg_.


<h3 id="dokumentpakkelogger">Lagre dokumentpakke som sendes</h3>

Som avsender kan det være ønskelig å lagre selve pakken med dokumenter som sendes. Denne inneholder også XML som sendes og er den faktiske pakken som krypteres og sendes. For å gjøre dette, setter du følgende på <code>Klientkonfigurasjon</code>:

{% highlight csharp %}
var transportkvittering = sikkerDigitalPostKlient.Send(forsendelse, lagreDokumentpakke: true);
{% endhighlight %}

Hvis <code> Klientkonfigurasjon.StandardLoggSti </code> ikke settes, så finner du dokumentpakkene i _%AppData%/Digipost/Dokumentpakke_.