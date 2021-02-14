namespace ExtendedArithmetic
{
	/// <summary>
	/// The ICloneable&lt;T&gt; interface represents an object that can clone itself.
	/// The interface defines a single method, <see cref="ExtendedArithmetic.ICloneable{T}.Clone"/> 
	/// which is called to create a clone of the instance.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICloneable<T>
	{
		/// <summary>
		/// Returns a clone of this instance.
		/// </summary>
		T Clone();
	}
}
