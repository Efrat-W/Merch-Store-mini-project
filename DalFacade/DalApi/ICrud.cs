

namespace DalApi;

public interface ICrud<T> where T : struct
{
    public int Create(T t);

    public IEnumerable<T?> RequestAll(Func<T?, bool>? func = null);

    public T RequestById(int i);

    public T RequestByFunc(Func<T?, bool>? func);

    public void Update(T t);

    public void Delete(T t);


}