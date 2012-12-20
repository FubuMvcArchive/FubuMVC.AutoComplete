using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.UI;

namespace FubuMVC.AutoComplete
{
    public class AutoCompleteEndpoints : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            var tagPolicy = new AutoCompleteTagPolicy();
            

            registry.Actions.FindWith(new LookupActionSource());
            registry.Routes.UrlPolicy<LookupUrlPolicy>();

            registry.Import<HtmlConventionRegistry>(x => x.Editors.Add(tagPolicy));

            registry.Configure(graph => {
                var rules = graph.Settings.Get<AccessorRules>();

//                var strategy = new AutoCompleteStringifierStrategy(rules);
//                var stringifier = graph.Services.DefaultServiceFor<Stringifier>().Value.As<Stringifier>();
//                stringifier.AddStrategy(strategy);

                tagPolicy.Rules = rules;
            });
        }
    }
}