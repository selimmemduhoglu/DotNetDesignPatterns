﻿namespace MembershipSystem.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        public string Build()
        {
            if (AppUser is null)
                throw new Exception("user not found");

            StringBuilder stringBuilder = new();
            stringBuilder.Append("<div class='card'>");

            stringBuilder.Append(SetPicture());

            stringBuilder.Append($@"<div class='card-body'>
                                    <h5>{AppUser.UserName}</h5>
                                    <p>{AppUser.Description}</p>");

            stringBuilder.Append(SetFooter());

            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();

        }

        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
