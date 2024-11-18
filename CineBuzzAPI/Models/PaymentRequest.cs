namespace CineBuzzApi.Models
{
    public class PaymentRequest
    {
        
        /// Unique identifier for the payment request.
       public int CartId { get; set; }  
        public int PaymentRequestId { get; set; }

        
        /// Credit or debit card number used for payment.
      
        public required string CardNumber { get; set; }

        /// Expiration date of the card in MM/YY format.
 
        public required string ExpirationDate { get; set; }

   
        /// Name of the cardholder.
 
        public required string CardholderName { get; set; }

        /// Card Verification Code (CVC), typically a 3- or 4-digit code.
     
        public required string CVC { get; set; }

    
        /// Default constructor for PaymentGateway.
        
        public PaymentRequest() { }
    }
}