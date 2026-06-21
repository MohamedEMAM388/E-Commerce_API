namespace Shared.DTOS.OrderDTOS
{
    public record OrderItemDTO
    { 

        //ProductName , PictureUrl , Price , Quantity]

        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}