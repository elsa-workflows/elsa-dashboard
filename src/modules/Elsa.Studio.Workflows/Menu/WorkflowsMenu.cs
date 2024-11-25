using Elsa.Studio.Contracts;
using Elsa.Studio.Localization;
using Elsa.Studio.Models;
using Elsa.Studio.Workflows.Services;
using MudBlazor;

namespace Elsa.Studio.Workflows.Menu;

public class WorkflowsMenu : IMenuProvider
{
    private readonly DefaultLocalizer _localizer;
    public WorkflowsMenu(DefaultLocalizer localizer)
    {
        _localizer = localizer;
    }
    public ValueTask<IEnumerable<MenuItem>> GetMenuItemsAsync(CancellationToken cancellationToken = default)
    {
        var menuItems = new List<MenuItem>
        {
            new()
            {
                Icon = Icons.Material.Outlined.Schema,
                Text = _localizer["Workflows"],
                GroupName = MenuItemGroups.General.Name,
                SubMenuItems =
                {
                    new MenuItem()
                    {
                        Text = _localizer["Definitions"],
                        Href = "workflows/definitions"
                    },
                    new MenuItem()
                    {
                        Text = _localizer["Instances"],
                        Href = "workflows/instances"
                    },
                }
            }
        };

        return new ValueTask<IEnumerable<MenuItem>>(menuItems);
    }
}