using BH.PAM.Data;
using BH.PAM.Model.Enums;
using BH.PAM.ViewModel;

namespace BH.PAM.Tests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void CalculateVolume_WithValidData_CalculatesCorrectly()
        {
            var unitConversionServiceMock = new Mock<IUnitConversionService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var dataAccessMock = new DataAccess();

            var rand = new Random();
            unitConversionServiceMock.SetupSequence(x => x.ConvertUnits(
                It.IsAny<double>(),
                VolumeType.Feets,
                VolumeType.Meters))
                .Returns(GetRandomNumberInRange(rand, 4000, 5000))
                .Returns(GetRandomNumberInRange(rand, 5000, 6000))
                .Returns(GetRandomNumberInRange(rand, 6000, 7000));

            var viewModel = new MainViewModel(unitConversionServiceMock.Object, dialogServiceMock.Object);
            viewModel.ReservoirHandler.TopHorizonData = dataAccessMock.GetHorizonData(Path.Combine(Directory.GetCurrentDirectory(), "topHorizonData.txt"));
            viewModel.ReservoirHandler.BaseHorizonData = dataAccessMock.GetHorizonData(Path.Combine(Directory.GetCurrentDirectory(), "baseHorizonData.txt"));
            viewModel.ReservoirHandler.SelectedUnit = VolumeType.Meters.ToString();

            viewModel.CalculateCommand.Execute(null);
            Assert.IsTrue(viewModel.VolumeHandler > 0);
        }

        [TestMethod]
        public void CalculateVolume_WithInvalidData_ShowsErrorMessage()
        {
            var unitConversionServiceMock = new Mock<IUnitConversionService>();
            var dialogServiceMock = new Mock<IDialogService>();

            var viewModel = new MainViewModel(unitConversionServiceMock.Object, dialogServiceMock.Object);

            viewModel.CalculateCommand.Execute(null);

            dialogServiceMock.Verify(ds => ds.ShowMessage(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public void FluidContactDepth_PropertyChanged_RaisesEvent()
        {
            var unitConversionServiceMock = new Mock<IUnitConversionService>();
            var dialogServiceMock = new Mock<IDialogService>();

            var viewModel = new MainViewModel(unitConversionServiceMock.Object, dialogServiceMock.Object);
            bool propertyChangedRaised = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.FluidContactDepthHandler))
                {
                    propertyChangedRaised = true;
                }
            };

            viewModel.FluidContactDepthHandler = 5000;

            Assert.IsTrue(propertyChangedRaised, "Event triggered");
        }

        public double GetRandomNumberInRange(Random random, double minNumber, double maxNumber)
        {
            return random.NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}
