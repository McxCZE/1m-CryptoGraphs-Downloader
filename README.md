# 1m-CryptoGraphs-Downloader
 C# překódováno.

## Návod jak na to BINANCE

Stáhnu soubor z release. Složku extrahuji a spustím jako administrátor.

![image](https://user-images.githubusercontent.com/2656561/134644700-1daa0a0f-be85-4b13-904d-5b9b293dc92b.png)

Vyplním patřičné náležitosti toho co chci stahovat. 

![image](https://user-images.githubusercontent.com/2656561/134645151-9dcf356a-5dae-4b67-ba1b-010434c77d72.png)

Potvrdím enterem. A započnu stahování. 

![image](https://user-images.githubusercontent.com/2656561/134645308-02d2cb95-d388-437b-9b99-87eb97b1e28b.png)

V mé uživatelské složce dokumenty se mi vyrobí složka 1m-Grafy do které bude graf následně ukládán ve formátu **(Pár)_(Od)_(Do).csv**. Mezitím co program stahuje postavím si vodu na čaj. 

![image](https://user-images.githubusercontent.com/2656561/134645519-d1fd847e-4358-4eca-b81c-14f98531b91d.png)

Po úspěšném stáhnutí obdržím v programu tuto hlášku.
 
 ![image](https://user-images.githubusercontent.com/2656561/134645812-c58cd502-cfee-4f84-b04c-98cb6ed8523d.png)

Potvrdím enterem, program se vypnu, a mohu jít backtestit :)

## Návod jak na to přidání více párů do konfiguračního souboru

V případě že nechci po každé vypisovat co chci stahovat, je možnost přidat páry do **config.json** který je ve stejné složce jako **.exe** stahovátka. Formát je dle obrázku následující. Asi nejjednoduší a nejrychlejší je si zkopírovat předlohu, a při spuštění robota vybrat "A" jako ano, chci nechat stahovat dle konfiguračního souboru. Pak si akorát udělat čaj a vyčkat než se nastahují data. 

![image](https://user-images.githubusercontent.com/2656561/136189102-fdb05384-cd02-47af-be57-463f95644daa.png)


## Spustím to, aplikace problikne a nic se neděje ? 
 
Je potřeba s největší pravděpodobností stáhnout .NET Core 3.1 balíček, lze stáhnout zde : https://dotnet.microsoft.com/download/dotnet/3.1/runtime?utm_source=getdotnetcore&utm_medium=referral
