using System;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.AutoComplete.Testing
{
    [TestFixture]
    public class SimpleLookupTester
    {
        [Test]
        public void get_the_label_by_value()
        {
            var lookup = new TestRoomLookup();
            var item1 = lookup.Items.First(x => x.label == "OR01");
            var item2 = lookup.Items.First(x => x.label == "ER02");

            lookup.LabelFor(Guid.Parse(item1.value)).ShouldEqual("OR01");
            lookup.LabelFor(Guid.Parse(item2.value)).ShouldEqual("ER02");
        }

        [Test]
        public void get_lookup_name()
        {
            new TestRoomLookup().Name.ShouldEqual("TestRoom");
        }

        [Test]
        public void lookup_by_term_1()
        {
            var lookup = new TestRoomLookup();
            lookup.Lookup(new AutoCompleteQuery { term = "OR" })
                .Select(x => x.label)
                .ShouldHaveTheSameElementsAs("OR01", "OR02", "OR03");
        }

        [Test]
        public void default_lookup_is_case_insensitive()
        {
            var lookup = new TestRoomLookup();
            lookup.Lookup(new AutoCompleteQuery { term = "or" })
                .Select(x => x.label)
                .ShouldHaveTheSameElementsAs("OR01", "OR02", "OR03");
        }

        [Test]
        public void lookup_by_term_2()
        {
            var lookup = new TestRoomLookup();
            lookup.Lookup(new AutoCompleteQuery { term = "ER" })
                .Select(x => x.label)
                .ShouldHaveTheSameElementsAs("ER01", "ER02");
        }

        [Test]
        public void can_mess_with_the_filter()
        {
            var lookup = new TestRoomLookup
            {
                ItemFilter = (item, query) => item.label.Contains(query.term)
            };

            lookup.Lookup(new AutoCompleteQuery { term = "02" })
                .Select(x => x.label)
                .ShouldHaveTheSameElementsAs("ER02", "OR02", "R02");
        }

        [Test]
        public void find_guid_by_label()
        {
            var lookup = new TestRoomLookup();
            var guid = lookup.IdFor("ER01");

            lookup.LabelFor(guid).ShouldEqual("ER01");
        }
    }

    public class TestRoomLookup : SimpleLookup
    {
        public TestRoomLookup()
        {
            Register("OR03");
            Register("OR01");
            Register("OR02");

            Register("ER01");
            Register("ER02");
            Register("R01");
            Register("R02");
        }
    }
}