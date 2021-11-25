using System;
using System.Collections.Generic;

namespace TradeStops.Common.Constants
{
    public static class PredefinedPortfoliosConstants
    {
        public const int NumberOfTrendingTickersPositions = 10;
        public const int NumberOfPureQuantPositions = 20;
        public const decimal PureQuantPortfolioDefaultInvestmentAmount = 10000;
        public const decimal RecoveryPortfolioDefaultInvestmentAmount = 100000;       
        public static int[] RecoveryPortfolioSymbolIds => new[]
        {
            9651, 19255, 7375357, 20214, 13269, 4635202, 19055, 16038, 6622330, 7370959
        };
    }
}
