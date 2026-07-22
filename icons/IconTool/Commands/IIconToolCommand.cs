namespace IconTool.Commands
{
    public interface IIconToolCommand<TArgs>
    {
        public void Handle(TArgs args);
    }
}
