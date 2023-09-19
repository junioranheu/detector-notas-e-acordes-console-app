using DetectorNotasMusicais.App.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace DetectorNotasMusicais.UnitTests.Tests.Acordes
{
    public sealed class AcordeControllerTest
    {
        private readonly ITestOutputHelper _output;

        public AcordeControllerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        #region test_data
        public static IEnumerable<object[]> TestData_Validar_MapearAcorde_Sucesso()
        {
            yield return new object[] { new List<string> { "C", "E", "G" }, "C" };
            yield return new object[] { new List<string> { "C", "D#", "G" }, "Cm" };
            yield return new object[] { new List<string> { "B", "D#", "F#" }, "B" };
            yield return new object[] { new List<string> { "B", "D", "F#" }, "Bm" };
        }

        public static IEnumerable<object[]> TestData_Validar_MapearAcorde_Falha()
        {
            yield return new object[] { new List<string> { "C", "E", "B" } };
            yield return new object[] { new List<string> { "C", "E" } };
            yield return new object[] { new List<string> { "C" } };
            yield return new object[] { new List<string> { } };
            yield return new object[] { new List<string> { "X", "Y", "Z" } };
        }
        #endregion

        [Theory]
        [MemberData(nameof(TestData_Validar_MapearAcorde_Sucesso))]
        public void Validar_MapearAcorde_Sucesso(List<string> listaNotas, string acorde)
        {
            // Arrange;

            // Act;
            var (isErro, strRetorno) = AcordeController.MapearAcorde(listaNotas);

            // Assert;
            _output.WriteLine(strRetorno);
            Assert.False(isErro);
            Assert.True(strRetorno == acorde);
        }

        [Theory]
        [MemberData(nameof(TestData_Validar_MapearAcorde_Falha))]
        public void Validar_MapearAcorde_Falha(List<string> listaNotas)
        {
            // Arrange;

            // Act;
            var (isErro, strRetorno) = AcordeController.MapearAcorde(listaNotas);

            // Assert;
            _output.WriteLine(strRetorno);
            Assert.True(isErro);
        }
    }
}