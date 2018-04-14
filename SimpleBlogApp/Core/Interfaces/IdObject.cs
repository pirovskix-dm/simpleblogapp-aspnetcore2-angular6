namespace SimpleBlogApp.Core.Interfaces
{
	public class IdObject<T>
	{
		public int Id { get; set; }
		public T Object { get; set; }
	}
}
