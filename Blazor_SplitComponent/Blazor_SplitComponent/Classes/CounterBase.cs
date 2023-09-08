using Microsoft.AspNetCore.Components;

namespace Blazor_SplitComponent.Classes;

public class CounterBase : ComponentBase
{
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
