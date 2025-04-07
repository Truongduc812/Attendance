//namespace iSoft.Common.Models.ConfigModel.Subs
//{
//    public class RabbitMQConfigModel
//    {
//        public string Address { get; set; }
//        public int Port { get; set; }
//        public string Username { get; set; }
//        public string Password { get; set; }

//        public RabbitMQConfigModel(string address, int port, string username, string password)
//        {
//            Address = address;
//            Port = port;
//            Username = username;
//            Password = password;
//        }

//        public override bool Equals(object? obj)
//        {
//            return obj != null &&
//                   Address == ((RabbitMQConfigModel)obj).Address &&
//                   Port == ((RabbitMQConfigModel)obj).Port &&
//                   Username == ((RabbitMQConfigModel)obj).Username &&
//                   Password == ((RabbitMQConfigModel)obj).Password;
//        }

//        public object GetLogStr()
//        {
//            return $"Address: {Address}, Port: {Port}, Username: {Username}, Password: *****";
//        }
//    }
//}