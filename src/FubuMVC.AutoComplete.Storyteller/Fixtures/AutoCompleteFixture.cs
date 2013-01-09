using OpenQA.Selenium;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.AutoComplete.Storyteller.Fixtures
{
    public class ModelFixture : Fixture
    {
        public ModelFixture()
        {
            Title = "The system state is";
        }

        private UserService _users;

        public override void SetUp(ITestContext context)
        {
            _users = Retrieve<IUserService>().As<UserService>();
        }

        public IGrammar TheUsersAre()
        {
            return CreateNewObject<User>(x =>
            {
                x.SetProperty(u => u.Id);
                x.SetProperty(u => u.Name);

                x.Do = (user) => _users.Add(user);
            }).AsTable("The users are").Before(() => _users.Clear());
        }
    }


    public class AutoCompleteFixture : ScreenFixture<AutoCompleteModel>
    {
        public AutoCompleteFixture()
        {
            Title = "In the AutoComplete Screen";
            EditableElementsForAllImmediateProperties();
        }

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new AutoCompleteModel());
        }

        [FormatAs("Click the submit button")]
        public void ClickSubmit()
        {
            Driver.FindElement(By.Id("Submit"));
        }
    }
}