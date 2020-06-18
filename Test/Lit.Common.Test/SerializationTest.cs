using System;
using System.Text;
using Lit.DataType;
using NUnit.Framework;

namespace Lit.Common.Test
{
    public class SerializationTest
    {
        private const int Repetitions = 1000;

        [Test]
        public void StringTest()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 0xD800; i++)
            {
                sb.Append(char.ConvertFromUtf32(i));
            }

            var text = sb.ToString();

            var encodedText = Serialization.EncodeString(text, SerializationMode.Compact);
            var decodedText = Serialization.DecodeString(encodedText, SerializationMode.Compact);
            Assert.AreEqual(text, decodedText);

            Assert.Pass();
        }

        [Test]
        public void ArrayTest()
        {
            for (var r = 0; r < Repetitions; r++)
            {
                var arr1 = new int[5] { 6, 7, 8, 9, 10 };
                var encoded1 = Serialization.Encode(arr1);
                Assert.AreEqual("[6,7,8,9,10]", encoded1);

                var arr2 = new int[2, 5] { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 } };
                var encoded2 = Serialization.Encode(arr2);
                Assert.AreEqual("[[1,2,3,4,5],[6,7,8,9,10]]", encoded2);

                var setter2 = TypeHelper.GetArraySetter(arr2);
                setter2(arr2, new[] { 1, 2 }, 0);
                encoded2 = Serialization.Encode(arr2);
                Assert.AreEqual("[[1,2,3,4,5],[6,7,0,9,10]]", encoded2);

                var arr3 = new int[2, 6] { { 6, 7, 8, 9, 10, 11 }, { 1, 2, 3, 4, 5, 6 } };
                var encoded3 = Serialization.Encode(arr3);
                Assert.AreEqual("[[6,7,8,9,10,11],[1,2,3,4,5,6]]", encoded3);
            }

            Assert.Pass();
        }

        [Test]
        public void EnumTest()
        {
            for (var r = 0; r < Repetitions; r++)
            {
                foreach (var value in Enum.GetValues(typeof(MyEnum)))
                {
                    var encodedText = Serialization.Encode(value);
                    var decodedValue = Serialization.DecodeEnum(encodedText, typeof(MyEnum));
                    Assert.AreEqual(value, decodedValue);
                }
            }

            Assert.Pass();
        }

        public enum MyEnum
        {
            Option1,

            [Serialization("custom-option-2")]
            Option2,

            Option3
        }
    }
}
