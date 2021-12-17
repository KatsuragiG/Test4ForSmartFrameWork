using System;

namespace PageForms.ModelCars.Models.CarsParametrsModel
{
    public class CarsParametersModel : IEquatable<CarsParametersModel>
    {
        public string Style { get; set; }
        public string InventoryPrice { get; set; }
        public string Mpg { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string Drivetrain { get; set; }
        public string Color { get; set; }
        public string Seating { get; set; }
        public string FullSpecs { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CarsParametersModel);
        }

        public bool Equals(CarsParametersModel other)
        {
            return Equals(other.Style, Style)
                   && Equals(other.InventoryPrice, InventoryPrice)
                   && Equals(other.Mpg, Mpg)
                   && Equals(other.Engine, Engine)
                   && Equals(other.Transmission, Transmission)
                   && Equals(other.Drivetrain, Drivetrain)
                   && Equals(other.Color, Color)
                   && Equals(other.Seating, Seating)
                   && Equals(other.FullSpecs, FullSpecs);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }


}