# Equilobe Library

Solutia curenta reprezinta un sistem de management pentru o biblioteca, astfel incat cititorii sa poata imprumuta carti. Sistemul tine atat evidenta cartilor aflate in biblioteca, cat si a celor care au fost imprumutate.

## Specificatii functionale

- Utilizatorul poate adauga carti noi, poate vedea cartile inregistrate in biblioteca si numarul de copii disponibile ale unei carti
- Cititorii pot imprumuta carti si le pot returna
- O carte imprumutata poate fi inapoiata in maximum 2 saptamani, in caz contrar, se plateste penalitate pentru fiecare zi de intarziere (1% din valoarea pretului de inchiriere / zi intarziata).
  
## Specificatii tehnice

- Solutia este implementata ca Web API
- Datele sunt stocate intr-o instanta SQLite, iar conectarea la aceasta se face prin intermediul EntityFramework Core
- Testele unitare sunt implementate folosind xUnit si Moq
