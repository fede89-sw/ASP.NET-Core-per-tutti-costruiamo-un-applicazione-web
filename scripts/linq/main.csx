//Per fare il debug di questi esempi, devi prima installare il global tool dotnet-script con questo comando:
//dotnet tool install -g dotnet-script
//Trovi altre istruzioni nel file /scripts/readme.md
class Apple {
    public string Color { get; set; }
    public int Weight { get; set; } //In grammi
}

List<Apple> apples = new List<Apple> {
    new Apple { Color = "Red", Weight = 180 },
    new Apple { Color = "Green", Weight = 195 },
    new Apple { Color = "Red", Weight = 190 },
    new Apple { Color = "Green", Weight = 185 },
    new Apple { Color = "Green", Weight = 190 },
    new Apple { Color = "Red", Weight = 165 },
};

// un oggetto è un IEnumerable se è possibile ciclarlo con 'foreach'
// foreach(var apple in apples){
// }
// se VS non da errore vuol dire che è possibile ciclarlo
// oppure vai nelle definizioni di List e vedi se deriva da IEnumerable
// Il requisito minimo per usare LINQ è che una classe implementi IEnumerable e che permetta quindi l'accesso sequenziale

// seleziono solo le mele rosse
IEnumerable<Apple> redApples = apples.Where(apple => apple.Color == "Red");


// seleziono le mele rosse e ne prendo il peso mettendolo nella variabile
IEnumerable<int> weightOfRedApples = apples.Where(apple => apple.Color == "Red")
                                           .Select(apple => apple.Weight);

double averageRedApplesWeight = weightOfRedApples.Average();
Console.WriteLine($"Peso medio delle Mele Rosse: {averageRedApplesWeight}gr\n");


//ESERCIZIO #1: Qual è il peso minimo delle 4 mele?
var applesWeightList = apples.Select(apple => apple.Weight);
var minWeight = applesWeightList.Min();
Console.WriteLine($"Peso Mela più leggera: {minWeight}gr\n");


//ESERCIZIO #2: Di che colore è la mela che pesa 190 grammi?
var TargetWeightApples = apples.Where(apple => apple.Weight == 190)
                               .Select(apple => apple.Color);

Console.WriteLine($"Ci sono {TargetWeightApples.Count()} mele del peso di 190gr.");                        
foreach(string color in TargetWeightApples){
    Console.WriteLine($"Colore della Mela di 190gr: {color}");
}


//ESERCIZIO #3: Quante sono le mele rosse?
int redApplesCount = apples.Where(apple => apple.Color == "Red")
                           .Count();

Console.WriteLine($"\nCi sono in totale {redApplesCount} mele rosse!\n");


//ESERCIZIO #4: Qual è il peso totale delle mele verdi?
int totalWeight = apples.Where(apple => apple.Color == "Green")
                        .Select(apple => apple.Weight)
                        .Sum();

Console.WriteLine($"Il peso totale delle mele verdi è: {totalWeight}gr.");