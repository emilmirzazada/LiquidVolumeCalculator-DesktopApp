namespace BH.PAM.StartupHelpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}