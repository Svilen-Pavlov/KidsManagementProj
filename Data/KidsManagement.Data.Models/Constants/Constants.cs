using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Models.Constants
{
    //toq klas trqa li mi vub6te?
    public static class Constants
    {
        public const int humanNameMinLen = 2;
        public const int humanNameMaxLen = 40;

        public const int entityNameMinLen = 2;
        public const int entityNameMaxLen = 30;
        public const int entityMinCount = 1;
        public const int entityMaxCount = 256;
        public const int textMaxLen= 256;

        public const string dateOnlyFormat = "d";
        public const string hourMinutesFormat = @"hh\:mm";
        public const string salaryFormat ="{0} BGN/mo.";

        public const double breakBetweenGroupsMinutes = 15;
        public const double groupMinDuration = 15;
        public const double groupMaxDuration = 180;

        public const string defaultProfPicURL = "http://res.cloudinary.com/svilenpavlov/image/upload/v1601641040/by5z42szy576diu5brz7.jpg";

        public const string humanNamesRegex = @"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,39}$"; //equals 2 and 40 inclusive
        public const string entityNamesRegex = @"^[\w'\-,.][^_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,29}$"; //equals 2 and 30 inclusive can be numbers https://stackoverflow.com/questions/2385701/regular-expression-for-first-and-last-name


    }
}
