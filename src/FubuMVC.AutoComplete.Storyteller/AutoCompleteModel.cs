using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.AutoComplete.Storyteller
{
    public class AutoCompleteModel
    {
        public User User { get; set; }
    }

    public class AutoCompleteModelOverrides : OverridesFor<AutoCompleteModel>
    {
        public AutoCompleteModelOverrides()
        {
            Property(x => x.User).AutoComplete<UserLookup>();
        }
    }

    public class User
    {
        public User() {}
        public User(string id)
        {
            Id = int.Parse(id);
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IUserService
    {
        IEnumerable<User> All();
    }

    public class UserService : IUserService
    {
        private readonly IList<User> _users = new List<User>();
 
        public void Add(User user)
        {
            _users.Fill(user);
        }

        public void Clear()
        {
            _users.Clear();
        }

        public IEnumerable<User> All()
        {
            return _users;
        }
    }

    public class UserLookup : ILookupProvider
    {
        private readonly IUserService _users;

        public UserLookup(IUserService users)
        {
            _users = users;
        }

        public object IdFor(string label)
        {
            return _users.All().Single(x => x.Name == label).Id;
        }

        public string LabelFor(object value)
        {
            return _users.All().Single(x => x.Id.Equals(value)).Name;
        }

        public IEnumerable<LookupItem> Lookup(AutoCompleteQuery query)
        {
            return _users
                .All()
                .Where(x => x.Name.ToLower().Contains(query.term.ToLower()))
                .Select(x => new LookupItem {label = x.Name, value = x.Id.ToString()});
        }
    }

    public class UserEndpoint
    {
        private readonly FubuHtmlDocument<AutoCompleteModel> _page;

        public UserEndpoint(FubuHtmlDocument<AutoCompleteModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<AutoCompleteModel> get_users_create(AutoCompleteModel model)
        {
            var form = _page.FormFor<AutoCompleteModel>();
            form.Append(_page.Edit(x => x.User));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Submit"));
            form.Append(_page.WriteScriptTags());
            _page.Add(form);
            
            return _page;
        }

        public string post_users_create(AutoCompleteModel input)
        {
            return "Success";
        }
    }
}