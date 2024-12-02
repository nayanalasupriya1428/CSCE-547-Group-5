using System.Collections.Generic;

namespace CineBuzzApi.Models
{

    /// Represents a shopping cart for a user.

    public class Cart
    {

        /// Unique identifier for each cart.

        public int CartId { get; set; }


        /// Total price of items in the cart.

        public double Total { get; set; }


        /// Foreign key for the associated user.

        public int? UserId { get; set; }


        /// Navigation property to the associated user.

        public User? User { get; set; }

        /// Collection of items in the cart.


        public List<CartItem> Items { get; set; } = new List<CartItem>();



        /// Default constructor for Cart.

        public Cart() { }
    }





}