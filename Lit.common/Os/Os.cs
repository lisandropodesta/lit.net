namespace Lit.Os
{
    using Lit.Os.Fsys;

    public class Os
    {
        private static IOs os;

        public static void Register(IOs osIntance)
        {
            os = osIntance;
        }

        #region IOs

        public static IFsys Fsys { get { return os.Fsys; } }

        #endregion
    }
}
