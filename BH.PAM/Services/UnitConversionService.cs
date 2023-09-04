using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.PAM.Model.Enums;
using BH.PAM.Services.Interfaces;

namespace BH.PAM.Services
{
    public class UnitConversionService : IUnitConversionService
    {
        public double ConvertUnits(double units, VolumeType from, VolumeType to)
        {
            double[][] factor =
                {
                new double[] { 1, 3.28084, 6.2898107704},
                new double[] { 0.3048, 1, 0.178108},
                new double[] { 0.16, 5.61, 1}
            };
            return units * factor[(int)from][(int)to];
        }
    }
}
