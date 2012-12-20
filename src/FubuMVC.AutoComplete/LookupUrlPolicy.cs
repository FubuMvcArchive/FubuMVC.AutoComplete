using FubuCore;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace FubuMVC.AutoComplete
{
    public class LookupUrlPolicy : IUrlPolicy
    {
        public bool Matches(ActionCall call)
        {
            return call.HandlerType.CanBeCastTo<ILookupProvider>();
        }

        public IRouteDefinition Build(ActionCall call)
        {
            var pattern = "_lookup/" + call.HandlerType.Name.Replace("LookupProvider", "").ToLower();
            return new RouteDefinition(pattern);
        }
    }
}