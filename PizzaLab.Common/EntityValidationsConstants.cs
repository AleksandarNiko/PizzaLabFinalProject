namespace PizzaLab.Common
{
    public static class EntityValidationsConstants
    {
        public static class Pizza
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;

            public const int ImageMinLength = 5;
            public const int ImageMaxLength = 512;

            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 1024;

        }

        public static class Product
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public static class Dough
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public static class Topping
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public static class Menu
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;

            public const int DescriptionMinLength = 3;
            public const int DescriptionMaxLength = 255;
        }

        public static class Admin
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }
    }
}
