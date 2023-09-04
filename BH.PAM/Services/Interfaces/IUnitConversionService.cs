using BH.PAM.Model.Enums;

namespace BH.PAM.Services.Interfaces
{
    public interface IUnitConversionService
    {
        double ConvertUnits(double units, VolumeType from, VolumeType to);
    }
}
