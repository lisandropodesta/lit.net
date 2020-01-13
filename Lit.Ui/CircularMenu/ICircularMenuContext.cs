namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu context definition.
    /// </summary>
    public interface ICircularMenuContext<T> where T : CircularMenuItem
    {
        CircularMenu<T> ContextMenu { get; }
    }
}
