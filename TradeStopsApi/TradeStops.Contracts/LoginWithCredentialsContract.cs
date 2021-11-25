using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about user credentials and product that user is trying to log in.
    /// </summary>
    public class LoginWithCredentialsContract
    {
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public LoginWithCredentialsContract()
        {
        }

        /// <summary>
        /// Constructor to initialize all fields
        /// </summary>
        /// <param name="username">Username (usually email)</param>
        /// <param name="password">Password</param>
        /// <param name="product">Product where user is going to login (not really used anywhere currently)</param>
        public LoginWithCredentialsContract(string username, string password, Products product)
        {
            Credentials = new UserCredentialsContract
            {
                UserName = username,
                Password = password,
            };

            ProductId = product;
        }

        /// <summary>
        /// User credentials
        /// </summary>
        public UserCredentialsContract Credentials { get; set; }

        /// <summary>
        /// The product that user is logging in
        /// </summary>
        public Products ProductId { get; set; }
    }
}
