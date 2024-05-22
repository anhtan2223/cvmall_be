namespace Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool del_flg { get; set; }
    }
}
