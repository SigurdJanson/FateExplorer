using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using UITests.UnitTests.Mocks;

namespace UITests.Components
{
    public abstract class BUnitTestBase
    {
        protected Bunit.TestContext Ctx { get; private set; }

        [SetUp]
        public virtual void Setup()
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
}
