namespace Lit.Os.Win
{
    using Lit.Os;
    using Lit.Os.Fsys;

    public class WinOs : IOs
    {
        private Fsys.Fsys files;

        public IFsys Fsys { get { return files ?? (files = new Fsys.Fsys()); } }

        public static void Register()
        {
            Os.Register(new WinOs());
        }
    }
}
