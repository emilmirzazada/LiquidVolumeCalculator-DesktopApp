namespace BH.PAM.Data;

public class DataAccess : IDataAccess
{
    public double[,] GetHorizonData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.Length;
        int columns = lines[0].Split(' ').Length;
        double[,] data = new double[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            string[] values = lines[i].Split(' ');
            for (int j = 0; j < columns; j++)
            {
                if (double.TryParse(values[j], out double value))
                {
                    data[i, j] = value;
                }
                else
                {
                    return null;
                }
            }
        }
        return data;
    }
}
