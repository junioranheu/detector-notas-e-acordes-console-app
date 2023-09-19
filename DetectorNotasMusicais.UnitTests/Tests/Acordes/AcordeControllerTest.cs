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
            yield return new object[] { new List<string> { "Apple", "Banana", "Cherry" }, "xxx", false };
            yield return new object[] { new List<string> { "Dog", "Cat", "Fish" }, "xxx", false };
            yield return new object[] { new List<string> { "Dog", "Cat", "Fish" }, "xxx", false };
            yield return new object[] { new List<string> { "Dog", "Cat", "Fish" }, "xxx", false };
        }
        #endregion

        [Theory]
        [MemberData(nameof(TestData_Validar_MapearAcorde_Sucesso))]
        public async Task Validar_MapearAcorde_Sucesso(List<string> listaNotas, string acorde, bool esperado)
        {
            // Arrange;

            // Act;
            var (isErro, strRetorno) = AcordeController.MapearAcorde(listaNotas);

            // Assert;
            _output.WriteLine(strRetorno);
            Assert.Equal(isErro, esperado);
        }

        //[Fact]
        //public async Task Listar_ChecarResultadoEsperado()
        //{
        //    // Arrange;
        //    var paginacao = new Mock<PaginacaoInput>();

        //    List<WardInput> listaInput = WardMock.CriarListaInput();
        //    await _context.Wards.AddRangeAsync(_map.Map<List<Ward>>(listaInput));
        //    await _context.SaveChangesAsync();

        //    var query = new ListarWardQuery(_context);

        //    // Act;
        //    var resp = await query.Execute(paginacao.Object, string.Empty);

        //    // Assert;
        //    Assert.True(resp.Any());
        //}
    }
}