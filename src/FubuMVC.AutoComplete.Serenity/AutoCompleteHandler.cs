using System.Collections.Generic;
using System.Linq;
using FubuCore;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures.Handlers;
using StoryTeller.Assertions;

namespace FubuMVC.AutoComplete.Serenity
{
    public class AutoCompleteElementHandler : IElementHandler
    {
        private ISearchContext _searchContext;
        private static readonly By MenuItemSelector = By.CssSelector("li.ui-menu-item > a");

        public bool Matches(IWebElement element)
        {
            return element.TagName == "input" && element.GetAttribute("type") == "hidden" &&
                   element.GetAttribute("class").Contains("lookup");
        }

        public void EnterData(ISearchContext context, IWebElement element, object data)
        {
            _searchContext = context;

            var textbox = FindTextbox(context, element);

            var text = (string)data;

            new TextboxElementHandler().EnterData(context, textbox, text);

            Wait.Until(() => context.FindElements(MenuItemSelector).Any());

            var links = context.FindElements(MenuItemSelector);
            var link = links.FirstOrDefault(x => x.Text == text);
            StoryTellerAssert.Fail(link == null, () =>
            {
                string availableItems = links.Select(x => x.Text).Join(", ");
                return "Unable to find drop down item with text {0}, available items were {1}"
                    .ToFormat(text, availableItems);
            });

            Retry.Twice(() => link.Click());
        }

        private static void clickItem(string text, IEnumerable<IWebElement> items)
        {
            foreach (var item in items)
            {
                var link = item.FindElement(By.TagName("a"));
                if (link != null && link.Text == text)
                {
                    link.Click();


                    return;
                }
            }
        }

        public string GetData(ISearchContext context, IWebElement element)
        {
            // TODO:   discuss how to fix in a more elegant manner.  ISearchContext reference is null here 
            // so we use the one we hijacked from EnterData
            IWebElement textbox = FindTextbox(_searchContext, element);

            return new TextboxElementHandler().GetData(_searchContext, textbox);
        }

        public static IWebElement FindTextbox(ISearchContext context, IWebElement element)
        {
            var id = element.GetAttribute("id");
            var labelId = id + "Value";

            return context.FindElement(By.Id(labelId));
        }

        public bool Equals(AutoCompleteElementHandler other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(AutoCompleteElementHandler)) return false;
            return Equals((AutoCompleteElementHandler)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}