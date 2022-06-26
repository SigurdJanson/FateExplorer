using Bunit;
using FateExplorer.Components;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using FateExplorer;
using MudBlazor;
using UITests.Components;
using AngleSharp.Dom;

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
        //mockL10n.SetupGet(c => c[It.IsAny<string>()]).Returns(new LocalizedString("", "String"));
        mockL10n.Setup(_ => _[It.IsAny<string>()]).Returns((string s) => new LocalizedString(s, s));
    }


    [Test]
    public async Task RenderEnergiesCorrectly([Values(1, 2, 3)] int TestCount)
    {
        var EnergyNames = new string[] { "LP", "AE", "KE" };

        // Arrange
        SetupL10N();
        SetupMudDialog(out IRenderedComponent<MudDialogProvider> comp, out DialogService dlgService);

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




    [Test]
    public async Task Regenerate_Submit_CorrectOutput()
    {
        const int TestCount = 2;
        var EnergyNames = new string[] { "LP", "AE", "KE" };

        // Arrange
        SetupL10N();
        SetupMudDialog(out IRenderedComponent<MudDialogProvider> comp, out DialogService dlgService);

        // Open the dialog
        var parameters = new DialogParameters
        {
            { "Names", EnergyNames[0..(TestCount-1)] }
        };
        IDialogReference? dlgReference = null;
        await comp.InvokeAsync(() => dlgReference = dlgService!.Show<RegenerationDialog>("", parameters));
        Assume.That(dlgReference, Is.Not.Null); // verify success

        // Act
        IElement[] inputs;
        var RdbSite = comp.FindComponent<MudRadioGroup<RegenerationSite>>();
        inputs = RdbSite.FindAll("input").ToArray();
        inputs[0].Click();

        var RdbDisturbed = comp.FindComponent<MudRadioGroup<RegenerationDisturbance>>();
        inputs = RdbDisturbed.FindAll("input").ToArray();
        inputs[1].Click();

        var ChbSick = comp.FindComponent<MudCheckBox<bool>>();
        inputs = ChbSick.FindAll("input").ToArray();
        inputs[0].Change(true);

        // verify
        Assume.That(RdbSite.Instance.SelectedOption, Is.EqualTo(RegenerationSite.Good));
        Assume.That(RdbDisturbed.Instance.SelectedOption, Is.EqualTo(RegenerationDisturbance.Brief));
        Assume.That(ChbSick.Instance.Checked, Is.True);

        // ASSERT ===
        comp.Find("button[type=submit]").Click();
        var result = await dlgReference!.Result;
        Assert.That(result.Cancelled, Is.False);

        var DlgResult = ((RegenerationSite, RegenerationDisturbance, bool, int[]))result.Data;
        Assert.Multiple(() =>
        {
            Assert.That(DlgResult.Item1, Is.EqualTo(RegenerationSite.Good));
            Assert.That(DlgResult.Item2, Is.EqualTo(RegenerationDisturbance.Brief));
            Assert.That(DlgResult.Item3, Is.True);
        });
    }

    [Test]
    public async Task Regenerate_ClickReset_SettingsBackToDefault()
    {
        const int TestCount = 2;
        var EnergyNames = new string[] { "LP", "AE", "KE" };

        // ARRANGE ====
        SetupL10N();
        SetupMudDialog(out IRenderedComponent<MudDialogProvider> comp, out DialogService dlgService);

        // Open the dialog
        var parameters = new DialogParameters
        {
            { "Names", EnergyNames[0..(TestCount-1)] }
        };
        IDialogReference? dlgReference = null;
        await comp.InvokeAsync(() => dlgReference = dlgService!.Show<RegenerationDialog>("", parameters));
        Assume.That(dlgReference, Is.Not.Null); // verify success

        // Set some values in the dialog
        IElement[] inputs;
        var RdbSite = comp.FindComponent<MudRadioGroup<RegenerationSite>>();
        inputs = RdbSite.FindAll("input").ToArray();
        inputs[0].Click();

        var RdbDisturbed = comp.FindComponent<MudRadioGroup<RegenerationDisturbance>>();
        inputs = RdbDisturbed.FindAll("input").ToArray();
        inputs[1].Click();

        var ChbSick = comp.FindComponent<MudCheckBox<bool>>();
        inputs = ChbSick.FindAll("input").ToArray();
        inputs[0].Change(true);

        // verify
        Assume.That(RdbSite.Instance.SelectedOption, Is.EqualTo(RegenerationSite.Good));
        Assume.That(RdbDisturbed.Instance.SelectedOption, Is.EqualTo(RegenerationDisturbance.Brief));
        Assume.That(ChbSick.Instance.Checked, Is.True);

        // ACT ===
        comp.Find("button[type=button]").Click(); // the first of the 2 buttons (!= submit) should be it

        // ASSERT ===
        comp.Find("button[type=submit]").Click();
        var result = await dlgReference!.Result;
        Assert.That(result.Cancelled, Is.False);

        var DlgResult = ((RegenerationSite, RegenerationDisturbance, bool, int[]))result.Data;
        Assert.Multiple(() =>
        {
            Assert.That(DlgResult.Item1, Is.EqualTo(RegenerationSite.Default));
            Assert.That(DlgResult.Item2, Is.EqualTo(RegenerationDisturbance.None));
            Assert.That(DlgResult.Item3, Is.False);
        });
    }
}