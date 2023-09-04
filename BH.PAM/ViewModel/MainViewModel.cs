using BH.PAM.Data;
using BH.PAM.Model;
using BH.PAM.Model.Enums;
using BH.PAM.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Markup;

namespace BH.PAM.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Reservoir Reservoir;

        public event PropertyChangedEventHandler PropertyChanged;

        #region InputHandlers
        public double VolumeHandler
        {
            get { return Reservoir.Volume; }
            set
            {
                Reservoir.Volume = value;
                OnPropertyChanged();
            }
        }

        public double FluidContactDepthHandler
        {
            get { return Reservoir.FluidContactDepth; }
            set
            {
                Reservoir.FluidContactDepth = value;
                OnPropertyChanged();
            }
        }

        public int DimensionXHandler
        {
            get { return Reservoir.DimensionX; }
            set
            {
                Reservoir.DimensionX = value;
                OnPropertyChanged();
            }
        }

        public int DimensionYHandler
        {
            get { return Reservoir.DimensionY; }
            set
            {
                Reservoir.DimensionY = value;
                OnPropertyChanged();
            }
        }

        public double GridCellSizeXHandler
        {
            get { return Reservoir.GridCellSizeX; }
            set
            {
                Reservoir.GridCellSizeX = value;
                OnPropertyChanged();
            }
        }

        public double GridCellSizeYHandler
        {
            get { return Reservoir.GridCellSizeY; }
            set
            {
                Reservoir.GridCellSizeY = value;
                OnPropertyChanged();
            }
        }

        public string SelectedUnitHandler
        {
            get { return Reservoir.SelectedUnit; }
            set
            {
                Reservoir.SelectedUnit = value;
                OnPropertyChanged();
            }
        }

        public Reservoir ReservoirHandler
        {
            get { return Reservoir; }
            set { Reservoir = value; }
        }
        #endregion

        public ICommand CalculateCommand { get; set; }
        private readonly IUnitConversionService _unitConversionService;
        private readonly IDialogService _dialogService;

        public MainViewModel(
            IUnitConversionService unitConversionService,
            IDialogService dialogService)
        {
            _unitConversionService = unitConversionService;
            _dialogService = dialogService;
            Reservoir = new Reservoir
            {
                DimensionX = 16,
                DimensionY = 26,
                GridCellSizeX = 200,
                GridCellSizeY = 200,
                FluidContactDepth = 3000
            };
            CalculateCommand = new RelayCommand(CalculateVolume);
        }

        public int ValidateData()
        {
            if (Reservoir.TopHorizonData == null)
            {
                _dialogService.ShowMessage(@"Load top horizon data first!
Example file is in the project folder called: topHorizonData");
                return -1;
            }
            if (Reservoir.BaseHorizonData == null)
            {
                _dialogService.ShowMessage(@"Load base horizon data first!
Example file is in the project folder called: topHorizonData");
                return -1;
            }
            if (Reservoir.SelectedUnit == null)
            {
                _dialogService.ShowMessage(@"Select a unit for the result!");
                return -1;
            }
            return 1;
        }

        private void CalculateVolume()
        {
            if (ValidateData() == -1)
                return;
            double volume = 0.0;
            double height = 0;

            for (int i = 0; i < Reservoir.DimensionY; i++)
            {
                for (int j = 0; j < Reservoir.DimensionX; j++)
                {
                    double topDepth = _unitConversionService.ConvertUnits(Reservoir.TopHorizonData[i, j],
                        VolumeType.Feets, VolumeType.Meters);
                    double baseDepth = _unitConversionService.ConvertUnits(Reservoir.BaseHorizonData[i, j],
                        VolumeType.Feets, VolumeType.Meters);

                    if (topDepth - Reservoir.FluidContactDepth > baseDepth - topDepth)
                        height = baseDepth - topDepth;
                    else if (topDepth - Reservoir.FluidContactDepth < 0)
                        height = 0;
                    else
                        height = topDepth - Reservoir.FluidContactDepth;

                    volume += Reservoir.GridCellSizeX * Reservoir.GridCellSizeY * height;
                }
            }

            if (Reservoir.VolumeType == VolumeType.Meters)
                VolumeHandler = volume;
            else
                VolumeHandler = _unitConversionService.ConvertUnits(volume, VolumeType.Meters, Reservoir.VolumeType);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
