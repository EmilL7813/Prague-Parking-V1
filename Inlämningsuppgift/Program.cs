
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

    #region Case 1: Visa parkeringsplatser

    switch (val)
    {



        case "1":
            Console.WriteLine("\n=== PARKERINGSPLATSER ===");
            for (int j = 0; j < parkeringsplatser.Length; j++)
            {
                string status = string.IsNullOrEmpty(parkeringsplatser[j]) ? "Ledig" : parkeringsplatser[j];

                // PadRight(8) ser till att ordet "Plats X" tar upp minst 8 tecken, 
                // vilket gör att kolon och status hamnar på exakt samma lodräta linje!
                string platsNamn = $"Plats {j + 1}:".PadRight(10);

                Console.WriteLine($"{platsNamn} {status}");
            }
            Console.WriteLine("\nTryck på valfri knapp för att komma till menyn...");
            Console.ReadKey();
            break;
    #endregion

    #region case 2: Parkera fordon  

        case "2": //Parkerar själva fordonet
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

                    bool parkerad = false;

                    if (typ == "B")
                    {
                        for (int j = 0; j < parkeringsplatser.Length; j++)
                        {
                            if (string.IsNullOrEmpty(parkeringsplatser[j]))
                            {
                                parkeringsplatser[j] = $"BIL:{regNR}";
                                Console.WriteLine($"Bilen {regNR} har parkerats på plats {j + 1}.");
                                parkerad = true;
                                break;
                            }
                        }
                    }
                    else if (typ == "M")
                    {
                        // Steg 1: Leta efter en plats med en MC som kan delas
                        for (int j = 0; j < parkeringsplatser.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(parkeringsplatser[j]) && parkeringsplatser[j].StartsWith("MC:") && !parkeringsplatser[j].Contains(","))
                            {
                                parkeringsplatser[j] += $",{regNR}";
                                Console.WriteLine($"Motorcykeln {regNR} har delat plats {j + 1} med en annan MC.");
                                parkerad = true;
                                break;
                            }
                        }

                        // Steg 2: Om ingen delad plats fanns, leta efter en helt tom plats
                        if (!parkerad)
                        {
                            for (int j = 0; j < parkeringsplatser.Length; j++)
                            {
                                if (string.IsNullOrEmpty(parkeringsplatser[j]))
                                {
                                    parkeringsplatser[j] = $"MC:{regNR}";
                                    Console.WriteLine($"Motorcykeln {regNR} har parkerats på plats {j + 1}.");
                                    parkerad = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig fordonstyp. Välj B eller M.");
                    }

                    if (!parkerad && (typ == "B" || typ == "M"))
                    {
                        Console.WriteLine("Tyvärr, det finns ingen ledig plats för detta fordon.");
                    }
                }

                Console.WriteLine("\nTryck på valfri knapp för att komma till menyn...");
                Console.ReadKey();
                break;
            }



        #endregion

    #region Case 3: Flytta fordon
        case "3": // Flyttar fordon när det behövs
            Console.WriteLine("\n----FLYTTA FORDON----");
            Console.Write("Ange nuvarande platsnummer (1-100): ");
            if (int.TryParse(Console.ReadLine(), out int franPlats) && franPlats >= 1 && franPlats <= 100)
            {
                int franIndex = franPlats - 1;

                if (!string.IsNullOrEmpty(parkeringsplatser[franIndex]))
                {
                    Console.Write("Ange ny platsnummer att flytta till (1-100): ");
                    if (int.TryParse(Console.ReadLine(), out int tillPlats) && tillPlats >= 1 && tillPlats <= 100)
                    {
                        int tillIndex = tillPlats - 1;

                        if (string.IsNullOrEmpty(parkeringsplatser[tillIndex]))
                        {
                            parkeringsplatser[tillIndex] = parkeringsplatser[franIndex];
                            parkeringsplatser[franIndex] = null;
                            Console.WriteLine($"Fordonet har flyttats från plats {franPlats} till {tillPlats}.");
                        }
                        else
                        {
                            Console.WriteLine("Målsplatsen är redan upptagen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt platsnummer. Välj mellan 1 och 100");
                    }
                }
                else
                {
                    Console.WriteLine("Platsen är redan tom.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt platsnummer. Välj mellan 1 och 100");
            }

            Console.WriteLine("\nTryck på valfri knapp för att komma till menyn.");
            Console.ReadKey();
            break;


        #endregion

    #region Case 4 Ta bort fordon/Upphämtning
        case "4": //Att ta ut fordon för uthämtning
            Console.WriteLine("\n---- TA BORT FORDON (UTHÄMTNING) ----");
            Console.Write("Vilken plats vill du hämta ut ifrån? (1-100)");

            if (int.TryParse(Console.ReadLine(), out int platsNummer) && platsNummer >= 1 && platsNummer <= 100)
            {
                int index = platsNummer - 1;

                if (!string.IsNullOrEmpty(parkeringsplatser[index]))

                {
                    string borttagetFordon = parkeringsplatser[index];
                    parkeringsplatser[index] = null;
                    Console.WriteLine($"Fordonet {borttagetFordon} har hämtats från plats {platsNummer} Platsen är nu ledig");

                }
                else

                {
                    Console.WriteLine("Platsen är redan tom");

                }
            }
            else
            {
                Console.WriteLine("Ogiltigt platsnummer. Välj mellan 1 och 100");

            }
            Console.WriteLine("\nTryck på valfri knapp för att komma till menyn.");
            Console.ReadKey();
            break;
        #endregion

    #region Case 5: Sök efter fordon
        case "5":
            Console.WriteLine("\n---- SÖK FORDON ----");
            Console.Write("Ange registreringsnummer att söka efter: ");
            string sokTerm = Console.ReadLine().Trim().ToLower();
            bool hittad = false;

            for (int j = 0; j < parkeringsplatser.Length; j++)
            {
                if(!string.IsNullOrEmpty(parkeringsplatser[j]) && parkeringsplatser[j].ToLower().Contains(sokTerm))
                {
                    Console.WriteLine($"Fordonet hittades! Den står på parkeringsplats {j + 1}. ");
                    hittad = true;
                    break;
                }
            }

            if (!hittad)
            {
                Console.WriteLine("Inget fordon med angivna registreringsnummer hittades på parkeringen");
                Console.WriteLine();
            }

            Console.WriteLine("\nTryck på valfri knapp för att komma till menyn.");
            Console.ReadKey();
            break;


        #endregion
        case "6":
            korProgrammet = false;
            Console.WriteLine("Programmet avslutas...");
            break;
    }
}







