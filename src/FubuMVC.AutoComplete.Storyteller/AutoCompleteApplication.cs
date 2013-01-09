using FubuMVC.AutoComplete.Serenity;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Serenity;
using Serenity.Fixtures.Handlers;

namespace FubuMVC.AutoComplete.Storyteller
{
    public class AutoCompleteApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .DefaultPolicies()
                .StructureMapObjectFactory(x =>
                    {
                        x.Scan(scanner =>
                        {
                            scanner.TheCallingAssembly();
                            scanner.WithDefaultConventions();
                        });

                        x.ForSingletonOf<IUserService>().Use<UserService>();
                    });
        }
    }

    public class AutoCompleteSystem : FubuMvcSystem<AutoCompleteApplication>
    {
        protected override void configureApplication(IApplicationUnderTest application, FubuCore.Binding.BindingRegistry binding)
        {
            ElementHandlers.Handlers.Add(new AutoCompleteElementHandler());
        }
    }
}