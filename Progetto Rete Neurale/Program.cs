internal class Program
{

    private double THRESHOLD = 0.5; 
    private int FEATURES = 5; 
    
    
  
    private int activation(float x) {
        if (x > THRESHOLD) return 1; 
        else return 0;
    }
    
    static int Prevedi(float[] weights, float bias, int[] input)
    {
        float somma = bias;
        for (int i = 0; i < FEATURES; i++)
        {
            somma += input[i] * weights[i];
        }
        return activation(somma);
    }

    static bool CaricaPesi(string filename, out float[] weights, out float bias)
    {
        weights = new float[FEATURES];
        bias = 0f;

        try
        {
            var lines = File.ReadAllLines(filename);

            weights = lines.Take(FEATURES)
                        .Select(l => float.Parse(l.Split(':')[1].Trim(), CultureInfo.InvariantCulture))
                        .ToArray();

            bias = float.Parse(lines[FEATURES].Split(':')[1].Trim(), CultureInfo.InvariantCulture);

            return true;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Errore: file {filename} non trovato!");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore nella lettura: {ex.Message}");
            return false;
        }
    }
  
}