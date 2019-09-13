namespace Lit.Os
{
    using Lit.Os.Fsys;

    public interface IOs
    {
        IFsys Fsys { get; }
    }
}
