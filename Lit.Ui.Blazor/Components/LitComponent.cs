using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Lit.Ui.Blazor
{
    public abstract class LitComponent : ComponentBase, IDisposable
    {
        [Inject]
        IJSRuntime jsRuntime { get; set; }

        /// <summary>
        /// Closes the game.
        /// </summary>
        void IDisposable.Dispose()
        {
            DoCloseAll();
        }

        protected virtual void DoCloseAll()
        {
        }
    }
}
