using WebdriverFramework.Framework.Util;

namespace PageForms.Enums.TrimParametrs
{
    public enum TrimParametrsEnums
    {
        [EnumExtensions.StringMapping("Style")]        
        Style = 1,

        [EnumExtensions.StringMapping("Inventory price")]
        InventoryPrice = 2,

        [EnumExtensions.StringMapping("MPG")]
        Mpg = 3,

        [EnumExtensions.StringMapping("Engine")]
        Engine = 4,

        [EnumExtensions.StringMapping("Transmission")]
        Transmission = 5,

        [EnumExtensions.StringMapping("Drivetrain")]
        Drivetrain = 6,

        [EnumExtensions.StringMapping("Color")]
        Color = 7,

        [EnumExtensions.StringMapping("Seating")]
        Seating = 8,

        [EnumExtensions.StringMapping("Full specs")]
        FullSpecs = 9
    }
}