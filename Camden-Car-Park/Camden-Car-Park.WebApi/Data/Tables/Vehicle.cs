namespace Camden_Car_Park.WebApi.Data.Tables
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        public required string RegistrationNumber { get; set; }

        public required string Make { get; set; }

        public required string Model { get; set; }

        public required string Colour { get; set; }

        public required string Year { get; set; }
    }
}
