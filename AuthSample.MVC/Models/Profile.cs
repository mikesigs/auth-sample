using System.Collections.Generic;
using System.Web.Mvc;

namespace AuthSample.MVC.Models
{
    public class Profile
    {
        private readonly IEnumerable<SelectListItem> _awesomenessOptions;

        public Profile(string userName, string firstName, string lastName, int awesomeness)
            : this()
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Awesomeness = awesomeness;
        }

        public Profile()
        {
            _awesomenessOptions = new[]
            {
                new SelectListItem {Text = "Not Awesome", Value = "1"},
                new SelectListItem {Text = "Somewhat Awesome", Value = "2"},
                new SelectListItem {Text = "Really Awesome", Value = "3"},
                new SelectListItem {Text = "Totally Awesome", Value = "4"},
                new SelectListItem {Text = "Completely and Totally Awesome", Value = "5"}
            };
        }
        
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Awesomeness { get; set; }

        public IEnumerable<SelectListItem> AwesomenessOptions
        {
            get { return _awesomenessOptions; }
        }
    }
}