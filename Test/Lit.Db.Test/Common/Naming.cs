using System;
using Lit.Auditing;
using Lit.Names;

namespace Lit.Db.Test.Common
{
    public static class Naming
    {
        public static void Execute()
        {
            foreach (var i in itemList)
            {
                var text = Name.Format(i.Text, i.Case, i.Id, "id");
                Audit.Message($"  Translated [{i.Text}] into [{text}], with case={i.Case} and ids={i.Id}");
                if (text != i.ExpectedResult)
                {
                    throw new Exception("Wrong translation");
                }
            }
        }

        private class Item
        {
            public AffixPlacing Id;

            public Case Case;

            public string Text;

            public string ExpectedResult;
        }

        private static readonly Item[] itemList = new Item[]
        {
            // Snake case
            new Item { Id = AffixPlacing.Sufix, Case = Case.Snake, Text = "idABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new Item { Id = AffixPlacing.Sufix, Case = Case.Snake, Text = "id__ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new Item { Id = AffixPlacing.Prefix, Case = Case.Snake, Text = "ID  ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "id_abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Snake case
            new Item { Id = AffixPlacing.Prefix, Case = Case.Snake, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Kebab case
            new Item { Id = AffixPlacing.Prefix, Case = Case.Kebab, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc-def-g-hi-jkl-mno-pqr123-stu456-vw-x" },

            // Camel case
            new Item { Id = AffixPlacing.Prefix, Case = Case.Camel, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abcDefGHiJklMnoPqr123Stu456VwX" },

            // Pascal case
            new Item { Id = AffixPlacing.Prefix, Case = Case.Pascal, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "AbcDefGHiJklMnoPqr123Stu456VwX" },
        };
    }
}
