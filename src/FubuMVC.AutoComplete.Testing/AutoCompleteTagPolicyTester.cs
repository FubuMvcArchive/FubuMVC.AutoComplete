using System.Collections.Generic;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Elements;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuMVC.AutoComplete.Testing
{
    [TestFixture]
    public class AutoCompleteTagPolicyTester
    {
        private AutoCompleteTagPolicy thePolicy;

        [SetUp]
        public void SetUp()
        {
            thePolicy = new AutoCompleteTagPolicy
            {
                Rules = new AccessorRules()
            };
        }

        [Test]
        public void matches_false_with_no_rules()
        {
            var request = ElementRequest.For<AutoCompleteTarget>(x => x.Name);
            thePolicy.Matches(request).ShouldBeFalse();
        }

        [Test]
        public void matches_true_with_lookup_rule()
        {
            thePolicy.Rules.Add<AutoCompleteTarget>(x => x.Name, new LookupMarker(typeof(FakeLookup)));

            var request = ElementRequest.For<AutoCompleteTarget>(x => x.Name);
            thePolicy.Matches(request).ShouldBeTrue();
        }

        [Test]
        public void matches_true_with_attribute()
        {
            var request = ElementRequest.For<AutoCompleteTarget>(x => x.State);
            thePolicy.Matches(request).ShouldBeTrue();
        }

        [Test]
        public void build_for_an_attribute()
        {
            var request = ElementRequest.For<AutoCompleteTarget>(x => x.State);
            thePolicy.BuilderFor(request).ShouldEqual(new AutoCompleteTagBuilder(typeof (SecondLookup)));
        }

        [Test]
        public void build_for_registered_rule()
        {
            thePolicy.Rules.Add<AutoCompleteTarget>(x => x.Name, new LookupMarker(typeof(FakeLookup)));

            var request = ElementRequest.For<AutoCompleteTarget>(x => x.Name);
            thePolicy.BuilderFor(request).ShouldEqual(new AutoCompleteTagBuilder(typeof(FakeLookup)));
        }
    }

    public class AutoCompleteTarget
    {
        public string Name { get; set; }

        [AutoComplete(typeof(SecondLookup))]
        public string State { get; set; }
    }

    public class FakeLookup : ILookupProvider
    {
        public object IdFor(string label)
        {
            throw new System.NotImplementedException();
        }

        public string LabelFor(object value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<LookupItem> Lookup(AutoCompleteQuery query)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SecondLookup : ILookupProvider
    {
        public object IdFor(string label)
        {
            throw new System.NotImplementedException();
        }

        public string LabelFor(object value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<LookupItem> Lookup(AutoCompleteQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}
