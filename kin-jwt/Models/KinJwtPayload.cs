namespace Kin.Jwt.Models
{
    public class KinJwtPayload
    {
        public string Name { get; }
        public object Data { get; protected set; }

        public KinJwtPayload(string name, string data)
        {
            Name = name;
            Data = data;
        }

        public KinJwtPayload(string name)
        {
            Name = name;
        }
    }

    public class KinJwtPayload<T> : KinJwtPayload where T : class
    {
        public KinJwtPayload(string name, T data) : base(name)
        {
            Data = data;
        }
    }
}