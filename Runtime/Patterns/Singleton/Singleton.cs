namespace LFramework
{
    /// <summary>
    /// Singleton pattern implementation.
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        static T s_instance;

        /// <summary>
        /// Returns a singleton class instance.
        /// </summary>
        public static T instance { get { return s_instance ?? (s_instance = new T()); } }

        public static void SetInstance(T instance)
        {
            s_instance = instance;
        }
    }
}