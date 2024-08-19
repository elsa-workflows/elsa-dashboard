using Elsa.Api.Client.Resources.Scripting.Models;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Models;
using Elsa.Api.Client.Shared.UIHints.DropDown;
using Elsa.Studio.Models;
using Microsoft.AspNetCore.Components;

namespace Elsa.Studio.UIHints.Components;

/// <summary>
/// Provides a component for picking a variable.
/// </summary>
public partial class VariablePicker2
{
    private ICollection<SelectListItem> _items = Array.Empty<SelectListItem>();

    /// <summary>
    /// Gets or sets the editor context.
    /// </summary>
    [Parameter] public DisplayInputEditorContext EditorContext { get; set; } = default!;

    private ICollection<Variable> Variables => EditorContext.WorkflowDefinition.Variables;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        _items = Variables.Select(x => new SelectListItem(x.Name, x.Id)).OrderBy(x => x.Text).ToList();
    }

    private SelectListItem? GetSelectedValue()
    {
        var value = EditorContext.GetExpressionValueOrDefault();
        return _items.FirstOrDefault(x => x.Value == value);
    }
    
    private async Task OnValueChanged(SelectListItem? value)
    {
        //var variableId = value?.Value;
        //var variable = Variables.FirstOrDefault(x => x.Id == variableId);
        //await EditorContext.UpdateValueAsync(variable);
        var expression = Expression.CreateObject(value?.Value ?? "");
        expression.Type = "VariableInput";
        await EditorContext.UpdateExpressionAsync(expression);
    }
}