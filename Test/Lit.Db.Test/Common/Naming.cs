using System;

namespace Lit.Db.Test.Common
{
    public static class Naming
    {
        public static void Execute()
        {
            foreach (var i in itemList)
            {
                var text = DbNaming.Translate(i.Text, i.Case, i.Id, "id");
                Console.WriteLine($"  Translated [{i.Text}] into [{text}], with case={i.Case} and ids={i.Id}");
                if (text != i.ExpectedResult)
                {
                    throw new Exception("Wrong translation");
                }
            }
        }

        private class Item
        {
            public DbNaming.Placing Id;

            public DbNaming.Case Case;

            public string Text;

            public string ExpectedResult;
        }

        private static readonly Item[] itemList = new Item[]
        {
            // Snake case
            new Item { Id = DbNaming.Placing.Sufix, Case = DbNaming.Case.Snake, Text = "idABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new Item { Id = DbNaming.Placing.Sufix, Case = DbNaming.Case.Snake, Text = "id__ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new Item { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Snake, Text = "ID  ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "id_abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Snake case
            new Item { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Snake, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Kebab case
            new Item { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Kebab, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc-def-g-hi-jkl-mno-pqr123-stu456-vw-x" },

            // Camel case
            new Item { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Camel, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abcDefGHiJklMnoPqr123Stu456VwX" },

            // Pascal case
            new Item { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Pascal, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "AbcDefGHiJklMnoPqr123Stu456VwX" },
        };
    }
}
