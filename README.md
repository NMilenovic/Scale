Za pokretanje aplikacije neophodno je:

- dotnet 8.0 (Za pokretanje API-a).
- MongoDB C# Driver (Dontnet automatski preuzima pri prvom pokretanju).
- MongoDB Comunity server (Aplikacija očekuje da se koristi kao servis, podignut na podrazumevanom portu).
- Live server ekstenzija za Visual Studio Code (za hostovanje frontend aplikacije).


Pokretanje:
- Pokrenuti komandu u API folderu "dotnet watch run", za pokretanje API-a. Prilikom prvog pokretanja preuzeće se neophodne zavisnosti.  
- Na adresi localhost:5275 iskrostiti endpoint-ove: 
  - Register (Za kreiranje korisnika)
  - AddGenre (Dodavanje žanra)
  - AddArtist (Dodavanje izvođača)
  - AddAlbum (Dodavanje albuma)
  - AddFeatureReview (Nakon što krisinik doda recenziju za neki album, može se postaviti na početnu stranu aplikacije)
- Pokrenuti live server.

