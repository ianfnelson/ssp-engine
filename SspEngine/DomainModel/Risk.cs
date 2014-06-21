namespace SspEngine.DomainModel
{
    public class Risk
    {
        public string Name { get; set; }

        public string Occupation { get; set; }

        public Address Address { get; set; }

        public Postcode KeptPostcode { get; set; }
    }
}
