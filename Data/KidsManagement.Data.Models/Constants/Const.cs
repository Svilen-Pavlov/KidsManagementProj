using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Models.Constants
{
    //toq klas trqa li mi vub6te?
    public static class Const
    {
        public const int humanNameMinLen = 2;
        public const int humanNameMaxLen = 40;

        public const int entityNameMaxLen = 30;
        public const int entityMinCount = 1;
        public const int entityMaxCount = 256;
        public const int textMaxLen= 256;

        public const string dateOnlyFormat = "d";
        public const string hourMinutesFormat = @"h\:mm";

        public const string defProfPicURL = "http://res.cloudinary.com/svilenpavlov/image/upload/v1601641040/by5z42szy576diu5brz7.jpg";
    }
}
