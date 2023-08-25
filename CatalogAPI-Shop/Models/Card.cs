namespace Card.Models

{
    public class Card
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public int Level { get; set; }
        public string? Category { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public string? Rarity { get; set; }
        public Double price { get; set; }

    }

}