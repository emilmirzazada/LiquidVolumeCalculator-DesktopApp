using BH.PAM.Model.Enums;
using System.ComponentModel;

namespace BH.PAM.Model
{
    public sealed class Reservoir
    {
        public double[,]? TopHorizonData { get; set; }
        public double[,]? BaseHorizonData { get; set; }
        public double Volume { get; set; }
        public double FluidContactDepth { get; set; }
        public int DimensionX { get; set; }
        public int DimensionY { get; set; }
        public double GridCellSizeX { get; set; }
        public double GridCellSizeY { get; set; }
        public VolumeType VolumeType { get; set; }
        public string SelectedUnit { get; set; }
    }
}