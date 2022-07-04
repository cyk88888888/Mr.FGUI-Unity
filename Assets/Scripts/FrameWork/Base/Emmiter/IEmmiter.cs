
public interface IEmmiter
{
    void Emit(string notificationName);
    void Emit(string notificationName, object[] body);
    void Emit(string notificationName, object[] body, object sender);

    void OnEmitter(string type, EventListenerDelegate listener);
    void UnEmitter(string type, EventListenerDelegate listener);
}
