
** Versjonering
For å legge til ny versjon:
- kopier mappen med siste gjeldene versjon og lim det inn som en ny versjon _vX_X i root (cp -r _v1_3 _vX_X)
- gå inn i index.html, i mappen du lagde, og endre '{% for dok in site.v1_3 %}' til '{% for dok in site.vX_X %}'
- gå inn i _config.yml og sett riktig versjon i 'currentCollecion' og 'currentVersion'. Legg deretter til versjonen din i 'versions' og 'collections' 
