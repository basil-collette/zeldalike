public interface ISavable
{
    public string ToJsonString();

    public void Load(string json);
}
