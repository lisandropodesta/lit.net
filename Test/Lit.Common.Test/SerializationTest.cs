using System;
using System.Text;
using Lit.DataType;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Lit.Common.Test
{
    public class SerializationTest
    {
        private const int Repetitions = 1000;

        private static readonly string bigString = BuildBigString();

        [SetUp]
        public void Setups()
        {
            Serialization.Setup();
        }

        [Test]
        public void StringValidationTest()
        {
            string text, s1, s2;
            for (var i = 0; i < bigString.Length; i++)
            {
                text = bigString[i].ToString();
                s1 = JsonConvert.SerializeObject(text);
                s2 = Serialization.Encode(text);
                Assert.AreEqual(s1, s2);
            }

            text = bigString;
            s1 = JsonConvert.SerializeObject(text);
            s2 = Serialization.Encode(text);
            Assert.AreEqual(s1, s2);

            Assert.Pass();
        }

        [Test]
        public void StringEncodeTest()
        {
            for (var r = 0; r < 500; r++)
            {
                Serialization.Encode(bigString);
            }

            Assert.Pass();
        }

        [Test]
        public void StringEncodeNewtonTest()
        {
            for (var r = 0; r < 500; r++)
            {
                JsonConvert.SerializeObject(bigString);
            }

            Assert.Pass();
        }

        [Test]
        public void StringPerformanceTest()
        {
            var text = bigString;
            for (var r = 0; r < Repetitions; r++)
            {
                var encodedText = Serialization.Encode(text);
                var decodedText = Serialization.DecodeString(encodedText);
                Assert.AreEqual(text, decodedText);
            }

            Assert.Pass();
        }

        [Test]
        public void StringPerformanceNewtonTest()
        {
            var text = bigString;
            for (var r = 0; r < Repetitions; r++)
            {
                var encodedText = JsonConvert.SerializeObject(text);
                var decodedText = JsonConvert.DeserializeObject<string>(encodedText);
                Assert.AreEqual(text, decodedText);
            }

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

        /// <summary>
        /// Builds a big string with most of valid characters.
        /// </summary>
        private static string BuildBigString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 0xD800; i++)
            {
                sb.Append(char.ConvertFromUtf32(i));
            }

            return sb.ToString();
        }
    }
}
