﻿using Microsoft.AspNetCore.Components;

namespace UITests.UnitTests.Mocks;

/// <summary>
/// 
/// </summary>
/// <remarks>"Borrowed" from MudBlazor</remarks>
public class MockNavigationManager
    : NavigationManager
{
    public MockNavigationManager() : base() =>
        this.Initialize("http://localhost:2112/", "http://localhost:2112/test");

    protected override void NavigateToCore(string uri, bool forceLoad) =>
        this.WasNavigateInvoked = true;

    public bool WasNavigateInvoked { get; private set; }
}