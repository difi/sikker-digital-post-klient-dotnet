
##Kjøre dokumentasjon første gang
* [Sjekk guide på help.github](https://help.github.com/articles/using-jekyll-with-pages/)
* Her står det hvordan du kjører oppdatering av pakker osv. når det er lenge siden sist.


##For å legge til ny versjon:
* kopier mappen med siste gjeldene versjon og lim det inn som en ny versjon (f.eks. `_v1_0` i root (`cp -r _v1_0 _v1_1`)
* gå inn i index.html, i den nye mappen du lagde, og endre '{% for dok in site.v1_0 %}' til '{% for dok in site.v1_1 %}'
* gå inn i _config.yml og sett riktig versjon i `currentCollecion`, `currentVersion` og `versions`. Legg deretter til den nye versjonen i `collections`.
* I gammel _index.html_ settes `redirect_from:` (`/` fjernes) og i ny _index.html_ settes den til `redirect_from: /`


