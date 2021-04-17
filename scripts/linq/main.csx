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
};