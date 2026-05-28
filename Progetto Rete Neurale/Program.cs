using System;
using System.Globalization; 
using System.IO;           
using System.Linq;        

internal class Program
{
    private const double THRESHOLD = 0.5; 
    private const int FEATURES = 5; 
    
    static double[] weights = new double[FEATURES] { 0.7, 0.6, 0.5, 0.4, 0.3 };
    static double bias = 0.0;

    private static void Main(string[] args)
    {
        string scelta = "";
        Console.WriteLine("premere A se vuoi addestrare il percettrone o premere P se vuoi prevedere");
        scelta = Console.ReadLine();

        if (scelta.ToUpper() == "A")
        {
            Console.WriteLine("Avvio addestramento dal file input.txt...");          

            if (Addestramento("input.txt", ref weights, ref bias))
            {
                Console.WriteLine("\n--- Addestramento Completato con Successo ---");
                Console.WriteLine("Nuovi pesi e bias ottimizzati dal dataset:");
                for (int i = 0; i < FEATURES; i++)
                {
                    Console.WriteLine($"Peso {i + 1}: {weights[i]:F4}");
                }
                Console.WriteLine($"Bias: {bias:F4}");
            }
        }
        else if (scelta.ToUpper() == "P")
        {
            double[] input = new double[FEATURES]; 
            
            Console.WriteLine("Artista famoso? (1 per sì, 0 per no)");
            input[0] = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Bel meteo? (1 per sì, 0 per no)");
            input[1] = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Amici presenti? (1 per sì, 0 per no)");
            input[2] = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Cibo buono? (1 per sì, 0 per no)");
            input[3] = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Si può bere? (1 per sì, 0 per no)");
            input[4] = double.Parse(Console.ReadLine());

            int risultato = Prevedi(weights, bias, input);

            if (risultato == 1)
            {
                Console.WriteLine("Esci stasera!");
            }
            else
            {
                Console.WriteLine("Rimani a casa stasera.");
            }
        }
        else
        {
            Console.WriteLine("Scelta non valida. Riprova.");
        }
    }

    private static int activation(double x) 
    {
        if (x > THRESHOLD) return 1; 
        else return 0;
    }

    static int Prevedi(double[] weights, double bias, double[] input)
    {
        double somma = bias; 
        for (int i = 0; i < FEATURES; i++)
        {
            somma += input[i] * weights[i];
        }
        return activation(somma);
    }


    static bool Addestramento(string filename, ref double[] weights, ref double bias)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Errore: il file '{filename}' non è stato trovato nella cartella di esecuzione!");
                return false;
            }

            var lines = File.ReadAllLines(filename);
            double learningRate = 0.1; 
            int epoche = 30;           

            for (int epoca = 0; epoca < epoche; epoca++)
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 6) continue;

                    int startIndex = parts.Length - 6;

                    double[] inputs = new double[FEATURES];
                    for (int i = 0; i < FEATURES; i++)
                    {
                        inputs[i] = double.Parse(parts[startIndex + i], CultureInfo.InvariantCulture);
                    }

                    int target = int.Parse(parts[startIndex + FEATURES]);

                    int predizione = Prevedi(weights, bias, inputs);
                    int errore = target - predizione;

                    if (errore != 0)
                    {
                        for (int i = 0; i < FEATURES; i++)
                        {
                            weights[i] += learningRate * errore * inputs[i];
                        }
                        bias += learningRate * errore;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la lettura o l'elaborazione del dataset: {ex.Message}");
            return false;
        }
    }
}