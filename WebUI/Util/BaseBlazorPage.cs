using Microsoft.AspNetCore.Components;
using Serilog;

namespace Hides.WebUI.Util;

public class BaseBlazorPage:ComponentBase
{
    public static void HandleException(Exception ex, NavigationManager navigation)
    {
        Log.Error(ex, "NotLoggedInException");
            navigation.NavigateTo("/");
            return;
    }
}