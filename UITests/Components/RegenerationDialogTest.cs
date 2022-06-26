using Bunit;
using FateExplorer.Components;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using FateExplorer;
using MudBlazor;
using UITests.Components;

namespace Bunit.Docs.Samples;

public class RegenerationDialogTest : BUnitTestBase
{
    private MockRepository mockRepository;
    private Mock<IStringLocalizer<App>> mockL10n;


    [SetUp]
    public override void Setup()
    {
        base.Setup();

        mockRepository = new MockRepository(MockBehavior.Strict);
        mockL10n = mockRepository.Create<IStringLocalizer<App>>();
    }

    private void SetupL10N()
    {
        Ctx.Services.AddSingleton(mockL10n.Object);
        mockL10n.SetupGet(c => c[It.IsAny<string>()]).Returns(new LocalizedString("", "String"));
    }


    [Test]
    public async Task RenderEnergiesCorrectly([Values(1, 2, 3)] int TestCount)
    {
        var EnergyNames = new string[] { "LP", "AE", "KE" };

        // Arrange
        SetupL10N();

        var comp = Ctx.RenderComponent<MudDialogProvider>();
        Assume.That(comp.Markup, Is.Empty); // verify success
        var dlgService = Ctx.Services.GetService<IDialogService>() as DialogService;
        Assume.That(dlgService, Is.Not.Null); // verify success

        // Act: open the dialog
        var parameters = new DialogParameters
        {
            { "Names", EnergyNames[0..TestCount] }
        };
        IDialogReference? dlgReference = null;
        await comp.InvokeAsync(() => dlgReference = dlgService!.Show<RegenerationDialog>("", parameters));
        Assume.That(dlgReference, Is.Not.Null); // verify success

        // ASSERT ===
        // Verify that dialog has been rendered correctly
        Assert.That(comp.Find("div.mud-dialog-container"), Is.Not.Null);
        // Verify the number of energies (each is represented ba an avatar)
        var RenderedEnergies = comp.FindComponents<MudAvatar>();
        Assert.That(RenderedEnergies, Has.Count.EqualTo(TestCount));
        for (int i = 0; i < TestCount; i++)
            Assert.That(RenderedEnergies[i].Find("div.mud-avatar").TextContent, Is.EqualTo(EnergyNames[i]));
        // Verify calls to mocks
        mockRepository.VerifyAll();
    }
}