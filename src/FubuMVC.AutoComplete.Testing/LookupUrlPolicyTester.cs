using FubuMVC.Core.Registration.Nodes;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuMVC.AutoComplete.Testing
{
    [TestFixture]
    public class LookupUrlPolicyTester
    {
        [Test]
        public void find_the_route_pattern()
        {
            var call = ActionCall.For<FakeLookup>(x => x.Lookup(null));
            var route = new LookupUrlPolicy().Build(call);

            route.Pattern.ShouldEqual("_lookup/fakelookup");
        }

        [Test]
        public void matches_negative()
        {
            var call = ActionCall.For<LookupUrlPolicyTester>(x => x.matches_negative());
            new LookupUrlPolicy().Matches(call).ShouldBeFalse();
        }

        [Test]
        public void matches_positive()
        {
            var call = ActionCall.For<FakeLookup>(x => x.Lookup(null));
            new LookupUrlPolicy().Matches(call).ShouldBeTrue();
        }
    }
}