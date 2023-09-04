namespace BH.PAM.Data
{
    public interface IDataAccess
    {
        double[,] GetTopHorizonData(string filePath);
    }
}