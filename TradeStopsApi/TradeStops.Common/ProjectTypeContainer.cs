using TradeStops.Common.Enums;

namespace TradeStops.Common
{
    public class ProjectTypeContainer
    {
        private readonly ProjectTypes _projectType;

        public ProjectTypeContainer(ProjectTypes projectType)
        {
            _projectType = projectType;
        }

        public bool IsTradeStops => _projectType == ProjectTypes.Tradestops;

        public bool IsCryptoStops => _projectType == ProjectTypes.CryptoStops;

        public Products Product => IsCryptoStops ? Products.CryptoTradeSmith : Products.TradeStops;

        public ProjectTypes ProjectType => _projectType;
    }
}
