
public interface IBombManager
{
    void Initialize(IInputWrapper inputWrapper, float timeToAction, ObjectPool objectPool);
    void Tapped();
}
