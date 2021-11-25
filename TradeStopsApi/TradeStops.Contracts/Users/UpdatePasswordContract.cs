namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with new password
    /// </summary>
    public class UpdatePasswordContract
    {
        /// <summary>
        /// Default empty constructor for serialization.
        /// </summary>
        public UpdatePasswordContract()
        {
        }

        /// <summary>
        /// Parameterized constructor for initialization.
        /// </summary>
        /// <param name="newPassword">New password</param>
        public UpdatePasswordContract(string newPassword)
        {
            NewPassword = newPassword;
        }

        /// <summary>
        /// New password to update
        /// </summary>
        public string NewPassword { get; set; }
    }
}