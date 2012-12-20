using FubuMVC.AutoComplete;

namespace AutoCompleteHarness
{
    public class RoomLookup : SimpleLookup
    {
        public RoomLookup()
        {
            Register("OR03");
            Register("OR01");
            Register("OR02");

            Register("ER01");
            Register("ER02");
            Register("R01");
            Register("R02");
        
            Register("Living Room");
            Register("Dining Room");
            Register("Master Bedroom");
        }
    }
}