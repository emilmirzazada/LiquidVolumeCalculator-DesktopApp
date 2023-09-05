using BH.PAM.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using BH.PAM.Data;
using BH.PAM;
using BH.PAM.StartupHelpers;
using BH.PAM.Services.Interfaces;
using Microsoft.Extensions.Logging;
using BH.PAM.Model.Enums;
using BH.PAM.ViewModel;
using System.Windows.Controls;

namespace BH.PAM
{
    public partial class MainWindow : Window
    {
        private readonly IDataAccess _dataAccess;
        private readonly IDialogService _dialogService;
        private readonly ILogger<MainWindow> _logger;
        private MainViewModel _viewModel;

        public MainWindow(
            IDataAccess dataAccess,
            IUnitConversionService unitConversionService,
            IDialogService dialogService,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MainWindow>();
            InitializeComponent();
            _dialogService = dialogService;
            _dataAccess = dataAccess;
            _viewModel = new MainViewModel(unitConversionService, dialogService);
            DataContext = _viewModel;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
           _viewModel.VolumeHandler = 0;
            if (radioButton == CubicMetersRadioButton)
            {
                _viewModel.ReservoirHandler.VolumeType = VolumeType.Meters;
                _viewModel.SelectedUnitHandler = VolumeType.Meters.ToString();
            }
            else if (radioButton == CubicFeetRadioButton)
            {
                _viewModel.ReservoirHandler.VolumeType = VolumeType.Feets;
                _viewModel.SelectedUnitHandler = VolumeType.Feets.ToString();
            }
            else if (radioButton == BarrelsRadioButton)
            {
                _viewModel.ReservoirHandler.VolumeType = VolumeType.Barrels;
                _viewModel.SelectedUnitHandler = VolumeType.Barrels.ToString();
            }
            _viewModel.CalculateCommand.Execute(null);
            DataContext = _viewModel;
        }

        private void LoadTopData_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenFileDialog();

            _viewModel.ReservoirHandler.TopHorizonData = _dataAccess.GetHorizonData(_dialogService.FilePath);
            DataContext = _viewModel;
            if (_viewModel.ReservoirHandler.TopHorizonData != null)
                _dialogService.ShowMessage("Success");
            else
                _dialogService.ShowMessage("Failed to load and parse data.");
        }

        private void LoadBaseData_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.OpenFileDialog();

            _viewModel.ReservoirHandler.BaseHorizonData = _dataAccess.GetHorizonData(_dialogService.FilePath);
            DataContext = _viewModel;
            if (_viewModel.ReservoirHandler.BaseHorizonData != null)
                _dialogService.ShowMessage("Success");
            else
                _dialogService.ShowMessage("Failed to load and parse data.");
        }
    }
}