using System.ComponentModel.Design;

string[] parkeringsplatser = new string[100];
bool korProgrammet = true;

while (korProgrammet) // Vi skapar en while-loop som körs så länge korProgrammet är true.  
{
    Console.Clear();
    Console.WriteLine("================================");
    Console.WriteLine("        PRAG PARKERING           ");
    Console.WriteLine("================================");
    Console.WriteLine("     1. Visa parkeringsplatser");
    Console.WriteLine("     2. Parkera fordon");
    Console.WriteLine("     3. Flytta fordon");
    Console.WriteLine("     4. Ta bort fordon");
    Console.WriteLine("     5. Sök efter fordon");
    Console.WriteLine("     6. Avsluta");
    Console.WriteLine(" ---------------------------------");
    Console.WriteLine("Välj ett alternativ mellan 1-6:");

    string val = Console.ReadLine(); // Vi läser in användarens val och sparar det i variabeln "val". 

    switch (val)
    {
        #region Case 1: Visa parkeringsplatser
        case "1":
            VisaParkeringsplatser(parkeringsplatser);
            break;
        #endregion

        #region Case 2: Parkera fordon  
        case "2":
            ParkeraFordon(parkeringsplatser);
            break;
        #endregion

        #region Case 3: Flytta fordon
        case "3":
            FlyttaFordon(parkeringsplatser);
            break;
        #endregion

        #region Case 4: Ta bort fordon/Upphämtning
        case "4":
            TaBortFordon(parkeringsplatser);
            break;
        #endregion

        #region Case 5: Sök efter fordon
        case "5":
            SokFordon(parkeringsplatser);
            break;
        #endregion

        #region Case 6: Programmet Avslutas
        case "6":
            korProgrammet = false;
            Console.WriteLine("Programmet avslutas...");
            break;
        #endregion
    }
}

// ==========================================
// Metoderna ligger längst ner:
// ==========================================

#region Metod: Visa Parkeringsplats
static void VisaParkeringsplatser(string[] parkeringsplatser)
{
    Console.WriteLine("\n=== PARKERINGSPLATSER ===");
    for (int j = 0; j < parkeringsplatser.Length; j++)
    {
        string status = string.IsNullOrEmpty(parkeringsplatser[j]) ? "Ledig" : parkeringsplatser[j];
        string platsNamn = $"Plats {j + 1}:".PadRight(10);
        Console.WriteLine($"{platsNamn} {status}");
    }
    Console.WriteLine("\nTryck på valfri knapp för att komma till menyn...");
    Console.ReadKey();
}

static void ParkeraFordon(string[] parkeringsplatser)
{
    Console.WriteLine("\n---- PARKERA FORDON ----");
    Console.Write("Ange registreringsnummer: ");
    string regNR = Console.ReadLine().Trim().ToUpper();

    if (string.IsNullOrEmpty(regNR) || regNR.Length > 10)
    {
        Console.WriteLine("Ogiltigt registreringsnummer! Det får max vara 10 tecken.");
    }
    else
    {
        Console.Write("Är det en Bil (B) eller en Motorcykel (M)? ");
        string typ = Console.ReadLine().Trim().ToUpper();

        if (typ == "B" || typ == "M")
        {
            int ledigIndex = FindFreeSpot(parkeringsplatser, typ);
            if (ledigIndex != -1)
            {
                if (typ == "B")
                {
                    parkeringsplatser[ledigIndex] = $"BIL:{regNR}";
                    Console.WriteLine($"Bilen {regNR} har parkerats på plats {ledigIndex + 1}.");
                }
                else if (typ == "M")
                {
                    if (string.IsNullOrEmpty(parkeringsplatser[ledigIndex]))
                    {
                        parkeringsplatser[ledigIndex] = $"MC:{regNR}";
                    }
                    else
                    {
                        parkeringsplatser[ledigIndex] += $",{regNR}";
                    }
                    Console.WriteLine($"Motorcykeln {regNR} har parkerats på plats {ledigIndex + 1}.");
                }
            }
            else
            {
                Console.WriteLine("Tyvärr, det finns ingen ledig plats för detta fordon.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltig fordonstyp. Välj B eller M.");
        }
    }

    Console.WriteLine("\nTryck på valfri knapp för att komma till menyn...");
    Console.ReadKey();
}

#endregion

#region Metod: Flytta fordon
static void FlyttaFordon(string[] parkeringsplatser)
{
    Console.WriteLine("\n---- FLYTTA FORDON ----");
    Console.Write("Ange registreringsnummer på fordonet som ska flyttas: ");
    string regNR = Console.ReadLine().Trim().ToUpper();

    int nuvarandeIndex = FindVehicle(parkeringsplatser, regNR);

    if (nuvarandeIndex == -1)
    {
        Console.WriteLine("Fordonet kunde inte hittas på parkeringen.");
    }
    else
    {
        string platsInnehall = parkeringsplatser[nuvarandeIndex];
        string typ = platsInnehall.StartsWith("MC") ? "M" : "B";

        // 1. LETA NY PLATS FÖRST (medan fordonet fortfarande står kvar på sin gamla plats!)
        int nyIndex = FindFreeSpot(parkeringsplatser, typ);

        if (nyIndex != -1)
        {
            // 2. Ta bort från gamla platsen
            TaBortFordonFraanPlats(parkeringsplatser, nuvarandeIndex, regNR);

            // 3. Sätt in på den nya platsen
            if (typ == "B")
            {
                parkeringsplatser[nyIndex] = $"BIL:{regNR}";
            }
            else
            {
                if (string.IsNullOrEmpty(parkeringsplatser[nyIndex]))
                    parkeringsplatser[nyIndex] = $"MC:{regNR}";
                else
                    parkeringsplatser[nyIndex] += $",{regNR}";
            }

            Console.WriteLine($"Fordonet {regNR} har flyttats från plats {nuvarandeIndex + 1} till plats {nyIndex + 1}.");
        }
        else
        {
            Console.WriteLine("Det finns tyvärr ingen ledig plats att flytta fordonet till.");
        }
    }

    Console.WriteLine("\nTryck på valfri knapp för att komma till menyn...");
    Console.ReadKey();
}
#endregion

#region Metod: Ta bort fordon (uthämtning) 
static void TaBortFordon(string[] parkeringsplatser)
{
    Console.WriteLine("\n---- TA BORT FORDON (UTHÄMTNING) ----");
    Console.Write("Ange registreringsnummer på fordonet som ska hämtas ut: ");
    string regNR = Console.ReadLine().Trim().ToUpper();

    int index = FindVehicle(parkeringsplatser, regNR);

    if (index != -1)
    {
        string borttagetFordon = parkeringsplatser[index];
        TaBortFordonFraanPlats(parkeringsplatser, index, regNR);
        Console.WriteLine($"Fordonet {borttagetFordon} har hämtats från plats {index + 1}. Platsen är nu ledig.");
    }
    else
    {
        Console.WriteLine("Inget fordon med det registreringsnumret hittades på parkeringen.");
    }

    Console.WriteLine("\nTryck på valfri knapp för att komma till menyn.");
    Console.ReadKey();
}
#endregion

#region Hjälpmetoder: Hitta fordon, hitta ledig plats, ta bortfordon från plats
static void SokFordon(string[] parkeringsplatser)
{
    Console.WriteLine("\n---- SÖK FORDON ----");
    Console.Write("Ange registreringsnummer att söka efter: ");
    string sokTerm = Console.ReadLine().Trim().ToUpper();
    bool hittad = false;

    for (int j = 0; j < parkeringsplatser.Length; j++)
    {
        if (!string.IsNullOrEmpty(parkeringsplatser[j]) && parkeringsplatser[j].ToUpper().Contains(sokTerm))
        {
            Console.WriteLine($"Fordonet hittades! Den står på parkeringsplats {j + 1}. ");
            hittad = true;
            break;
        }
    }

    if (!hittad)
    {
        Console.WriteLine("Inget fordon med angivet registreringsnummer hittades på parkeringen.");
    }

    Console.WriteLine("\nTryck på valfri knapp för att komma till menyn.");
    Console.ReadKey();
}

static void TaBortFordonFraanPlats(string[] parkeringsplatser, int index, string regNR)
{
    string str = parkeringsplatser[index];

    if (str.Contains(","))
    {
        string[] delar = str.Split(',');
        if (delar[0].Contains(regNR))
        {
            parkeringsplatser[index] = "MC:" + delar[1];
        }
        else
        {
            parkeringsplatser[index] = delar[0];
        }
    }
    else
    {
        parkeringsplatser[index] = null;
    }
}

static int FindVehicle(string[] parkeringsplatser, string regNr)
{
    for (int i = 0; i < parkeringsplatser.Length.ToString().Length; i++) 
    {
        
    }
    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (!string.IsNullOrEmpty(parkeringsplatser[i]) && parkeringsplatser[i].ToUpper().Contains(regNr.ToUpper()))
        {
            return i;
        }
    }
    return -1;
}

static int FindFreeSpot(string[] parkeringsplatser, string typ)
{
    if (typ == "M")
    {
        for (int i = 0; i < parkeringsplatser.Length; i++)
        {
            if (!string.IsNullOrEmpty(parkeringsplatser[i]) && parkeringsplatser[i].StartsWith("MC:") && !parkeringsplatser[i].Contains(","))
            {
                return i;
            }
        }
    }

    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (string.IsNullOrEmpty(parkeringsplatser[i]))
        {
            return i;
        }
    }

    return -1;
}

#endregion