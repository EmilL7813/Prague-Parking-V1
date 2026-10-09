Console.Clear();

// Centrerad titel
string titel = "=== PRAG PARKERING ===";
int blankaStegTitel = (Console.WindowWidth - titel.Length) / 2;
Console.WriteLine(titel.PadLeft(blankaStegTitel > 0 ? blankaStegTitel + titel.Length : titel.Length));

Console.WriteLine(); // Tom rad för luft

// Menyalternativ centrerade
string[] menyRader = {
    "1. Visa parkeringsplatser",
    "2. Parkera fordon",
    "3. Flytta fordon",
    "4. Ta bort fordon (uthämtning)",
    "5. Sök efter fordon",
    "6. Avsluta"
};

foreach (string rad in menyRader)
{
    int blankaSteg = (Console.WindowWidth - rad.Length) / 2;
    Console.WriteLine(rad.PadLeft(blankaSteg > 0 ? blankaSteg + rad.Length : rad.Length));
}

Console.WriteLine();
string valText = "Välj ett alternativ mellan 1-6: ";
// Om du vill ha en fast indragning för sista raden eller också centrera den:
int blankaStegVal = (Console.WindowWidth - valText.Length) / 2;
Console.Write(valText.PadLeft(blankaStegVal > 0 ? blankaStegVal + valText.Length : valText.Length));

string val = Console.ReadLine();