using System;
using System.Linq;
using NUnit.Framework;
using Lit.DataType;

namespace Lit.Common.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static TypeBindingCache<MyAttribute> bindingCache;

        [Test]
        public void Test1()
        {
            bindingCache = new TypeBindingCache<MyAttribute>();

            var instance = new MyTestClass
            {
                StringValue = "hi!",
                IntegerValue = 123,
                DoubleValue = 1.23
            };

            var text = Serialize(instance);
            Assert.AreEqual("{IntegerValue=123,StringValue=hi!}", text);

            Assert.Pass();
        }

        private string Serialize<T>(T instance)
        {
            var props = bindingCache.Get(typeof(T));
            var enumProps = props.BindingList.Select(p => $"{p.PropertyInfo.Name}={p.PropertyInfo.GetValue(instance)}");
            return $"{{{string.Join(",", enumProps)}}}";
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class MyAttribute : Attribute
    {
        public MyAttribute()
        {
        }
    }

    public class MyTestBaseClass
    {
        [MyAttribute]
        public string StringValue { get; set; }
    }

    public class MyTestClass : MyTestBaseClass
    {
        [MyAttribute]
        public int IntegerValue { get; set; }

        public double DoubleValue { get; set; }
    }
}