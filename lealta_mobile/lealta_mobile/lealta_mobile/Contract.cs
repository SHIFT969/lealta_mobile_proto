namespace lealta_mobile
{
    public class Contract
    {
        public int Id { get; set; }

        public string ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Rate { get; set; }
        public bool Frozen { get; set; }
        public string Adress { get; set; }
    }
}
