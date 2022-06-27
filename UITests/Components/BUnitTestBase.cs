using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using FateExplorer.UITests.Mocks;

namespace FateExplorer.UITests.Components;

public abstract class BUnitTestBase
{
    protected Bunit.TestContext Ctx { get; private set; }

    [SetUp]
    public virtual void Setup() // code borrowed from MudBlazor
    {
        Ctx = new();

        Ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        Ctx.Services.AddSingleton<NavigationManager>(new MockNavigationManager());
        Ctx.Services.AddMudServices(options =>
        {
            options.SnackbarConfiguration.ShowTransitionDuration = 0;
            options.SnackbarConfiguration.HideTransitionDuration = 0;
        });
        Ctx.Services.AddScoped(sp => new HttpClient());
        Ctx.Services.AddOptions();
    }

    protected virtual void SetupMudDialog(out IRenderedComponent<MudDialogProvider> comp, out DialogService dlgService)
    {
        comp = Ctx.RenderComponent<MudDialogProvider>();
        Assume.That(comp.Markup, Is.Empty); // verify success
        dlgService = (Ctx.Services.GetService<IDialogService>() as DialogService)!; // null-forgiving: next line checks for null
        Assume.That(dlgService, Is.Not.Null); // verify success
    }


    [TearDown]
    public void TearDown()
    {
        try
        {
            Ctx.Dispose();
        }
        catch (Exception)
        {
            /*ignore*/
        }
    }
}
