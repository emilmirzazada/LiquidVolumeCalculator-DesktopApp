namespace BH.PAM.Data
{
    public interface IDataAccess
    {
        double[,] GetHorizonData(string filePath);
    }
}