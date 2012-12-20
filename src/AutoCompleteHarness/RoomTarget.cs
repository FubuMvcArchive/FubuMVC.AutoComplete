using System;
using FubuMVC.Core.Registration;
using FubuMVC.AutoComplete;

namespace AutoCompleteHarness
{
    public class HomeEndpoint
    {
        public RoomTarget Index()
        {
            return new RoomTarget();
        }
    }

    public class RoomTarget
    {
        public Guid Room { get; set; } 
    }

    public class RoomTargetOverrides : OverridesFor<RoomTarget>
    {
        public RoomTargetOverrides()
        {
            Property(x => x.Room).AutoComplete<RoomLookup>();
        }
    }
}