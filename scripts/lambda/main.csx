//Per fare il debug di questi esempi, devi prima installare il global tool dotnet-script con questo comando:
//dotnet tool install -g dotnet-script
//Trovi altre istruzioni nel file /scripts/readme.md
// Prove di Lambda Expression

// Func è il tipo ricevuto da una Lambda che ritorna un valore
Func<DateTime, bool> canDrive = dob => {
    return dob.AddYears(18) <= DateTime.Today;
};

DateTime dob = new DateTime(2005, 12, 25);
bool result = canDrive(dob);
Console.WriteLine(result);


// Action è il tipo ricevuto da una Lambda che NON ritorna un valore
Action<DateTime> printDate = date => Console.WriteLine(date);

DateTime date = DateTime.Today;
printDate(date);


// Lambda che restituisce 2 stringhe concatenate
Func<string, string, string> Concatenate = (nome, cognome) => {
    return nome + cognome;
};

string nome = "Mario";
string cognome = "Rossi";
string stringaConcatenata = Concatenate(nome, cognome);
Console.WriteLine(stringaConcatenata);


// Lambda che restituisce il massimo tra 3 numeri
Func<int, int, int, int> getMax = (numero1, numero2, numero3) => {
    if(numero1 > numero2 && numero2 > numero3){
        return numero1;
    }
    else if(numero1 > numero2 && numero2 < numero3){
        if(numero1 > numero3){
            return numero1;
        }
        else{
            return numero3;
        }
    }
    else if (numero1 < numero2 && numero2 > numero3){
        return numero2;
    }
    else {
        return numero3;
    }
};

int numero1 = 10;
int numero2 = 15;
int numero3 = 11;
int max = getMax(numero1, numero2, numero3);
Console.WriteLine(max);

// lambda che prende due parametri DateTime e non restituisce nulla, ma stampa la minore delle due date in console
Action<DateTime, DateTime> printLowerDate = (data1, data2) => {
    if(data1 > data2){
        Console.WriteLine(data2);
    }
    else {
       Console.WriteLine(data1); 
    }
};

DateTime data1 = new DateTime(2000, 5, 10);
DateTime data2 = new DateTime(2000, 7, 10);
printLowerDate(data1, data2);