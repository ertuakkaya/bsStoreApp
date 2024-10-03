namespace Entities.Exceptions
{
    public class PriceOutOfRangeBadRequestExceiton : BadRequestException
    {
        public PriceOutOfRangeBadRequestExceiton() : base("Maximum price should be less than 1000 and greater than 10!")
        {

        }
    }


}
