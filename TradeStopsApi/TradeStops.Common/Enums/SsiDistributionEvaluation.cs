namespace TradeStops.Common.Enums
{
    public enum SsiDistributionEvaluation
    {
        Undefined = 0,
        SinglePositionInRed = 1,
        SinglePositionInYellow = 2,
        SinglePositionInGreen = 3,
        MultiplePositionsAllRed = 4,
        MultiplePositionsAllGreen = 5,
        TooMuchRed = 6,
        AllHealthyAndMostlyGreen = 7,
        GreenAndYellowMix = 8,
        SomeRedButGenerallyOkay = 9,
    }
}