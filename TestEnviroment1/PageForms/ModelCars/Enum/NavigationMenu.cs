using WebdriverFramework.Framework.Util;

namespace PageForms.Enums.NavigationMenu
    {
    public enum NavigationEnums
    {
        [EnumExtensions.StringMapping("Cars for Sale")]        
        CarsForSale = 1,

        [EnumExtensions.StringMapping("Research & Reviews")]
        ResearchReviews = 2,

        [EnumExtensions.StringMapping("News & Videos")]
        NewsVideos = 3,

        [EnumExtensions.StringMapping("Sell Your Car")]
        SellYourCar = 4,

        [EnumExtensions.StringMapping("Service & Repair")]
        ServiceRepair = 5
    }
}