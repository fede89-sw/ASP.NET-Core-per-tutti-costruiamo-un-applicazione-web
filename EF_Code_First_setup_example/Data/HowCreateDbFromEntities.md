```
- Crea le tue Entità, ovvero le classi .cs che saranno le tabelle de Db
- Crea il dbContext
- per creare il database devi fare le migrazioni,quindi da terminale:
    dotnet ef migrations add PrimaMigrazione
- devi avere installato dotnet ef, puoi farlo con:
    dotnet tool install --global dotnet-ef
- e per fare le migrazioni devi avere installato nel progetto il package NuGet:
    dotnet add package Microsoft.EntityFrameworkCore.Design
- per aggiornare il database se fai modifiche alle Entità usa il comando:
    dotnet ef database update
```