namespace Faker.Implementation;

public class Faker 
{
    public T Create<T>()
    {
        return (T) Create(typeof(T));
    }
    
    public object Create(Type t)
    {
        var random = new Random();
        byte[] buffer = new byte[10];
        random.NextBytes(buffer);
    }
}